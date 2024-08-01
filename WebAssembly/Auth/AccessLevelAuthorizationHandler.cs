using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Shared.DB.Classes.User;
using Shared.Extensions;
using WebAssembly.Services;

namespace WebAssembly.Auth;

public class AccessLevelAuthorizationHandler(ApiService apiService) : AuthorizationHandler<AccessLevelRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AccessLevelRequirement requirement)
    {
        if (context.User.Identity is ClaimsIdentity identity)
        {
            var userIdClaim = identity.FindFirst(JwtExtensions.JwtCookieName);
            if (userIdClaim != null)
            {
                var userId = userIdClaim.Value.ToGuid();
                var accessLevel = await apiService.GetUserAccessById(userId);

                if (accessLevel == null)
                {
                    context.Fail();
                    return;
                }

                if (requirement.RequiredLevel == (uint)AccessLevel.Exists
                    || (accessLevel & requirement.RequiredLevel) == requirement.RequiredLevel)
                {
                    context.Succeed(requirement);
                }
            }
        }
    }
}

