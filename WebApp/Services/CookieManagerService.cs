namespace WebApp.Services;

public class CookieManagerService
{
    public void SetUserIdCookie(HttpContext httpContext, string userId)
    {
        httpContext.Response.Cookies.Append("UserId", userId, new CookieOptions
        {
            Expires = DateTimeOffset.Now.AddDays(1),
            HttpOnly = true
        });
    }


    public static string GetUserIdFromCookie(HttpContext httpContext)
    {
        return httpContext.Request.Cookies.TryGetValue("UserId", out string userId) ? userId : null;
    }
}