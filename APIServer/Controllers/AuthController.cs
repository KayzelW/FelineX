using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using APIServer.Database;
using APIServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic.CompilerServices;
using Shared.DB.Classes.User;
using Shared.Extensions;
using Shared.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace APIServer.Controllers;

[ApiController, Route("[controller]")]
public class AuthController : Controller
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<AuthController> _logger;
    private readonly TokenService _tokenService;

    public AuthController(AppDbContext dbContext, ILogger<AuthController> logger, TokenService tokenService)
    {
        _dbContext = dbContext;
        _logger = logger;
        _tokenService = tokenService;
    }

    [HttpPatch(Name = "SendMessage")]
    public async Task<ActionResult> SendMessage([FromBody] string msg)
    {
        _logger.LogInformation(msg);
        return Ok();
    }

    // [HttpPost(Name = "PostUser")]
    // [SwaggerOperation("Post a new user from UserDTO")]
    // [SwaggerResponse(409, "Already Exists")]
    // [SwaggerResponse(201, "Success")]
    // public async Task<ActionResult> PostUser(UserDto user)
    // {
    //     var _user = new User()
    //     {
    //         Id = user.Id,
    //         UserName = user.UserName,
    //         Access = AccessLevel.Student
    //     };
    //     if (_user.Id != null)
    //     {
    //         if (await _dbContext.Users!.FindAsync(_user.Id) != null)
    //         {
    //             return Conflict(_user);
    //         }
    //     }
    //
    //     await _dbContext.Users!.AddAsync(_user);
    //     await _dbContext.SaveChangesAsync();
    //
    //     return Ok(_user.Id);
    // }

    [HttpPost("auth")]
    public async Task<IActionResult> TryAuth([FromBody]AuthData auth)
    {
        if (auth.HashedPassword == null || auth.Login == null)
            return NotFound();

        var user = await _dbContext.Users!.FirstOrDefaultAsync(x =>
            x.UserName == auth.Login && x.PasswordHash == auth.HashedPassword);
        if (user == null) return NotFound();

        var token = _tokenService.RegisterSession(user.Id);
                
        return Ok(new AuthAnswer()
        {
            UserToken = token,
            UserName = user.NormalizedUserName,
            Access = user.AccessFlags,
        });
    }

    [HttpPost("authtoken")]
    public async Task<IActionResult> TryAuthByToken([FromBody] string token)
    {
        _logger.LogInformation(
            $"{HttpContext.Connection.RemoteIpAddress} => {HttpContext.Connection.LocalIpAddress}:\n {HttpContext.Request}");
        
        if (!_tokenService.TryGetUserId(token, out var userId)) return NotFound();

        var user = await _dbContext.Users!.FirstOrDefaultAsync(x => x.Id == userId);
        if (user == null)
        {
            _tokenService.RemoveToken(token);
            return NotFound("User doesn't exists, but token exists....");
        }
        
        return Ok(new AuthAnswer()
        {
            UserToken = token,
            UserName = user?.NormalizedUserName,
            Access = user?.AccessFlags,
        });
    }

    [HttpGet("test_hash")]
    public async Task<string> TestHash(string password)
    {
        return await UserExtensions.HashPasswordAsync(password);
    }
}