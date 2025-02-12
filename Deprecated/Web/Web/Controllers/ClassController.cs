using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Data;
using Shared.Models;
using Web.Extensions;
using Web.Services;

namespace Web.Controllers;

[ApiController, Route("api/[controller]"), Authorize(Roles = "Teacher,Admin")]
public class ClassController : Controller
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<ClassController> _logger;
    private readonly UserManager<ApplicationUser> _userManager;

    public ClassController(AppDbContext dbContext, ILogger<ClassController> logger,
        UserManager<ApplicationUser> userManager)
    {
        _dbContext = dbContext;
        _logger = logger;
        _userManager = userManager;
    }

    [HttpGet("get_classes")]
    public async Task<IEnumerable<UserGroup>> GetClasses()
    {
        var ownerId = this.GetUserId().ToString();
        var groups = _dbContext.Groups.Where(x => x.GroupCreatorId == ownerId);
        return groups;
    }

    [HttpPost("add_student")]
    public async Task<ActionResult> AddUserToGroup(UserGroupDTO data)
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

            var student = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId.ToString());
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
    public async Task<ActionResult> AddGroup(UserGroup? group)
    {
        try
        {
            if (group == null)
            {
                return BadRequest("Group was null");
            }

            group.GroupCreatorId = this.GetUserId().ToString()!;
            await _dbContext.Groups.AddAsync(group);
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
    public async Task<IEnumerable<SimpleUser>> GetStudents([FromQuery] Guid? Id)
    {
        try
        {
            if (Id == null)
            {
                var students = (await _userManager.GetUsersInRoleAsync("Student"))
                    .Select(x => new SimpleUser(Guid.Parse(x.Id), x.UserName))
                    .Take(20);
                return students;
            }

            var group = await _dbContext.Groups.Include(x => x.Students).FirstOrDefaultAsync(x => x.Id == Id);
            return group?.Students?.Select(x => new SimpleUser(Guid.Parse(x.Id), x.UserName)) ?? [];
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Exсeption while getting students");
            return [];
        }
    }
}