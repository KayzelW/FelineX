﻿using APIServer.Database;
using Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.DB.Classes;
using Shared.Extensions;
using Swashbuckle.AspNetCore.Annotations;

namespace APIServer.Controllers;

[ApiController, Route("[controller]")]
public class UserController : Controller
{
    private readonly AppDbContext _dbContext;

    public UserController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    
    [HttpPost(Name = "PostUser")]
    [SwaggerOperation("Post a new user from UserDTO")]
    [SwaggerResponse(409, "Already Exists")]
    [SwaggerResponse(201, "Success")]
    public async Task<ActionResult> PostUser(UserDTO user)
    {
        var _user = new User()
        {
            Id = user.Id,
            UserName = user.UserName,
            Access = new() { AccessLevel.Student },
        };
        if (_user.Id != null)
        {
            if (await _dbContext.Users.FindAsync(_user.Id) != null)
            {
                return Conflict(_user.ToDTO());
            }
        }
        else
        {
            _user.Id = Guid.NewGuid().ToString();
        }

        await _dbContext.Users.AddAsync(_user);
        await _dbContext.SaveChangesAsync();

        return Ok(_user.Id);
    }
}