﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Shared.Extensions;

public static class JWTExtensions
{
    public static string JwtCookieName { get; } = "jwtToken";
    private static readonly JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

    public static Guid GetGuidFromToken(this JwtSecurityToken token)
    {
        return token.ValidTo <= DateTime.UtcNow
            ? token.GetValueFromToken(JWTClaimNames.UserId)!.ToGuid()
            : Guid.Empty;
    }

    /// <param name="token">JwtToken</param>
    /// <param name="type">Choice from types in ClaimTypes</param>
    /// <returns>Guid</returns>
    public static string? GetValueFromToken(this JwtSecurityToken token, string type) =>
        token.Claims.FirstOrDefault(x => x.Type == type)?.Value;

    public static JwtSecurityToken TokenFromString(string strToken) => handler.ReadJwtToken(strToken);

    public static Guid ToGuid(this string str) => Guid.Parse(str);

    public static bool TryGuid(this string str, out Guid guid) => Guid.TryParse(str, out guid);
}