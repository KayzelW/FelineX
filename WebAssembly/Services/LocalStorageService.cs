using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Shared.Extensions;

namespace WebAssembly.Services;

public partial class LocalStorageService(IJSRuntime jsRuntime)
{
    public async Task<Guid?> GetUserIdAsync()
    {
        var strToken = await GetItemAsync(JwtExtensions.JwtCookieName);
        return JwtExtensions.TokenFromString(strToken).GetUserIdFromToken();
    }

    public async void RemoveJwtToken()
    {
        await jsRuntime.InvokeVoidAsync("cookieHelper.deleteCookie", JwtExtensions.JwtCookieName);
    }

    public async Task SetJwtTokenAsync(string value)
    {
        await SetItemAsync(JwtExtensions.JwtCookieName, value, 7);
    }
}