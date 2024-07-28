using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Shared.Extensions;

namespace WebAssembly.Services;

public class CookieService
{
    private readonly IJSRuntime _jsRuntime;

    public CookieService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task SetCookieAsync(string name, string value, int days = 7)
    {
        var expires = new DateTimeOffset(DateTime.UtcNow.AddDays(days)).ToString("R");
        await _jsRuntime.InvokeVoidAsync("cookieHelper.setCookie", name, value, days);
    }

    public async Task<string?> GetCookieAsync(string name)
    {
        return await _jsRuntime.InvokeAsync<string>("cookieHelper.getCookie", name);
    }

    public async Task SetJwtTokenAsync(string value)
    {
        await SetCookieAsync(JWTExtensions.JwtCookieName, value, 7);
    }

    public async Task<Guid?> GetUserIdAsync()
    {
        var strToken = await GetCookieAsync(JWTExtensions.JwtCookieName);
        if (strToken == null) return null;
        return JWTExtensions.TokenFromString(strToken).GetGuidFromToken();
    }

    public async void RemoveJwtToken()
    {
        await _jsRuntime.InvokeVoidAsync("cookieHelper.deleteCookie", JWTExtensions.JwtCookieName);
    }
}