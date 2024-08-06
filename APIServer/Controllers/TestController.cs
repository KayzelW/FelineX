using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using APIServer.Database;
using Microsoft.AspNetCore.Authentication;
using Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.DB.Classes;
using Shared.DB.Classes.Test;
using Shared.DB.Classes.Test.Task.TaskAnswer;
using Shared.DB.Classes.User;
using Shared.Extensions;
using MyTask = Shared.DB.Classes.Test.Task.Task;

namespace APIServer.Controllers;

[ApiController, Route("[controller]")]
public class TestController : Controller
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger _logger;

    public TestController(AppDbContext dbContext, ILogger<TestController> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    [HttpGet("get_tests")]
    [SuppressMessage("ReSharper.DPA", "DPA0011: High execution time of MVC action")]
    public async Task<IActionResult> GetTests()
    {
        var tests = await _dbContext.Tests!.Include(x => x.Tasks).ToListAsync();
        return Ok(tests);
    }

    [HttpGet("get_test/{id:guid}")]
    public async Task<IActionResult> GetTest(Guid id)
    {
        Test? test;
        try
        {
            test = await _dbContext.Tests.Where(x => x.Id == id)
                .Include(x => x.Tasks)
                .ThenInclude(x => x.VariableAnswers)
                .FirstOrDefaultAsync();
            foreach (var task in test.Tasks)
            {
                if (task.IsStringTask())
                {
                    task.VariableAnswers[0].StringAnswer = "";
                    task.VariableAnswers[0].Truthful = false;
                }
                else
                {
                    foreach (var ans in task.VariableAnswers!)
                    {
                        ans.Truthful = false;
                    }
                }
            }
        }
        catch
        {
            _logger.Log(LogLevel.Error, $"Fail to get test with id {id}");
            return NotFound(id);
        }

        return Ok(test);
    }


    [HttpGet("get_test_result/{test_answer_id:guid}")]
    public async Task<IActionResult> GetTestResult(Guid test_answer_id)
    {
        try
        {
            var answeredTest = await _dbContext.TestAnswers.Where(x => x.Id == test_answer_id)
                .Include(x => x.AnsweredTest)
                .ThenInclude(x => x.Tasks)
                .ThenInclude(x => x.VariableAnswers)
                .Include(x => x.TaskAnswers)
                .ThenInclude(x => x.MarkedVariables).OrderByDescending(entity => entity.Id)
                .FirstOrDefaultAsync();

            if (answeredTest is null)
            {
                return NotFound();
            }

            return Ok(answeredTest);
        }
        catch
        {
            _logger.Log(LogLevel.Error, $"Fail to get score for test_answer with id {test_answer_id}");
            return NotFound();
        }
    }

    private async Task<double> CalculateScore(TestAnswer answeredTest)
    {
        var taskWeight = 100 / answeredTest.TaskAnswers.Count;

        var score = 0;

        foreach (var task in answeredTest.TaskAnswers)
        {
            var correctTask = await _dbContext.Tasks.Where(x => x.Id == task.AnsweredTaskId)
                .AsNoTracking()
                .Include(x => x.VariableAnswers)
                .FirstOrDefaultAsync();
            if (correctTask!.IsStringTask())
            {
                if (task.StringAnswer == correctTask.VariableAnswers.FirstOrDefault().StringAnswer)
                {
                    score += taskWeight;
                }
            }
            else
            {
                var allMarkedVariablesMatch = task.MarkedVariables!
                    .All(markedVar => correctTask.VariableAnswers!
                        .Any(varAnswer => varAnswer.Id == markedVar.Id && varAnswer.Truthful == true));

                if (allMarkedVariablesMatch)
                {
                    score += taskWeight;
                }
            }
        }

        _logger.LogInformation("Calculated score inside CalculateScore: {Score}", score);
        return score;
    }


    [HttpGet("get_list_students_testanswers/{test_id:guid}")]
    public async Task<IActionResult> GetListStudentsTestAnswers(Guid test_id)
    {
        try
        {
            var listTestAnswers = await _dbContext.TestAnswers!.Where(x => x.AnsweredTestId == test_id)
                .Include(x => x.Student)
                .ToListAsync();
            foreach (var test in listTestAnswers)
            {
            }

            return Ok(listTestAnswers);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Exception while getting list of students for test with id {test_id}");
            return BadRequest();
        }
    }

    [HttpGet("get_test_answer_id_by_id/{test_answer_id:guid}")]
    public async Task<IActionResult> GetTestAnswerId(Guid test_answer_id)
    {
        var answeredTest = await _dbContext.TestAnswers.Where(x => x.Id == test_answer_id)
            .Include(x => x.TaskAnswers)
            .ThenInclude(x => x.MarkedVariables).OrderByDescending(entity => entity.Id)
            .FirstOrDefaultAsync();

        if (answeredTest is null)
        {
            return NotFound();
        }

        return Ok(answeredTest);
    }

    [HttpPost("create_test")]
    public async Task<IActionResult> CreateTest(Test? test)
    {
        if (test is null)
        {
            _logger.LogInformation(
                $"test is null while executing CreateTest from user");
            return BadRequest();
        }
        
        try
        {
            var userId = (Guid)HttpContext.Items["User"]!;
            test.CreatorId = userId;
            if (test.Tasks is not null)
            {
                foreach (var task in test.Tasks)
                {
                    task.CreatorId = test.CreatorId;
                }
            }
            
            await _dbContext.Tests!.AddAsync(test);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Exception while saving new test from user");
            return BadRequest(test);
        }

        return Ok();
    }


    [HttpPost("submit_test")]
    public async Task<IActionResult> SubmitTest(TestDTO? solvedTest)
    {
        Test? test;
        var testAnswer = new TestAnswer();

        if (solvedTest?.Tasks is null)
        {
            _logger.LogError(
                $"Test or tasks are null while executing SubmitTest");
            return BadRequest();
        }

        _logger.LogCritical($"Items User is {HttpContext.Items["User"]}");
        solvedTest.StudentId = (Guid)HttpContext.Items["User"]!;

        try
        {
            testAnswer.AnsweredTestId = solvedTest.Id;

            testAnswer.PassingDate = DateTime.Now;
            if (solvedTest.StudentId != Guid.Empty && solvedTest.StudentId != null)
            {
                testAnswer.StudentId = solvedTest.StudentId;
                testAnswer.Student =
                    await _dbContext.Users.Where(x => x.Id == solvedTest.StudentId).FirstOrDefaultAsync();
            }
            else
            {
                testAnswer.FantomName = solvedTest.FantomName;
            }

            foreach (var task in solvedTest.Tasks!)
            {
                if (task is not null)
                    _dbContext.Attach(task);

                var taskToSave = new TaskAnswer(solvedTest.CreatorId, task);

                testAnswer.TaskAnswers!.Add(taskToSave);
            }

            var score = await CalculateScore(testAnswer);
            _logger.LogInformation("Calculated score: {Score}", score);

            testAnswer.Score = score;

            await _dbContext.TestAnswers!.AddAsync(testAnswer);
            await _dbContext.SaveChangesAsync();

            return Ok(testAnswer.Id);
        }
        catch (Exception e)
        {
            _logger.LogInformation(e, $"Exception while saving new test from user");
            return BadRequest(solvedTest);
        }
    }
}