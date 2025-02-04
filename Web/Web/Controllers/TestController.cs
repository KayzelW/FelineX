﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Data.Test;
using Shared.Data.Test.Answers;
using Web.Extensions;
using Web.Services;
using Web.Services.Interfaces;
using TestDTO = Web.Controllers.Models.TestDTO;

namespace Web.Controllers;

[ApiController, Route("api/[controller]"), Authorize]
public partial class TestController(
    AppDbContext dbContext,
    ILogger<TestController> logger,
    ITestWarriorQueue testWarrior)
    : Controller
{
    private readonly ILogger _logger = logger;

    /// <summary>
    /// Must be used when Teacher trying to see all available tests 
    /// </summary>
    /// <returns>list of <see cref="Test"/></returns>
    [HttpGet("get_tests"), Authorize(Policy = "Teacher")]
    public async Task<ActionResult<IEnumerable<UniqueTest>>> GetTests()
    {
        var userId = this.GetUserId().ToString();

        var tests = await dbContext.Tests
            .Where(t =>
                // t.Settings.TestUsers!.Any(u => u.Id == userId) ||
                // t.Settings.TestGroups!.Any(g => g.Students!.Any(u => u.Id == userId)) ||
                t.CreatorId == userId)
            .ToListAsync();

        return Ok(tests);
    }

    /// <summary>
    /// Calls when user try to load specific test
    /// </summary>
    /// <param name="id"></param>
    /// <returns><see cref="Test"/>></returns>
    [HttpGet("get_test_for_solving/{id:guid}"), AllowAnonymous]
    public async Task<IActionResult> GetTestForSolving(Guid id)
    {
        UniqueTest? test;
        try
        {
            test = await dbContext.Tests.AsNoTrackingWithIdentityResolution().FirstOrDefaultAsync(x => x.Id == id);
            if (test == null)
                return NotFound(id);

            foreach (var task in test.Tasks)
            {
                task.Settings.SqlQueryCheck = "";
                task.Settings.SqlQueryInstall = "";
                if (task.IsLongStringTask() || task.IsShortStringTask())
                {
                    foreach (var varAns in task.VariableAnswers!)
                    {
                        varAns.StringAnswer = "";
                    }
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

    [HttpGet("get_original_test/{id:guid}"), Authorize(Roles = "Teacher,Admin")]
    public async Task<IActionResult?> GetOriginalTest(Guid id)
    {
        var test = await dbContext.Tests
            .AsNoTrackingWithIdentityResolution()
            .Include(x => x.Settings)
            .FirstOrDefaultAsync(x => x.Id == id);
        return Ok(test);
    }

    [HttpGet("get_list_students_test_answers/{testId:guid}"), Authorize(Roles = "Teacher,Admin")]
    public async Task<IActionResult> GetListStudentsTestAnswers(Guid testId)
    {
        try
        {
            var listTestAnswers = await dbContext.TestAnswers!.Where(x => x.AnsweredTestId == testId)
                .Include(x => x.Student)
                .ToListAsync();
            return Ok(listTestAnswers);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Exception while getting list of students for test with id {testId}");
            return BadRequest();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="solvedTest"></param>
    /// <returns> TestAnswer id as <see cref="Guid"/></returns>
    [HttpPost("submit_test"), AllowAnonymous]
    public async Task<ActionResult<Guid>> SubmitTest([FromBody] TestDTO solvedTest)
    {
        if (solvedTest?.Tasks == null)
        {
            _logger.LogError($"Test or tasks are null while executing SubmitTest");
            return BadRequest("Object is null");
        }

        try
        {
            var testAnswer = new TestAnswer();
            testAnswer.ClientConnectionLog = AuthExtensions.GetConnectionLog(HttpContext);

            dbContext.Add(testAnswer);
            await dbContext.SaveChangesAsync();

            if (!string.IsNullOrEmpty(solvedTest.FantomName))
            {
                testAnswer.FantomName = solvedTest.FantomName;
            }
            else
            {
                testAnswer.StudentId = this.GetUserId().ToString()!;
            }

            testAnswer.PassingDate = DateTime.Now; // fill pass date
            testAnswer.AnsweredTestId = solvedTest.Id; // fill testId

            // fill list of tasksAnswers 
            foreach (var task in solvedTest.Tasks)
            {
                var taskAnswer = new TaskAnswer(testAnswer.StudentId, task); //TODO: get studentId from .NET session
                var originalTask = await dbContext
                    .Tasks
                    .Include(x => x.VariableAnswers)
                    .AsNoTrackingWithIdentityResolution()
                    .FirstOrDefaultAsync(x => x.Id == task.Id);

                if (originalTask == null)
                {
                    logger.LogInformation($"OriginalTask for {taskAnswer.StudentId}:{solvedTest.FantomName} is null");
                    continue;
                }

                dbContext.Entry(originalTask).State = EntityState.Unchanged;

                taskAnswer.TestAnswer = testAnswer;
                taskAnswer.AnsweredTask = originalTask;
                testAnswer.TaskAnswers!.Add(taskAnswer);
            }

            testWarrior.RegisterTestAnswer(testAnswer);

            return Ok(testAnswer.Id);
        }
        catch (Exception e)
        {
            _logger.LogInformation(e, $"Exception while saving new testAnswer from user");
            return BadRequest(solvedTest);
        }
    }
}