using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Shared.Extensions;
using Shared.Interfaces;
using WebAssembly.Services;

namespace WebAssembly.Auth;

public class AccessLevelAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly LocalStorageService _localStorageService;

    public AccessLevelAuthenticationStateProvider(LocalStorageService localStorageService)
    {
        _localStorageService = localStorageService;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await _localStorageService.GetItemAsync(JwtExtensions.JwtCookieName);
        
        if (token == null)
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        var jwtToken = JwtExtensions.TokenFromString(token)!;
        var identity = new ClaimsIdentity(jwtToken.Claims, "jwt");
        
        var user = new ClaimsPrincipal(identity);

        return new AuthenticationState(user);
    }

    public void NotifyUserAuthentication(string token)
    {
        var jwtToken = JwtExtensions.TokenFromString(token)!;
        var identity = new ClaimsIdentity(jwtToken.Claims, "jwt");

        var user = new ClaimsPrincipal(identity);
        var authState = Task.FromResult(new AuthenticationState(user));
        NotifyAuthenticationStateChanged(authState);
    }

    public void NotifyUserLogout()
    {
        var authState = Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
        NotifyAuthenticationStateChanged(authState);
    }
}