using System.IdentityModel.Tokens.Jwt;
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
    ApiService apiService,
    IConfiguration configuration)
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
            var userId = await AuthorizeAsync(login, password);
            if (userId == null || userId == Guid.Empty)
            {
                return null;
            }

            var token = _jwtTokenHandler.ReadJwtToken(
                GenerateJwtToken(userId.Value)
            );
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
        await SecureStorage.SetAsync(JWTExtensions.JwtCookieName, _jwtTokenHandler.WriteToken(token));
    }

    private async Task<Guid?> AuthorizeAsync(string username, string password)
    {
        var userId = await apiService.AuthUser(username, password);
        logger.LogInformation($"Get {userId} from {nameof(apiService.AuthUser)}");
        return userId;
    }

    private string GenerateJwtToken(Guid userId)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(JWTClaimNames.UserId, userId.ToString())
            }),
            Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(configuration["Jwt:ExpireMinutes"])),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}