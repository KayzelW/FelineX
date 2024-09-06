using BlazorServer.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Shared.Interfaces;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddSingleton(x =>
{
    return new HttpClient()
    {
        BaseAddress = new Uri(x.GetRequiredService<NavigationManager>().BaseUri)
    };
});
builder.Services.AddSingleton<ApiService>();
builder.Services.AddSingleton<IUserContextService, UserContextService>();
builder.Services.AddScoped<ILocalStorageService, LocalStorageService>();

await builder.Build().RunAsync();