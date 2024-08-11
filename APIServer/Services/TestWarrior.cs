using System.Collections.Concurrent;
using APIServer.Database;
using Microsoft.EntityFrameworkCore;
using Shared.DB.Test.Answers;
using Shared.Types;
using Task = System.Threading.Tasks.Task;

namespace APIServer.Services;

public sealed partial class TestWarrior : BackgroundService
{
    private AppDbContext dbContext;
    private ILogger<TestWarrior> logger;
    private IConfiguration configuration;

    public static Dictionary<DBMS, string> AvailableDBMS = [];

    private ConcurrentQueue<TestAnswer> _testAnswers = [];
    private ConcurrentQueue<SqlTaskPare> _sqlTasks = [];
    private const int BaseLimitThreads = 10;

    public TestWarrior(AppDbContext dbContext, ILogger<TestWarrior> logger, IConfiguration configuration)
    {
        this.dbContext = dbContext;
        this.logger = logger;
        this.configuration = configuration;

        #region DatabaseModelsInit

        try
        {
            var fields = this.configuration.GetSection("Settings:TestDatabaseUrls");

            AvailableDBMS.Add(DBMS.SqLite, "DataSource=:memory:");

            if (!string.IsNullOrEmpty(fields["mysql"]))
            {
                AvailableDBMS.Add(DBMS.MySQL, fields["mysql"]!);
            }

            if (!string.IsNullOrEmpty(fields["postgres"]))
            {
                AvailableDBMS.Add(DBMS.PostgreSQL, fields["postgres"]!);
            }
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex,
                "Fields for |Settings:TestDatabaseUrls| are empty or something were wrong while read fields");
        }

        #endregion
    }

    public void RegisterTestAnswer(TestAnswer test)
    {
        _testAnswers.Enqueue(test);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!int.TryParse(configuration["Settings:CountThreadsForTestChecking"], out var maxThreads))
        {
            maxThreads = BaseLimitThreads;
        }

        var workers = new List<Task>();

        #region TestAnswer worker

        workers.Add(Task.Run(async () =>
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_testAnswers.TryDequeue(out var testAnswer))
                {
                    CheckTasks(testAnswer);
                }
                else
                {
                    await Task.Delay(100, stoppingToken);
                }
            }
        }, stoppingToken));

        #endregion

        #region SqlTask Worker

        workers.Add(Task.Run(async () =>
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_sqlTasks.TryDequeue(out var taskAnswer))
                {
                    CheckSqlQuery(taskAnswer);
                }
                else
                {
                    await Task.Delay(100, stoppingToken);
                }
            }
        }, stoppingToken));

        #endregion

        return Task.WhenAll(workers);
    }

    private async void CheckTasks(TestAnswer answeredTest)
    {
        var sqlTasksCount = 0;
        var taskCount = answeredTest.TaskAnswers!.Count;

        answeredTest.TaskWeight = 100 / taskCount; //TODO: fix var when score can't be 100
        var score = 0;

        foreach (var task in answeredTest.TaskAnswers)
        {
            var originalTask = await dbContext.Tasks!.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == task.AnsweredTaskId);
            if (originalTask == null)
            {
                logger.LogInformation($"originalTask for {task.StudentId}:{answeredTest.FantomName} is null");
                continue;
            }

            if (originalTask.IsSqlTask())
            {
                sqlTasksCount += 1;
                _sqlTasks.Enqueue(new SqlTaskPare()
                {
                    OriginalTask = originalTask,
                    TaskAnswer = task,
                });
                continue;
            }

            if (originalTask.IsLongStringTask())
            {
                //TODO: merge with short string task
                if (task.StringAnswer == originalTask.VariableAnswers!.FirstOrDefault()!.StringAnswer)
                {
                    score += answeredTest.TaskWeight;
                }

                continue;
            }

            if (originalTask.IsShortStringTask())
            {
                if (originalTask.VariableAnswers!.Any(varAns => task.StringAnswer == varAns.StringAnswer))
                {
                    score += answeredTest.TaskWeight;
                }

                continue;
            }

            var allMarkedVariablesMatch = task.MarkedVariables!
                .All(markedVar => originalTask.VariableAnswers!
                    .Any(varAnswer => varAnswer.Id == markedVar.Id && varAnswer.Truthful == true));

            if (allMarkedVariablesMatch)
            {
                score += answeredTest.TaskWeight;
            }
        }

        dbContext.Add(answeredTest);
        await dbContext.SaveChangesAsync();
    }
}
