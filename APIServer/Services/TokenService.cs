using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Shared.Extensions;

namespace APIServer.Services;

public class TokenService(ILogger<TokenService> logger, IConfiguration configuration)
{
    private ConcurrentDictionary<string, Guid> _activeTokens = [];

    private JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

    public bool TryGetUserId(string? token, out Guid userId)
    {
        logger.LogInformation($"Try get token({token})");
        if (!ValidateToken(token))
        {
            userId = Guid.Empty;
            return false;
        }
        
        return _activeTokens.TryGetValue(token!, out userId);
    }
    
    public string RegisterSession(Guid userId)
    {
        var token = GenerateJwtToken(userId);
        if (!_activeTokens.TryAdd(token, userId))
            return token;

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
                // ClockSkew = TimeSpan.Zero,  // допустимая погрешность в несколько минут
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
            Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(configuration["Jwt:ExpireMinutes"])),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        logger.LogInformation($"Generated token for {userId}:{token}");
        return tokenHandler.WriteToken(token);
    }
}