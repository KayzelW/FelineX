using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using APIServer.Database;
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
public class UserController : Controller
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<UserController> _logger;
    private readonly IConfiguration _configuration;

    public UserController(AppDbContext dbContext, ILogger<UserController> logger, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _logger = logger;
        _configuration = configuration;
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
            if (await _dbContext.Users!.FindAsync(_user.Id) != null)
            {
                return Conflict(_user);
            }
        }

        await _dbContext.Users!.AddAsync(_user);
        await _dbContext.SaveChangesAsync();

        return Ok(_user.Id);
    }

    [HttpGet("get_user/{id:guid}")]
    public async Task<ActionResult> GetUser(Guid id)
    {
        try
        {
            var user = _dbContext.Users!.FirstOrDefault(x => x.Id == id);
            return Ok(user);
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
        
        var user = await _dbContext.Users!.FirstOrDefaultAsync(x => x.UserName == auth.Login);
        if (user == null) throw new ArgumentNullException(nameof(user));
        
        if (user.PasswordHash == auth.HashedPassword)
        {
            return Ok(GenerateJwtToken(user.Id));
        }

        return NotFound(false);
    }

    [HttpGet("get_user_access_by_id/{id:guid}")]
    public async Task<ActionResult> GetUserAccessById(Guid id)
    {
        var user = await _dbContext.Users!.FirstOrDefaultAsync(x => x.Id == id);
        if (user != null)
        {
            return Ok(user.AccessFlags);
        }

        return NotFound();
    }

    [HttpGet("test_hash")]
    public async Task<string> TestHash(string password)
    {
        return await UserExtensions.HashPasswordAsync(password);
    }

    private string GenerateJwtToken(Guid userId)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(JWTClaimNames.UserId, userId.ToString())
            }),
            Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"])),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}