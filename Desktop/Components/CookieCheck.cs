using System.IdentityModel.Tokens.Jwt;
using Desktop.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Shared.DB.Classes.User;
using Shared.Extensions;

namespace Desktop.Components;

public class CookieCheck : ComponentBase
{
    [Inject] protected NavigationManager navigationManager { get; set; }
    [Inject] protected AuthService authService { get; set; }
    [Parameter] public AccessLevel? RequiredAccessLevel { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        var token = await SecureStorage.GetAsync(JWTExtensions.JwtCookieName);
        if (token is null)
        {
            navigationManager.NavigateTo("/auth", true);
            return;
        }

        var userId = new JwtSecurityToken(token).GetGuidFromToken();

        if (userId == Guid.Empty || (RequiredAccessLevel == null
                ? !await authService.HasUser(userId)
                : !await authService.HasAccess(userId, RequiredAccessLevel.Value)))
        {
            navigationManager.NavigateTo("/auth", true);
        }
    }
}