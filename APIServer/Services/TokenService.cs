using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Shared.DB.User;
using Shared.Extensions;
using Shared.Models;

namespace APIServer.Services;

public class TokenService(ILogger<TokenService> logger, IConfiguration configuration)
{
    //TODO: replace with MemoryCache TTL: https://stackoverflow.com/questions/7435832/c-sharp-list-where-items-have-a-ttl
    private readonly ConcurrentDictionary<string, UserDto?> _activeTokens = []; 
    private readonly JwtSecurityTokenHandler _tokenHandler = new JwtSecurityTokenHandler();

    public bool TryGetUserId(string? token, out UserDto? userData)
    {
        logger.LogInformation($"Try get token({token})");
        if (!ValidateToken(token))
        {
            userData = null;
            return false;
        }

        return _activeTokens.TryGetValue(token!, out userData);
    }

    public string RegisterSession(UserDto userData)
    {
        var token = GenerateJwtToken(userData.Id);
        if (!_activeTokens.TryAdd(token, userData))
        {
            //TODO:continue token lifetime logic
            return token;
        }

        return token;
    }

    public bool RemoveToken(string token)
    {
        return _activeTokens.TryRemove(token, out _);
    }

    private bool ValidateToken(string? token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!);

        try
        {
            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true, // необходимость проверки срока действия токена
                ClockSkew = TimeSpan.FromMinutes(30), // допустимая погрешность в несколько минут
            }, out var validatedToken);

            logger.LogInformation($"Token({token}) passed the check");

            return true;
        }
        catch (Exception ex)
        {
            logger.LogInformation($"Token({token}) not passed check", ex);
            return false;
        }
    }

    private string GenerateJwtToken(Guid userId)
    {
        logger.LogInformation($"Generating token for {userId}");
        var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(JwtExtensions.JwtCookieName, userId.ToString())
            }),
            Expires = DateTime.Now.AddMinutes(Convert.ToDouble(configuration["Jwt:ExpireMinutes"])),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = _tokenHandler.CreateToken(tokenDescriptor);
        logger.LogInformation($"Generated token for {userId}:{token}");
        return _tokenHandler.WriteToken(token);
    }
}