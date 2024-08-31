using APIServer.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Attributes;
using Shared.DB.User;

namespace APIServer.Controllers;

[ApiController, Route("[controller]"), AuthorizeLevel(AccessLevel.Teacher)]
public class SearchController : Controller
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<SearchController> _logger;
    
    public SearchController(AppDbContext dbContext, ILogger<SearchController> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    [HttpGet("search_users")]
    public async Task<IActionResult> SearchUsers(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return BadRequest("Query cannot be empty.");
        }
        var users = await _dbContext.Users
            .Where(u => u.UserName.Contains(query))
            .OrderBy(u => u.UserName)
            .Select(x => new { x.Id, x.UserName, x.NormalizedUserName })
            .ToListAsync();
        return Ok(users.Take(10)); 
    }
    
    [HttpGet("search_groups")]
    public async Task<IActionResult> SearchGroups(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return BadRequest("Query cannot be empty.");
        }
        var users = await _dbContext.Groups
            .Where(u => u.GroupName.Contains(query))
            .OrderBy(u => u.GroupName)
            .Select(x => new { x.Id, x.GroupName})
            .ToListAsync();
        return Ok(users.Take(10)); 
    }
    
}