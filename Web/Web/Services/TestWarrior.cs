using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using Shared.Data.Test.Answers;
using Shared.Types;
using Web.Services.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace Web.Services;

public sealed partial class TestWarrior : ITestWarriorQueue
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<TestWarrior> _logger;

    public static Dictionary<DBMS, string>? AvailableDBMS;

    private readonly ConcurrentQueue<TestAnswer> _testAnswers;
    private readonly ConcurrentQueue<TaskAnswer> _sqlTasks;

    public TestWarrior(ILogger<TestWarrior> logger)
    {
        _logger = logger;

        _testAnswers = new ConcurrentQueue<TestAnswer>();
        _sqlTasks = new ConcurrentQueue<TaskAnswer>();

        logger.LogInformation("TestWarrior instance created.");

        if (AvailableDBMS != null)
        {
            return;
        }

        #region DatabaseModelsInit

        try
        {
            AvailableDBMS = new Dictionary<DBMS, string>();
            // var fields = configuration.GetSection("Settings:TestDatabaseUrls");

            if (!AvailableDBMS.TryAdd(DBMS.SqLite, "DataSource=:memory:"))
            {
                logger.LogWarning("Не смог добавить SqLite в список доступных баз данных.");
            }

            // if (!string.IsNullOrEmpty(fields["mysql"]))
            // {
            //     AvailableDBMS.Add(DBMS.MySQL, fields["mysql"]!);
            // }
            //
            // if (!string.IsNullOrEmpty(fields["postgres"]))
            // {
            //     AvailableDBMS.Add(DBMS.PostgreSQL, fields["postgres"]!);
            // }
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

    public async Task ProcessTestAnswers()
    {
        while (_testAnswers.TryDequeue(out var testAnswer))
        {
            _logger.LogInformation($"Dequeued test answer: {testAnswer}");
            await CheckTasksAsync(testAnswer);
        }
    }

    public async Task ProcessSqlTasks()
    {
        while (_sqlTasks.TryDequeue(out var taskAnswer))
        {
            await CheckSqlQueryAsync(taskAnswer);
        }
    }

    private async Task CheckTasksAsync(TestAnswer answeredTest)
    {
        var sqlTasksCount = 0;
        var taskCount = answeredTest.TaskAnswers!.Count;
        var taskWeight = 100.0 / taskCount;

        answeredTest.TaskWeight = taskWeight;
        double score = 0;

        foreach (var task in answeredTest.TaskAnswers)
        {
            if (task.AnsweredTask == null)
            {
                task.IsFailedCheck = true;
                continue;
            }

            _dbContext.Entry(task.AnsweredTask).State = EntityState.Unchanged;
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
            answeredTest.Score = Math.Round(score, 2);
        }

        foreach (var testAnswerTaskAnswer in answeredTest.TaskAnswers)
        {
            if (testAnswerTaskAnswer.MarkedVariables == null)
                continue;

            foreach (var variable in testAnswerTaskAnswer.MarkedVariables)
            {
                _dbContext.Entry(variable).State = EntityState.Unchanged;
            }
        }

        _dbContext.Update(answeredTest);
        await _dbContext.SaveChangesAsync();
    }
}