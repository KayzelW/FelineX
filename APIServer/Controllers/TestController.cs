using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using APIServer.Database;
using Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.DB.Classes;
using Shared.DB.Classes.Test;
using Shared.DB.Classes.Test.Task;
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
            foreach (var ans in test.Tasks.SelectMany(task => task.VariableAnswers))
            {
                ans.Truthful = false;
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
}