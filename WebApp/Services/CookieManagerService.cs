namespace WebApp.Services;

public class CookieManagerService //TODO know how this shit works
{
    public void SetUserIdCookie(HttpContext httpContext ,string userId)
    {
        httpContext.Response.Cookies.Append("UserId", userId, new CookieOptions
        {
            Expires = DateTimeOffset.Now.AddDays(1)
        });
    }


    public string GetUserIdFromCookie(HttpContext httpContext)
    {
        return httpContext.Request.Cookies.TryGetValue("UserId", out string userId) ? userId : null;
    }
}