using APIServer.Services;
using Microsoft.AspNetCore.Authentication;

namespace APIServer.Middlewares;

public class TokenCheckingMiddleware(RequestDelegate next, ILogger<TokenCheckingMiddleware> logger)
{
    public async Task Invoke(HttpContext context, TokenService tokenService)
    {
        var token = context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();

        logger.LogInformation($"User with token({token}) are checking");
        
        if (tokenService.TryGetUserId(token, out var userId))
        {
            context.Items["User"] = userId;
            logger.LogCritical($"HttpContext modified with {userId}");
        }
        else
        {
            logger.LogInformation($"access denied for token({token})");
            if (Environment.GetEnvironmentVariables()["ASPNETCORE_ENVIRONMENT"]?.ToString() != "Development")
                await context.ForbidAsync();
        }

        await next(context);
    }
}