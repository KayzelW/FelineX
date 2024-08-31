using UAParser;

namespace APIServer.Extensions;

public static class AuthExtensions
{
    public static string GetConnectionLog(HttpContext httpContext)
    {
        var userAgentString = httpContext.Request.Headers["User-Agent"].ToString();
        var userAgentParser = Parser.GetDefault();
        
        var clientInfo = userAgentParser.Parse(userAgentString);
        var ipAddress = httpContext.Connection.RemoteIpAddress?.ToString();
        var operatingSystem = clientInfo.OS.Family;
        var device = clientInfo.Device.Family;
        
        var logString = $"Браузер: {clientInfo.UA}, IP: {ipAddress}, ОС: {operatingSystem}, Устройство: {device}";
        return logString;
    }
}