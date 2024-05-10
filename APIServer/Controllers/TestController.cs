using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using APIServer.Database;
using Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.DB.Classes;
using Shared.DB.Classes.Test;
using Shared.DB.Classes.Test.Task;
using Shared.DB.Classes.Test.Task.TaskAnswer;
using Shared.DB.Classes.User;
using Shared.Extensions;
using Swashbuckle.AspNetCore.Annotations;
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
                if (task.InteractionType is InteractionType.LongStringTask or InteractionType.ShortStringTask
                    or InteractionType.SqlQueryTask)
                {
                    task.VariableAnswers[0].StringAnswer = "";
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

    [HttpPost("create_test")]
    public async Task<IActionResult> CreateTest(Test? test)
    {
        if (test is null)
        {
            _logger.LogWarning(
                $"test is null while executing CreateTest from user");
            return BadRequest();
        }

        try
        {
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
    public async Task<IActionResult> SubmitTest(Test? solvedTest)
    {
        Test? test;
        var testAnswer = new TestAnswer();

        if (solvedTest?.Tasks is null)
        {
            _logger.LogError(
                $"test or tasks are null while executing CreateTest from user");
            return BadRequest();
        }

        try
        {
            testAnswer.AnsweredTestId = solvedTest.Id;

            foreach (var task in solvedTest.Tasks!)
            {
                if (task.VariableAnswers is not null)
                    _dbContext.AttachRange(task.VariableAnswers);

                var taskToSave = new TaskAnswer(solvedTest.CreatorId, task);
                // TODO: WTF???? WHY CREATOR IS SOLVER????

                testAnswer.TaskAnswers!.Add(taskToSave);
            }

            await _dbContext.TestAnswers!.AddAsync(testAnswer);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            _logger.LogWarning(e, $"Exception while saving new test from user");
            return BadRequest(solvedTest);
        }

        return Ok();
    }
}