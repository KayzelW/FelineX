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
using FinishedTest = Shared.DB.Classes.Test.Task.TaskAnswer.TestAnswer;
using MyTaskAnswer = Shared.DB.Classes.Test.Task.TaskAnswer.TaskAnswer;
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
                $"test is null while executing CreateTest from user with session: {HttpContext.Session.Id}");
            return BadRequest();
        }

        try
        {
            await _dbContext.Tests!.AddAsync(test);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            _logger.LogWarning(e, $"Exception while saving new test from user with session: {HttpContext.Session.Id}");
            return BadRequest(test);
        }

        return Ok();
    }

    [HttpPost("submit_test")]
    public async Task<IActionResult> SubmitTest(Test? submitedTest)
    {
        Test? test;
        var FinishedTest = new FinishedTest();
        if (submitedTest is null)
        {
            _logger.LogWarning(
                $"test is null while executing CreateTest from user with session: {HttpContext.Session.Id}");
            return BadRequest();
        }

        try
        {
            test = await _dbContext.Tests.Where(x => x.Id == submitedTest.Id)
                .Include(x => x.Tasks)
                .ThenInclude(x => x.VariableAnswers).Include(test => test.Creator)
                .FirstOrDefaultAsync();

            FinishedTest.AnsweredTest = test;

            foreach (var submitedTask in submitedTest.Tasks)
            {
                var taskToSave = new MyTaskAnswer
                {
                    Student = test.Creator,
                    StudentId = test.Creator.Id,
                    AnsweredTaskId = submitedTask.Id,
                    AnsweredTask = submitedTask,
                    GotVariables = submitedTask.VariableAnswers
                };
                if (submitedTask.InteractionType is InteractionType.LongStringTask or InteractionType.ShortStringTask
                    or InteractionType.SqlQueryTask)
                {
                    taskToSave.StringAnswer = submitedTask.VariableAnswers[0].StringAnswer;
                }
                else
                {
                    foreach (var varAns in submitedTask.VariableAnswers)
                    {
                        if (varAns.Truthful is true)
                        {
                            taskToSave.MarkedVariables!.Add(varAns);
                        }
                    }
                }

                FinishedTest.TaskAnswers.Add(taskToSave);
            }


            await _dbContext.TestAnswers.AddAsync(FinishedTest);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            _logger.LogWarning(e, $"Exception while saving new test from user with session: {HttpContext.Session.Id}");
            return BadRequest(submitedTest);
        }

        return Ok();
    }
}