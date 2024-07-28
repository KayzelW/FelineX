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
    [Inject] public NavigationManager _navigationManager { get; set; }
    [Inject] public AuthService _authService { get; set; }
    [Inject] public CookieService _cookieService { get; set; }
    [Parameter] public AccessLevel? RequiredAccessLevel { get; set; }
    
    
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        var token = await _cookieService.GetCookieAsync(JWTExtensions.JwtCookieName);
        if (token is null)
        {
            _navigationManager.NavigateTo("/auth", true);
            return;
        }

        var userId = new JwtSecurityToken(token).GetGuidFromToken();

        if (userId == Guid.Empty || (RequiredAccessLevel == null
                ? !await _authService.HasUser(userId)
                : !await _authService.HasAccess(userId, RequiredAccessLevel.Value)))
        {
            _navigationManager.NavigateTo("/auth", true);
        }
    }
}