﻿using Microsoft.JSInterop;
using Shared.Extensions;
using Shared.Interfaces;

namespace WebAssembly.Services;

public partial class LocalStorageService : ILocalStorageService
{
    public async Task SetItemAsync(string name, string value, int days = 7)
    {
        var expires = new DateTimeOffset(DateTime.UtcNow.AddDays(days)).ToString("R");
        await jsRuntime.InvokeVoidAsync("cookieHelper.setCookie", name, value, days);
    }

    public async Task<string?> GetItemAsync(string name)
    {
        var cookie = await jsRuntime.InvokeAsync<string>("cookieHelper.getCookie", name);
        return string.IsNullOrEmpty(cookie) ? null : cookie;
    }
}