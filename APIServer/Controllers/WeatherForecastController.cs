using Microsoft.AspNetCore.Mvc;

namespace APIServer.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : Controller
{
    [HttpGet(Name = "GetTestData")]
    public string Get()
    {
        return "cringe";
    }
}