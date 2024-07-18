using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Shared.DB.Classes.User;
using Shared.Extensions;
using WebAssembly.Services;

namespace WebAssembly.Components;

public class CookieCheck : ComponentBase
{
    [Inject] protected NavigationManager navigationManager { get; set; }
    [Inject] protected AuthService authService { get; set; }
    [Parameter] public AccessLevel? RequiredAccessLevel { get; set; }
    [Inject] protected IJSRuntime _jsRuntime { get; set; }
    [Inject] protected CookieService _cookieService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        var token = await _cookieService.GetCookieAsync(JWTExtensions.JwtCookieName);
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