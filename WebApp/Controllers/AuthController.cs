using Microsoft.AspNetCore.Mvc;
using WebApp.Services;

namespace WebApp.Controllers;

public class AuthController : Controller
{
    private readonly ILogger<AuthController> _logger;
    private readonly AuthService _authService;


    public AuthController(ILogger<AuthController> logger, AuthService authService)
    {
        _logger = logger;
        _authService = authService;
    }

    [HttpGet("api/setcookie/{userId:guid}")]
    public async Task<IActionResult> SetUserIdCookie(Guid? userId)
    {
        _logger.LogInformation("SetUserIdCookie in controller");
        Response.Cookies.Append("UserId", userId.ToString());
        return Redirect("/profile");
    }
}