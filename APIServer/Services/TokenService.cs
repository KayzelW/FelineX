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
    private JwtSecurityTokenHandler _tokenHandler { get; } = new();
    //TODO: replace with MemoryCache TTL: https://stackoverflow.com/questions/7435832/c-sharp-list-where-items-have-a-ttl
    private ConcurrentDictionary<string, UserDto?> _activeTokens { get; } = [];
    private byte[] JwtKey { get; } = Encoding.UTF8.GetBytes(configuration["Jwt:JwtKey"]!);
    private double JwtExpiresInMinutes { get; } = Convert.ToDouble(configuration["Jwt:JwtExpiresInMinutes"]!);

    public bool TryGetUserId(string? token, out UserDto userData)
    {
        userData = default!;

        logger.LogInformation($"Try get token({token})");
        if (!ValidateToken(token))
        {
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

        try
        {
            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(JwtKey),
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
            logger.LogInformation(ex, $"Token({token}) not passed check");
            return false;
        }
    }

    private string GenerateJwtToken(Guid userId)
    {
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new Claim(JwtExtensions.JwtCookieName, userId.ToString())
            ]),
            Expires = DateTime.Now.AddMinutes(JwtExpiresInMinutes),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(JwtKey), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = _tokenHandler.CreateToken(tokenDescriptor);
        logger.LogInformation($"Generated token for {userId}:{token}");
        return _tokenHandler.WriteToken(token);
    }
}