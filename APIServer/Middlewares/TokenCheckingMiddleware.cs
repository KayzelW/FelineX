using APIServer.Services;
using Microsoft.AspNetCore.Authentication;

namespace APIServer.Middlewares;

public class TokenCheckingMiddleware(RequestDelegate next, ILogger<TokenCheckingMiddleware> logger)
{
    public async Task Invoke(HttpContext context, TokenService tokenService)
    {
        var token = context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();

        logger.LogInformation(
            $"{context.Connection.RemoteIpAddress} => {context.Connection.LocalIpAddress}:\n {context.Request}");


        logger.LogInformation($"User with token({token}) are checking");
        logger.LogWarning(context.Request.Headers.Authorization.ToString());

        if (tokenService.TryGetUserId(token, out var userId))
        {
            context.Items["User"] = userId;
            logger.LogCritical($"HttpContext modified with {userId}");
        }
        else
        {
            logger.LogInformation($"access denied for token({token})");
#if !DEBUG
            await context.ForbidAsync();
#endif
        }

        await next(context);
    }
}