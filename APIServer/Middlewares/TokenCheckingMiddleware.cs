using APIServer.Controllers;
using APIServer.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Shared.Attributes;
using Shared.Extensions;

namespace APIServer.Middlewares;

public class TokenCheckingMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
#if DEBUG
        if (context.Request.Host.Host == "localhost")
        {
            await next(context);
            return;
        }
#endif

        var tokenService = context.RequestServices.GetRequiredService<TokenService>();
        var logger = context.RequestServices.GetRequiredService<ILogger<TokenCheckingMiddleware>>();

        var token = context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();

        logger.LogInformation(
            $"User with token({token}) are checking:{context.Request.Headers.Authorization.ToString()}");

        var access = context.GetEndpoint()?.Metadata.GetMetadata<AuthorizeLevelAttribute>();

        if (!tokenService.TryGetUserId(token, out var userData))
        {
            var endPoint = context.GetEndpoint();
            if (endPoint == null || endPoint.Metadata.Count == 0)
            {
                await next(context);
                return;
            }

            if (endPoint.Metadata.GetMetadata<AllowAnonymousAttribute>() != null)
            {
                await next(context);
                return;
            }

            logger.LogInformation($"Request aborted: access denied");
            context.Abort();
            return;
        }

        if (access == null)
        {
            context.Items["User"] = userData.Id;
            await next(context);
            return;
        }

        if (!UserExtensions.HasAccess(access.RequiredLevel, userData.Access))
        {
            logger.LogInformation($"Request aborted: access denied for {userData.Id}:{token}");
            context.Abort();
            return;
        }

        context.Items["User"] = userData.Id;
        logger.LogDebug($"HttpContext modified with {userData.Id}");

        await next(context);
    }
}