using APIServer.Controllers;
using APIServer.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Shared.Attributes;
using Shared.DB.User;
using Shared.Extensions;

namespace APIServer.Middlewares;

public class TokenCheckingMiddleware(RequestDelegate next, ILogger<TokenCheckingMiddleware> logger)
{
    public async Task Invoke(HttpContext context, TokenService tokenService)
    {
        
        var token = context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();

        logger.LogInformation($"{context.Connection.RemoteIpAddress} => {context.Connection.LocalIpAddress}:\n {context.Request}");
        logger.LogInformation($"User with token({token}) are checking:{context.Request.Headers.Authorization.ToString()}");
        
        var access = context.GetEndpoint()?.Metadata.GetMetadata<AuthorizeLevelAttribute>();
        
        if (!tokenService.TryGetUserId(token, out var userData))
        {
            if (context.GetEndpoint()?.Metadata.GetMetadata<AllowAnonymousAttribute>() != null)
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