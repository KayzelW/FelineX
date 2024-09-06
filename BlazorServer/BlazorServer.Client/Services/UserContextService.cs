using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using BlazorServer.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Shared.DB.User;
using Shared.Extensions;
using Shared.Interfaces;
using Shared.Models;

namespace BlazorServer.Client.Services;

public sealed class UserContextService
    : IUserContextService
{
    private ILocalStorageService localStorageService { get; }
    private ILogger<UserContextService> logger { get; }
    private NavigationManager navigationManager { get; }
    private ApiService apiService { get; }
    private HttpClient httpClient { get; }

    // private readonly JwtSecurityTokenHandler _jwtTokenHandler = new();

    public string? UserToken { get; private set; }
    public string UserName { get; private set; } = "";
    public uint? Access { get; private set; }

    public bool isLoading { get; private set; } = true;

    public bool IsAuthorized
    {
        get => Access != null && !string.IsNullOrEmpty(UserToken);
        private set
        {
            if (value == false)
            {
                logger.LogInformation("IsAuthorized was set in false");
                navigationManager.NavigateTo("/auth");
            }
        }
    }

    public UserContextService(ILocalStorageService localStorageService, ILogger<UserContextService> logger, NavigationManager navigationManager, ApiService apiService, HttpClient httpClient)
    {
        this.localStorageService = localStorageService;
        this.logger = logger;
        this.navigationManager = navigationManager;
        this.apiService = apiService;
        this.httpClient = httpClient;
        
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
        navigationManager.NavigateTo("/auth");
        // IsAuthorized = false;
    }

    private async void FillUserContextStartUp()
    {
        var cookieToken = await localStorageService.GetItemAsync(JwtExtensions.JwtCookieName);
        if (cookieToken == null)
        {
            RemoveTokenAsync();
            isLoading = false;
            // IsAuthorized = false;
            return;
        }

        
        var data = await apiService.AuthUserByToken(cookieToken);
        FillUserContext(data);
        isLoading = false;
    }

    private void FillUserContext(AuthAnswer? data)
    {
        
        if (data?.UserToken == null || data?.Access == null)
        {
            RemoveTokenAsync();
            // IsAuthorized = false;
            return;
        }

        UserToken = data.UserToken;
        UserName = data.UserName ?? "";
        Access = data.Access;
        // IsAuthorized = true; not used
        SetTokenAsync(UserToken);
        
        logger.LogInformation($"CookieToken is {UserToken}");
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", UserToken);
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