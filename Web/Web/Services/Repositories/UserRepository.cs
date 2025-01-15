using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Caching.Distributed;
using Shared.Data;

namespace Web.Services.Repositories;

public class UserRepository
{
    private AppDbContext _dbContext;
    private AuthenticationStateProvider _authenticationStateProvider;

    public UserRepository(AppDbContext dbContext, AuthenticationStateProvider authenticationState)
    {
        _dbContext = dbContext;
        
        _authenticationStateProvider = authenticationState;
    }
    
    public async Task<List<ApplicationUser>> OnTrySearch(string? name)
    {
        var authenticationStateAsync = await _authenticationStateProvider.GetAuthenticationStateAsync();
        if (string.IsNullOrEmpty(name))
        {
            return [];
        }

        if (!authenticationStateAsync.User.IsInRole("Teacher") || !authenticationStateAsync.User.IsInRole("Admin"))
        {
            return [];
        }

        var users = _dbContext.Users.Where(x => x.UserName.StartsWith(name)).Take(10).ToList();
        
        return users;
    }
}