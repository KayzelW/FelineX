using Microsoft.Extensions.Logging.Abstractions;

namespace APIServer.Middlewares;

public class ListenerMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var remoteIp = context.Connection.RemoteIpAddress?.ToString();
        var remotePort = context.Connection.RemotePort.ToString();

        var logger = context.RequestServices.GetRequiredService<ILogger<ListenerMiddleware>>();
        logger?.LogInformation($"Remote Ip: {remoteIp}, Remote Port: {remotePort}");

        await next(context);
    }
}