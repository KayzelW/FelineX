using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Shared.Extensions;

namespace WebApp.Interceptors;

public class JwtTokenInterceptor : DelegatingHandler
{
    private readonly ProtectedLocalStorage _protectedLocalStorage;

    public JwtTokenInterceptor(ProtectedLocalStorage protectedLocalStorage)
    {
        _protectedLocalStorage = protectedLocalStorage;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var accessToken = await GetTokenAsync();
        if (!string.IsNullOrEmpty(accessToken))
        {
            request.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
        }

        return await base.SendAsync(request, cancellationToken);
    }

    private async Task<string> GetTokenAsync()
    {
        try
        {
            var result = await _protectedLocalStorage.GetAsync<string>(JWTExtensions.JwtCookieName);
            if (result.Success)
            {
                return result.Value ?? string.Empty;
            }
        }
        catch (Exception ex)
        {
            // Handle exception or log it
            Console.WriteLine($"Error retrieving token from ProtectedLocalStorage: {ex.Message}");
        }

        return string.Empty;
    }
}