using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Shared.Extensions;

public static class JwtExtensions
{
    public static string JwtCookieName { get; } = "jwtToken";
    private static readonly JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

    public static Guid? GetUserIdFromToken(this JwtSecurityToken? token)
    {
        // return token?.ValidTo <= DateTime.Now.AddMinutes(3)
        //     ? token.GetValueFromToken(JwtCookieName)!.ToGuid()
        //     : null;
        return token != null 
            ? token.GetValueFromToken(JwtCookieName)!.ToGuid()
            : null;
    }

    /// <param name="token">JwtToken</param>
    /// <param name="type">Choice from types in ClaimTypes</param>
    /// <returns>Guid</returns>
    public static string? GetValueFromToken(this JwtSecurityToken token, string type) =>
        token.Claims.FirstOrDefault(x => x.Type == type)?.Value;

    public static JwtSecurityToken? TokenFromString(string? strToken) 
        => strToken != null 
            ? handler.ReadJwtToken(strToken) 
            : null;

    public static Guid ToGuid(this string str) => Guid.Parse(str);

    public static bool TryGuid(this string str, out Guid guid) => Guid.TryParse(str, out guid);
}