using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Shared.DB.Classes.User;
using Shared.Extensions;
using Shared.Interfaces;
using Shared.Models;

namespace WebAssembly.Services;

public sealed class UserContextService
    : IUserContextService
{
    private ILocalStorageService localStorageService { get; }
    private ILogger<UserContextService> logger { get; }
    private NavigationManager navigationManager { get; }
    private ApiService apiService { get; }

    // private readonly JwtSecurityTokenHandler _jwtTokenHandler = new();

    public string? UserToken { get; private set; }
    public string UserName { get; private set; } = "";
    public uint? Access { get; private set; }

    public bool IsAuthorized
    {
        get => Access != null && !string.IsNullOrEmpty(UserToken);
        private set
        {
            if (value == false)
            {
                navigationManager.NavigateTo("/auth");
            }
        }
    }

    public UserContextService(ILocalStorageService localStorageService, ILogger<UserContextService> logger, NavigationManager navigationManager, ApiService apiService)
    {
        this.localStorageService = localStorageService;
        this.logger = logger;
        this.navigationManager = navigationManager;
        this.apiService = apiService;
        
        FillUserContextStartUp();
    }

    /// <summary>
    /// This func will authorize user and return the JwtToken as string
    /// </summary>
    /// <param name="login"></param>
    /// <param name="password"></param>
    public async Task AuthorizeAsync(string login, string password)
    {
        var data = await apiService.AuthUser(login, password);
        logger.LogInformation($"Get {data?.UserToken} - {data?.Access} from {apiService.AuthUser}");
        FillUserContext(data);
    }

    public ClaimsPrincipal ClaimsPrincipal =>
        IsAuthorized
            ? new ClaimsPrincipal(new ClaimsIdentity(authenticationType: "UserToken"))
            : new ClaimsPrincipal(new ClaimsIdentity());

    public bool HasAccess(AccessLevel required)
    {
        if (Access == null)
            return false;

        return required == (uint)AccessLevel.Exists
               || ((AccessLevel)Access & required) == required;
    }

    public bool HasAccess(uint required)
    {
        if (Access == null)
            return false;

        return required == (uint)AccessLevel.Exists
               || (Access & required) == required;
    }

    public async void Logout()
    {
        RemoveTokenAsync();
        UserToken = null;
        UserName = string.Empty;
        Access = null;
        IsAuthorized = false;
    }

    private async void FillUserContextStartUp()
    {
        var cookieToken = await localStorageService.GetItemAsync(JwtExtensions.JwtCookieName);
        if (cookieToken == null)
        {
            RemoveTokenAsync();
            IsAuthorized = false;
            return;
        }

        var data = await apiService.AuthUserByToken(cookieToken);
        FillUserContext(data);
    }

    private async void FillUserContext(AuthAnswer? data)
    {
        if (data?.UserToken == null || data?.Access == null)
        {
            RemoveTokenAsync();
            IsAuthorized = false;
            return;
        }

        UserToken = data.UserToken;
        UserName = data.UserName ?? "";
        Access = data.Access;
        // IsAuthorized = true; not used
        SetTokenAsync(UserToken);
    }

    private async void RemoveTokenAsync()
    {
        await localStorageService.RemoveItemAsync(JwtExtensions.JwtCookieName);
    }

    private async void SetTokenAsync(string value)
    {
        await localStorageService.SetItemAsync(JwtExtensions.JwtCookieName, value, 7);
    }
}