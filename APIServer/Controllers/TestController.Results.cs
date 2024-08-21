using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIServer.Controllers;

public partial class TestController
{
    [HttpGet("get_test_answer_id_by_id/{testAnswerId:guid}")]
    public async Task<IActionResult> GetTestAnswerId(Guid testAnswerId)
    {
        var answeredTest = await dbContext.TestAnswers.Where(x => x.Id == testAnswerId)
            .Include(x => x.TaskAnswers)!
            .ThenInclude(x => x.MarkedVariables).OrderByDescending(entity => entity.Id)
            .FirstOrDefaultAsync();

        if (answeredTest == null)
        {
            return NotFound();
        }

        return Ok(answeredTest);
    }

    [HttpGet("get_test_result/{testAnswerId:guid}")]
    public async Task<IActionResult> GetTestResult(Guid testAnswerId)
    {
        try
        {
            var answeredTest = await dbContext.TestAnswers.Where(x => x.Id == testAnswerId)
                .Include(x => x.AnsweredTest)
                .ThenInclude(x => x.Tasks)
                .ThenInclude(x => x.VariableAnswers)
                .Include(x => x.TaskAnswers)
                .ThenInclude(x => x.MarkedVariables).OrderByDescending(entity => entity.Id)
                .Include(x => x.Student)
                .FirstOrDefaultAsync();

            if (answeredTest == null)
            {
                return NotFound("Test is null");
            }

            if (answeredTest.TaskAnswers == null)
            {
                _logger.LogWarning("TaskAnswers is null from database object");
                return BadRequest("TaskAnswers is null from database object");
            }

            if (answeredTest.TaskAnswers.All(x => x.IsCheckEnded) && answeredTest.TaskAnswers.Count != 0)
            {
                return Ok(answeredTest);
            }

            return NotFound("Test not checked yet");
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Fail to get score for test_answer with id {testAnswerId}");
            return NotFound();
        }
    }
}