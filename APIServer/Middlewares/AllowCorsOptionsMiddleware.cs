using APIServer.Extensions;

namespace APIServer.Middlewares;

public class AllowCorsOptionsMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, ILogger<AllowCorsOptionsMiddleware> logger)
    {
        if (context.Request.Method == "OPTIONS")
        {
            logger.LogWarning(
                $"CORS failed(VPN?) {context.Request.Path}:{context.Connection.RemoteIpAddress}:{AuthExtensions.GetConnectionLog(context)}");
            context.Response.StatusCode = 204;
            await context.Response.CompleteAsync();
            return;
        }

        await next(context);
    }
}