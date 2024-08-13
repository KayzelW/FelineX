using System.Diagnostics.CodeAnalysis;
using APIServer.Database;
using APIServer.Services;
using Microsoft.AspNetCore.Authorization;
using Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.DB.Test;
using Shared.DB.Test.Answers;

namespace APIServer.Controllers;

[ApiController, Route("[controller]")]
public partial class TestController(AppDbContext dbContext, ILogger<TestController> logger, ITestWarriorQueue testWarrior)
    : Controller
{
    private readonly ILogger _logger = logger;
    private readonly ITestWarriorQueue _testWarrior = testWarrior;

    /// <summary>
    /// Must be used when Teacher trying to see all available tests 
    /// </summary>
    /// <returns>list of <see cref="Test"/></returns>
    [HttpGet("get_tests")]
    public async Task<IActionResult> GetTests()
    {
        var tests = await dbContext.Tests.ToListAsync();
        return Ok(tests);
    }

    /// <summary>
    /// Calls when user try to load specific test
    /// </summary>
    /// <param name="id"></param>
    /// <returns><see cref="Test"/>></returns>
    [HttpGet("get_test/{id:guid}")]
    public async Task<IActionResult> GetTest(Guid id)
    {
        Test? test;
        try
        {
            test = await dbContext.Tests.Where(x => x.Id == id)
                .Include(x => x.Tasks)
                .ThenInclude(x => x.VariableAnswers)
                .FirstOrDefaultAsync();

            if (test == null)
                return NotFound(id);

            foreach (var task in test.Tasks)
            {
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

    [HttpGet("get_list_students_test_answers/{testId:guid}")]
    public async Task<IActionResult> GetListStudentsTestAnswers(Guid testId)
    {
        try
        {
            var listTestAnswers = await dbContext.TestAnswers!.Where(x => x.AnsweredTestId == testId)
                .Include(x => x.Student)
                .ToListAsync();
            foreach (var test in listTestAnswers)
            {
            }

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
    [AllowAnonymous, HttpPost("submit_test")]
    public async Task<ActionResult<Guid>> SubmitTest([FromBody]TestDTO solvedTest)
    {
        if (solvedTest?.Tasks == null)
        {
            _logger.LogError($"Test or tasks are null while executing SubmitTest");
            return BadRequest("Object is null");
        }

        try
        {
            var testAnswer = new TestAnswer();
            // fill studentId or fantomName if first is null
            if (!string.IsNullOrEmpty(solvedTest.FantomName))
            {
                testAnswer.FantomName = solvedTest.FantomName;
            }
            else
            {
                testAnswer.StudentId = (Guid)HttpContext.Items["User"]!;
            }

            testAnswer.PassingDate = DateTime.Now; // fill pass date
            testAnswer.AnsweredTestId = solvedTest.Id; // fill testId

            
            
            // fill list of tasksAnswers 
            foreach (var task in solvedTest.Tasks)
            {
                foreach (var taskVariableAnswer in task.VariableAnswers)
                {
                    dbContext.Entry(taskVariableAnswer).State = EntityState.Unchanged;
                }//TODO fix cringe State
                var taskAnswer = new TaskAnswer(testAnswer.StudentId, task);
                testAnswer.TaskAnswers!.Add(taskAnswer);
                taskAnswer.TestAnswer = testAnswer;
            }

            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            var userAgent = HttpContext.Request.Headers.UserAgent.Append($"IP:{ipAddress};");
            var userInfo = userAgent.ToString();
            // var userAgentParser = new UserAgentParser();
            // var clientInfo = userAgentParser.Parse(userAgent);
            // var operatingSystem = clientInfo.OS.Name;
            // var device = clientInfo.Device.Family;
            //var logString = $"Браузер: {userAgent}, IP: {ipAddress}, ОС: {operatingSystem}, Устройство: {device}";

            testAnswer.ClientConnectionLog = userInfo ?? "failed parse";

            _testWarrior.RegisterTestAnswer(testAnswer);

            // var score = await CalculateScore(testAnswer);

            await dbContext.TestAnswers!.AddAsync(testAnswer);
            await dbContext.SaveChangesAsync();

            return Ok(testAnswer.Id);
        }
        catch (Exception e)
        {
            _logger.LogInformation(e, $"Exception while saving new testAnswer from user");
            return BadRequest(solvedTest);
        }
    }
}