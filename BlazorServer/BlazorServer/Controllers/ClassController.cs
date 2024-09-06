using System.Text.RegularExpressions;
using BlazorServer.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Attributes;
using Shared.DB.User;
using Shared.Models;
using Task = Shared.DB.Test.Task.Task;

namespace BlazorServer.Controllers;

[ApiController, Route("[controller]"), AuthorizeLevel(AccessLevel.Teacher)]
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
            _logger.LogError($"Exсeption while adding student {data.UserId} to group {data.GroupId}", e);
            return BadRequest($"Exсeption while adding student {data.UserId} to group {data.GroupId}");
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

            group.GroupCreatorId = (Guid)HttpContext.Items["User"]!;
            await _dbContext.Groups!.AddAsync(group);
            foreach (var groupStudent in group.Students!)
            {
                _dbContext.Entry(groupStudent).State = EntityState.Unchanged;
            }

            await _dbContext.SaveChangesAsync();
            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError($"Exсeption while adding group {e}");
            return BadRequest("Exсeption while adding group");
        }
    }

    [HttpGet("get_students")]
    public async Task<IActionResult> GetStudents()
    {
        try
        {
            var students = _dbContext.Users
                .Where(x => x.AccessFlags == (uint)AccessLevel.Student)
                .Select(x => new { x.Id, x.UserName, x.NormalizedUserName })
                .ToList();
            return Ok(students);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Exсeption while getting students");
            return BadRequest();
        }
    }
    

}