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

    public TestController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("get_tests")]
    public async Task<ActionResult> GetTests()
    {
        var tests = await _dbContext.Tests.
                Include(x => x.Tasks).ToListAsync();
        return Ok(tests);
    }
    
    [HttpGet("get_test/{id:guid}")]
    public async Task<ActionResult> GetTest(Guid id)
    {
        var test = await _dbContext.Tests.Where(x => x.Id == id)
            .Include(x => x.Tasks)
            .ThenInclude(x => x.VariableAnswers)
            .FirstOrDefaultAsync();
        return Ok(test);
    }
    
    [HttpPost("create_test")]
    public async Task<ActionResult> CreateTest(Test? test)
    {
        if (test is null)
        {
            return BadRequest();
        }

        await _dbContext.Tests!.AddAsync(test);
        await _dbContext.SaveChangesAsync();
        
        return Ok();
    }
}