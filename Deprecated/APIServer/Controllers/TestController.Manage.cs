﻿using System.Text.Json;
using APIServer.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Attributes;
using Shared.DB.Test;
using Shared.Types;

namespace APIServer.Controllers;

public partial class TestController
{
    [HttpDelete("delete_test/{testId:guid}"), AuthorizeLevel(AccessLevel.Teacher)]
    public async Task<IActionResult> DeleteTest(Guid testId)
    {
        try
        {
            var test = await dbContext.Tests.FirstOrDefaultAsync(x => x.Id == testId);

            if (test == null)
            {
                return NotFound("testId doesn't exists in database or already deleted");
            }

            dbContext.Remove(test);
            await dbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            _logger.LogError($"Delete error test: {testId}", e);
            return BadRequest($"Delete error test: {testId}");
        }

        return Ok();
    }

    [HttpPost("create_test"), AuthorizeLevel(AccessLevel.Teacher)]
    public async Task<IActionResult> CreateTest([FromBody] Test? test)
    {
        if (test is null)
        {
            _logger.LogInformation($"test is null while executing CreateTest from user");
            return BadRequest();
        }

        try
        {
            foreach (var settingsTestGroup in test.Settings.TestGroups)
            {
                dbContext.Entry(settingsTestGroup).State = EntityState.Unchanged;
            }

            foreach (var settingsTestUser in test.Settings.TestUsers)
            {
                dbContext.Entry(settingsTestUser).State = EntityState.Unchanged;
            }

            var userId = (Guid)HttpContext.Items["User"]!;
            test.CreatorId = userId;
            if (test.Tasks is not null)
            {
                foreach (var task in test.Tasks)
                {
                    task.CreatorId = test.CreatorId;
                    if (task.IsSqlTask())
                    {
                        task.DataRows = await TaskExtension.SetupAndFetch(task);
                    }
                }
            }

            await dbContext.Tests.AddAsync(test);
            await dbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Exception while saving new test from user");
            return BadRequest(test);
        }

        return Ok();
    }

    [HttpPost("edit_test"), AuthorizeLevel(AccessLevel.Teacher)]
    public async Task<IActionResult> EditTest([FromBody] Test? incomingTest)
    {
        try
        {
            if (incomingTest is null)
            {
                return BadRequest("Test was null");
            }

            var existingTest = dbContext.Tests.FirstOrDefault(t => t.Id == incomingTest.Id);
            if (existingTest != null)
            {
                UpdateTestUsers(existingTest.Settings, incomingTest.Settings);
                UpdateTasks(existingTest.Tasks!, incomingTest.Tasks!);

                existingTest.TestName = incomingTest.TestName;
                //TODO: existingTest.Settings 
                
                dbContext.Update(existingTest);

                await dbContext.SaveChangesAsync();
            }

            var json = JsonSerializer.Serialize(existingTest, new JsonSerializerOptions { WriteIndented = true });
            _logger.LogInformation(json);

            return Ok($"Test with id {incomingTest.Id} was modified");
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError($"Concurrency issue while editing test {ex}");
            return Conflict("There was a concurrency issue");
        }
        catch (Exception e)
        {
            _logger.LogError($"Exception while editing test {e}");
            return BadRequest(e);
        }
    }

    private void UpdateTasks(List<Shared.DB.Test.Task.Task> existingTasks, List<Shared.DB.Test.Task.Task> incomingTasks)
    {
        foreach (var existingTask in existingTasks.ToList())
        {
            if (!incomingTasks.Any(t => t.Id == existingTask.Id))
            {
                existingTasks.Remove(existingTask);
            }
        }

        foreach (var incomingTask in incomingTasks)
        {
            var existingTask = existingTasks.FirstOrDefault(t => t.Id == incomingTask.Id);
            if (existingTask == null)
            {
                existingTasks.Add(incomingTask);
                continue;
            }

            existingTask.Question = incomingTask.Question;
            existingTask.DatabaseType = incomingTask.DatabaseType;
            // existingTask.CreatorId = incomingTask.CreatorId;
            existingTask.Thematics!.SyncList(incomingTask.Thematics!);
            
            foreach (var inVarAns in incomingTask.VariableAnswers)
            {
                var exVarAns = existingTask.VariableAnswers.FirstOrDefault(x => x.Id == inVarAns.Id);
                if (exVarAns != null)
                {
                    exVarAns.StringAnswer = inVarAns.StringAnswer;
                    exVarAns.Truthful = inVarAns.Truthful;
                    continue;
                }

                existingTask.VariableAnswers.Add(inVarAns);
            }
        }
    }

    private void UpdateTestUsers(TestSettings existingSettings, in TestSettings settings)
    {
        var expectedIds = settings.TestUsers?.Select(x => x.Id).ToList();
        if (expectedIds == null)
        {
            existingSettings.TestUsers!.Clear();
            return;
        }

        var existingIds = existingSettings.TestUsers?.Select(x => x.Id).ToList();
        if (existingIds == null || existingIds.Count == 0)
        {
            existingSettings.TestUsers!.AddRange(dbContext.Users.Where(x => expectedIds.Contains(x.Id)));
            return;
        }

        var mustToStay = existingIds.Intersect(expectedIds);
        var newUsers = expectedIds.Except(existingIds);

        existingSettings.TestUsers!.RemoveAll(x => !mustToStay.Contains(x.Id));
        existingSettings.TestUsers!.AddRange(dbContext.Users.Where(x => newUsers.Contains(x.Id)));
    }
}