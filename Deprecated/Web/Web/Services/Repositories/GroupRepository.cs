using Microsoft.AspNetCore.Components.Authorization;
using Shared.Data;

namespace Web.Services.Repositories;

public class GroupRepository
{
    private AppDbContext _dbContext;
    private AuthenticationStateProvider _authenticationStateProvider;

    public GroupRepository(AppDbContext dbContext, AuthenticationStateProvider authenticationState)
    {
        _dbContext = dbContext;

        _authenticationStateProvider = authenticationState;
    }

    public async Task<List<UserGroup>> OnTrySearch(string? name)
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

        var groups = _dbContext.Groups.Where(x => x.GroupName.StartsWith(name)).Take(10).ToList();

        return groups;
    }
}