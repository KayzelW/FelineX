using APIServer.Controllers;
using APIServer.Services;
using Microsoft.AspNetCore.Authentication;

namespace APIServer.Middlewares;

public class TokenCheckingMiddleware(RequestDelegate next, ILogger<TokenCheckingMiddleware> logger)
{
    public async Task Invoke(HttpContext context, TokenService tokenService)
    {
        var token = context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();

        logger.LogInformation($"{context.Connection.RemoteIpAddress} => {context.Connection.LocalIpAddress}:\n {context.Request}");
        logger.LogInformation($"User with token({token}) are checking:{context.Request.Headers.Authorization.ToString()}");

        if (!tokenService.TryGetUserId(token, out var userId))
        {
            logger.LogInformation($"Request aborted: access denied for {userId}:{token}");

            // context.GetRouteData().GetType().
            var str = context.GetEndpoint()?.Metadata.GetRequiredMetadata<MyAttribute>();
            context.Abort();
            return; //Is it working?
        }

        context.Items["User"] = userId;
        logger.LogDebug($"HttpContext modified with {userId}");

        await next(context);
    }
}