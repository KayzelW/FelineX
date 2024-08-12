﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIServer.Controllers;

public partial class TestController
{
    [HttpGet("get_test_answer_id_by_id/{test_answer_id:guid}")]
    public async Task<IActionResult> GetTestAnswerId(Guid test_answer_id)
    {
        var answeredTest = await dbContext.TestAnswers.Where(x => x.Id == test_answer_id)
            .Include(x => x.TaskAnswers)
            .ThenInclude(x => x.MarkedVariables).OrderByDescending(entity => entity.Id)
            .FirstOrDefaultAsync();

        if (answeredTest is null)
        {
            return NotFound();
        }

        return Ok(answeredTest);
    }
    
    [HttpGet("get_test_result/{test_answer_id:guid}")]
    public async Task<IActionResult> GetTestResult(Guid test_answer_id)
    {
        try
        {
            var answeredTest = await dbContext.TestAnswers.Where(x => x.Id == test_answer_id)
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
}