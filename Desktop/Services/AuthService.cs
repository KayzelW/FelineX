﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;
using Shared.DB.Classes.User;
using Shared.Extensions;

namespace Desktop.Services;

public class AuthService(
    ILogger<AuthService> logger,
    ApiService apiService)
{
    private readonly JwtSecurityTokenHandler _jwtTokenHandler = new();

    public async Task<bool> HasAccess(Guid userId, AccessLevel accessLevel)
    {
        var access = await apiService.GetUserAccessById(userId);
        if (access is null)
        {
            return false;
        }

        return ((AccessLevel)access).HasFlag(accessLevel);
    }

    public async Task<bool> HasUser(Guid userId)
    {
        var access = await apiService.GetUserAccessById(userId);
        return access != null;
    }


    /// <summary>
    /// This func will authorize user and return the JwtToken
    /// </summary>
    /// <param name="login"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public async Task<JwtSecurityToken?> GetJwtToken(string login, string password)
    {
        try
        {
            var token = _jwtTokenHandler.ReadJwtToken(await AuthorizeAsync(login, password));
            // if (userId == null || userId == Guid.Empty)
            // {
            //     return null;
            // }

            return token;
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Error while get token in {this.GetJwtToken}");
            return null;
        }
    }

    public async Task RegisterJwtToken(IJSRuntime _jsRuntime, JwtSecurityToken token)
    {
        await SetJwtToken(_jsRuntime, token);
    }

    private async Task SetJwtToken(IJSRuntime _jsRuntime, JwtSecurityToken token)
    {
        await CookieInterop.SetJwtToken(_jsRuntime, _jwtTokenHandler.WriteToken(token));
        await Task.Delay(1500);
    }

    private async Task<string?> AuthorizeAsync(string username, string password)
    {
        var token = await apiService.AuthUser(username, password);
        logger.LogInformation($"Get {token} from {nameof(apiService.AuthUser)}");
        return token;
    }
}