using APIServer.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.DB.Test;

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
}