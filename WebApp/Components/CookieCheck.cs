using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Components;
using Shared.DB.Classes.User;
using Shared.Extensions;
using WebApp.Services;


namespace WebApp.Components;

public class CookieCheck : ComponentBase
{
    [Inject] protected NavigationManager navigationManager { get; set; }
    [Inject] protected IHttpContextAccessor HttpContextAccessor { get; set; }
    [Inject] protected AuthService authService { get; set; }
    [Parameter] public AccessLevel RequiredAccessLevel { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        var token = HttpContextAccessor.HttpContext!.Request.Cookies[JWTExtensions.JwtCookieName];
        var userId = new JwtSecurityToken(token).GetGuidFromToken();
        
        if (userId == Guid.Empty || !authService.HasAccess(userId, RequiredAccessLevel))
        {
            navigationManager.NavigateTo("/auth", true);
        }
    }
}