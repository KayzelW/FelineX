﻿using Microsoft.JSInterop;
using Shared.Extensions;

namespace WebApp;

public static class CookieInterop
{
    public static ValueTask SetCookie(IJSRuntime jsRuntime, string name, string value, int days = 7)
        => jsRuntime.InvokeVoidAsync("setCookie", name, value, days);

    public static async Task<string> getCookie(IJSRuntime jsRuntime, string name) =>
        await jsRuntime.InvokeAsync<string>("getCookie", name);

    public static ValueTask SetJwtToken(IJSRuntime jsRuntime, string value)
        => SetCookie(jsRuntime, JWTExtensions.JwtCookieName, value, 7);
}