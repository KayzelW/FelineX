using APIServer.Database;
using Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.DB.Classes;
using Shared.DB.Classes.User;
using Shared.Extensions;
using Swashbuckle.AspNetCore.Annotations;

namespace APIServer.Controllers;

[ApiController, Route("[controller]")]
public class TestController : Controller
{
    private readonly AppDbContext _dbContext;

    public TestController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<ActionResult> GetTest()
    {
        // var test = await _dbContext.Tests
        //     .Include(x => x.Tasks)
        //     .Include(x => x.Creator)
        //     .FirstOrDefaultAsync();
        var test = await _dbContext.Tests.Include(x => x.Creator).FirstOrDefaultAsync();
        return Ok(test);
    }
}