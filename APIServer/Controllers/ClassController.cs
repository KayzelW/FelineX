using APIServer.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.DB.User;
using Shared.Models;

namespace APIServer.Controllers;

[ApiController, Route("[controller]")]
public class ClassController : Controller
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<AuthController> _logger;

    public ClassController(AppDbContext dbContext, ILogger<AuthController> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    
    [HttpGet("get_classes")]
    public async Task<IActionResult> GetClasses()
    {
        var ownerId = (Guid)HttpContext.Items["User"]!;
        var groups = await _dbContext.Groups.Where(x => x.GroupCreatorId == ownerId).ToListAsync();
        return Ok(groups);
    }
    
    [HttpPost("add_student")]
    public async Task<IActionResult> AddUserToGroup(UserGroupDTO data)
    {
        try
        {
            var groupId = data.GroupId;
            var userId = data.UserId;
            if (groupId == Guid.Empty || userId == Guid.Empty)
            {
                return BadRequest("groupId or userId was null");
            }

            var group = await _dbContext.Groups.FirstOrDefaultAsync(x => x.Id == groupId);
            if (group == null)
            {
                return BadRequest("Group not found");
            }
            var student = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (student == null)
            {
                return BadRequest("Student not found");
            }
            group.Students?.Add(student);
            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError($"Exeption while adding student {data.UserId} to group {data.GroupId}", e);
            return BadRequest($"Exeption while adding student {data.UserId} to group {data.GroupId}");
        }
    }
    
    [HttpPost("add_group")]
    public async Task<IActionResult> AddGroup(UserGroup? group)
    {
        try
        {
            if (group == null)
            {
                return BadRequest("Group was null");
            }
            await _dbContext.Groups!.AddAsync(group);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError($"Exeption while adding group {e}");
            return BadRequest("Exeption while adding group");
        }
    }
}