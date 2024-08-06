using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;
using Shared.DB.Classes.User;
using Shared.Extensions;

namespace WebAssembly.Services;

public class AuthService(
    ILogger<AuthService> _logger,
    ApiService apiService,
    LocalStorageService localStorageService)
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
        var strToken = await AuthorizeAsync(login, password);
        if (string.IsNullOrEmpty(strToken))
        {
            throw new HttpRequestException($"Token from server is {strToken}");
        }

        var token = _jwtTokenHandler.ReadJwtToken(strToken);

        return token;
    }

    public async Task SetJwtToken(JwtSecurityToken token)
    {
        await localStorageService.SetJwtTokenAsync(_jwtTokenHandler.WriteToken(token));
    }

    private async Task<string?> AuthorizeAsync(string username, string password)
    {
        var token = await apiService.AuthUser(username, password);
        _logger.LogInformation($"Get {token} from {nameof(apiService.AuthUser)}");
        return token;
    }
}