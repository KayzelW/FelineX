using APIServer.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.CompilerServices;
using Shared.DB.Classes.User;
using Shared.Extensions;
using Shared.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace APIServer.Controllers;

[ApiController, Route("[controller]")]
public class UserController : Controller
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<UserController> _logger;

    public UserController(AppDbContext dbContext, ILogger<UserController> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }


    [HttpPost(Name = "PostUser")]
    [SwaggerOperation("Post a new user from UserDTO")]
    [SwaggerResponse(409, "Already Exists")]
    [SwaggerResponse(201, "Success")]
    public async Task<ActionResult> PostUser(UserDto user)
    {
        var _user = new User()
        {
            Id = user.Id,
            UserName = user.UserName,
            Access = AccessLevel.Student
        };
        if (_user.Id != null)
        {
            if (await _dbContext.Users.FindAsync(_user.Id) != null)
            {
                return Conflict(_user);
            }
        }

        await _dbContext.Users.AddAsync(_user);
        await _dbContext.SaveChangesAsync();

        return Ok(_user.Id);
    }

    [HttpGet("get_user")]
    public async Task<ActionResult> GetUser()
    {
        try
        {
            var user = _dbContext.Users!.FirstOrDefault();
            return Ok(user ?? null);
        }
        catch (Exception e)
        {
            _logger.Log(LogLevel.Debug, e, "There was an error while selecting a user");
        }

        return NotFound();
    }

    [HttpPost("auth")]
    public async Task<IActionResult> TryAuth(AuthData auth)
    {
        var _user = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserName == auth.Login);
        if (_user.PasswordHash == auth.HashedPassword)
        {
            return Ok(_user.Id);
        }

        return NotFound(false);
    }

    [HttpGet("test_hash")]
    public async Task<string> TestHash(string password)
    {
        return UserExtensions.HashPassword(password);
    }
}