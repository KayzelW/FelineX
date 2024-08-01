using System.Reflection;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Rendering;
using Shared.DB.Classes.User;
using Shared.Extensions;
using WebAssembly.Services;

namespace WebAssembly.Auth;

public class AccessLevelRouteView : RouteView
{
    [Inject] private ApiService _ApiService { get; set; }
    [Inject] private NavigationManager Navigation { get; set; }
    [Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; set; }
    [Inject] private ILogger<AccessLevelRouteView> _logger { get; set; }

    protected override async void Render(RenderTreeBuilder builder)
    {
        var pageType = RouteData.PageType;
        _logger.LogInformation($"Start routing {pageType}");
        var authorizeLevelAttribute = pageType.GetCustomAttribute<AuthorizeLevelAttribute>();

        if (authorizeLevelAttribute != null)
        {
            var requiredLevel = authorizeLevelAttribute.RequiredLevel;

            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity == null)
            {
                _logger.LogInformation($"user.Identity is null | {user.Identity}");
                Navigation.NavigateTo("/auth");
                return;
            }

            if (user.Identity.IsAuthenticated)
            {
                var userId = user.Claims.FirstOrDefault(x => x.Type == JwtExtensions.JwtCookieName)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogInformation($"userId is NullOrEmpty | {userId}");
                    Navigation.NavigateTo("/auth");
                    return;
                }

                var accessLevel = await _ApiService.GetUserAccessById(userId.ToGuid());

                if (accessLevel == null)
                {
                    _logger.LogInformation($"accessLevel == null | {accessLevel}");
                    Navigation.NavigateTo("/auth");
                    return;
                }

                _logger.LogInformation($"Check {userId} for {requiredLevel}");
                if (requiredLevel == (uint)AccessLevel.Exists
                    || (accessLevel & requiredLevel) == requiredLevel)
                {
                    base.Render(builder);
                }
            }
            else
            {
                _logger.LogInformation($"user is not authenticated | {user.Identity.IsAuthenticated} => {user.Identity.AuthenticationType}");
                Navigation.NavigateTo("/auth");
            }
        }
        else
        {
            _logger.LogInformation($"There is no attribute on this page | {pageType}\n {authorizeLevelAttribute}");
            base.Render(builder);
        }
        _logger.LogInformation($"Routed to {pageType}");
    }
}