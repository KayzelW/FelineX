using System.Text.Json;
using APIServer.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.DB.Test;
using Shared.DB.User;

namespace APIServer.Controllers;

public partial class TestController
{
    [HttpDelete("delete_test/{testId:guid}")]
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

    [HttpPost("create_test")]
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

    [HttpPost("edit_test")]
    public async Task<IActionResult> EditTest([FromBody] Test? incomingTest)
    {
        try
        {
            if (incomingTest is null)
            {
                return BadRequest("Test was null");
            }

            dbContext.Attach(incomingTest);
            UpdateTestUsers(incomingTest.Settings);

            dbContext.Update(incomingTest);

            await dbContext.SaveChangesAsync();

            // var existingTest = dbContext.Tests
            //     .Include(t => t.Settings)
            //     .ThenInclude(s => s.TestUsers)
            //     .Include(t => t.Tasks)
            //     .FirstOrDefault(t => t.Id == incomingTest.Id);
            //
            // if (existingTest != null)
            // {
            //     // Обновляем свойства вручную или с использованием автомаппера.
            //     dbContext.Entry(existingTest).CurrentValues.SetValues(incomingTest);
            //
            //     UpdateTasks(existingTest.Tasks, incomingTest.Tasks);
            //     
            //     // existingTest.Tasks = incomingTest.Tasks;
            //     
            //     UpdateTestUsers(existingTest.Settings.TestUsers, incomingTest.Settings.TestUsers);
            //
            //     dbContext.Update(existingTest);
            //
            //     await dbContext.SaveChangesAsync();
            //     
            // }
            // var json = JsonSerializer.Serialize(existingTest, new JsonSerializerOptions { WriteIndented = true });
            // _logger.LogInformation(json);
            //
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
        foreach (var existingTask in existingTasks)
        {
            if (!incomingTasks.Any(t => t.Id == existingTask.Id))
            {
                existingTasks.Remove(existingTask);
            }
        }

        foreach (var incomingTask in incomingTasks)
        {
            var existingTask = existingTasks.FirstOrDefault(t => t.Id == incomingTask.Id);
            if (existingTask != null)
            {
                dbContext.Entry(existingTask).CurrentValues.SetValues(incomingTask);
                foreach (var existingTaskVariableAnswer in existingTask.VariableAnswers)
                {
                    if (!incomingTask.VariableAnswers.Any(t => t.Id == existingTaskVariableAnswer.Id))
                    {
                        existingTask.VariableAnswers.Remove(existingTaskVariableAnswer);
                    }
                    else
                    {
                        existingTask.VariableAnswers = incomingTask.VariableAnswers;
                    }
                }
            }
            else
            {
                existingTasks.Add(incomingTask);
            }
        }
    }

    private void UpdateTestUsers(TestSettings settings)
    {
        var expectedIds = settings.TestUsers?.Select(x => x.Id).ToList();
        if (expectedIds == null)
        {
            return;
        }

        var existingIds = dbContext.TestSettings.First(x => x.Id == settings.Id).TestUsers?.Select(x => x.Id).ToList();
        if (existingIds == null || existingIds.Count == 0)
        {
            settings.TestUsers!.AddRange(dbContext.Users.Where(x => expectedIds!.Contains(x.Id)));
            return;
        }

        var mustToStay = existingIds.Intersect(expectedIds);
        var newUsers = expectedIds.Except(existingIds).ToList();

        settings.TestUsers!.RemoveAll(x => !mustToStay.Contains(x.Id));
        settings.TestUsers!.AddRange(dbContext.Users.Where(x => newUsers.Contains(x.Id)));
    }
}