using Microsoft.AspNetCore.Mvc;
using Shared.DB.Test;

namespace BlazorServer;

[ApiController, Route("api/[controller]")]
public class TestController : ControllerBase
{
    [HttpGet("get_tests")]
    public async Task<IActionResult> GetTests()
    {
        return Ok(new List<Test>()
        {
            new Test(),
            new Test(),
            new Test(),
            new Test(),
            new Test(),
            new Test(),
            new Test(),
        });
    }
}