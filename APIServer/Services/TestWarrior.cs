using System.Collections.Concurrent;
using APIServer.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Shared.DB.Test.Answers;
using Shared.DB.Test.Task;
using Shared.Types;
using Task = System.Threading.Tasks.Task;

namespace APIServer.Services;

public sealed partial class TestWarrior : BackgroundService, ITestWarriorQueue
{
    private AppDbContext dbContext;
    private readonly ILogger<TestWarrior> _logger;
    private readonly IConfiguration _configuration;
    private readonly IServiceProvider _serviceProvider;

    public static Dictionary<DBMS, string> AvailableDBMS = [];

    private readonly ConcurrentQueue<TestAnswer> _testAnswers;
    private readonly ConcurrentQueue<TaskAnswer> _sqlTasks;
    private const int BaseLimitThreads = 10;

    public TestWarrior(ILogger<TestWarrior> logger, IConfiguration configuration, IServiceProvider serviceProvider,
        CheckQueueService checkQueueService)
    {
        this._logger = logger;
        this._configuration = configuration;
        _serviceProvider = serviceProvider;

        _testAnswers = checkQueueService.TestAnswers;
        _sqlTasks = checkQueueService.SqlTasks;

        logger.LogInformation("TestWarrior instance created.");

        #region DatabaseModelsInit

        try
        {
            var fields = this._configuration.GetSection("Settings:TestDatabaseUrls");

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
        _logger.LogInformation($"Test registered in TestWarrior to {_testAnswers.GetHashCode()}");
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("TestWarrior service started.");
        // if (!int.TryParse(configuration["Settings:CountThreadsForTestChecking"], out var maxThreads))
        // {
        //     maxThreads = BaseLimitThreads;
        // }

        var workers = new List<Task>();

        #region TestAnswer worker

        workers.Add(Task.Run(async () =>
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    if (_testAnswers!.TryDequeue(out var testAnswer))
                    {
                        _logger.LogInformation($"Dequeued test answer: {testAnswer}");
                        CheckTasks(testAnswer);
                    }
                    else
                    {
                        await Task.Delay(100, stoppingToken);
                    }

                    // logger.LogInformation($"Current {_testAnswers.GetType()} count: {_testAnswers.Count}");
                    await Task.Delay(1000, stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in TestAnswer worker.");
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
                    await Task.Delay(1000, stoppingToken);
                }
            }
        }, stoppingToken));

        #endregion

        return Task.WhenAll(workers);
    }

    private void CheckTasks(TestAnswer answeredTest)
    {
        using var scope = _serviceProvider.CreateScope();
        dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var sqlTasksCount = 0;
        double taskCount = answeredTest.TaskAnswers!.Count;
        double taskWeight = 100.0 / taskCount;
        
        answeredTest.TaskWeight = taskWeight;
        double score = 0;

        foreach (var task in answeredTest.TaskAnswers)
        {
            if (task.AnsweredTask == null)
            {
                task.IsFailedCheck = true;
                continue;
            }

            dbContext.Entry(task.AnsweredTask).State = EntityState.Unchanged;
            if (task.AnsweredTask.IsSqlTask())
            {
                sqlTasksCount += 1;
                _sqlTasks.Enqueue(task);
                continue;
            }

            if (task.AnsweredTask.IsLongStringTask())
            {
                //TODO: merge with short string task
                if (task.StringAnswer == task.AnsweredTask.VariableAnswers!.FirstOrDefault()!.StringAnswer)
                {
                    score += answeredTest.TaskWeight;
                    task.IsSuccess = true;
                }

                task.IsCheckEnded = true;
                continue;
            }

            if (task.AnsweredTask.IsShortStringTask())
            {
                if (task.AnsweredTask.VariableAnswers!.Any(varAns => task.StringAnswer == varAns.StringAnswer))
                {
                    score += answeredTest.TaskWeight;
                    task.IsSuccess = true;
                }

                task.IsCheckEnded = true;
                continue;
            }

            var allMarkedVariablesMatch = task.MarkedVariables!
                .All(markedVar => task.AnsweredTask.VariableAnswers!
                    .Any(varAnswer => varAnswer.Id == markedVar.Id && varAnswer.Truthful == true));

            if (allMarkedVariablesMatch)
            {
                score += answeredTest.TaskWeight;
                task.IsSuccess = true;
            }
            
            task.IsCheckEnded = true;
        }

        if (score > 99.9)
        {
            answeredTest.Score = 100;
        }
        else
        {
            answeredTest.Score = Math.Round(score,2);
        }
        
        foreach (var testAnswerTaskAnswer in answeredTest.TaskAnswers)
        {
            if (testAnswerTaskAnswer.MarkedVariables == null)
                continue;

            foreach (var variable in testAnswerTaskAnswer.MarkedVariables)
            {
                dbContext.Entry(variable).State = EntityState.Unchanged;
            }
        }

        dbContext.Update(answeredTest);
        dbContext.SaveChanges();
    }
}