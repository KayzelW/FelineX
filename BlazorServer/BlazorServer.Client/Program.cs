using BlazorServer.Client;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddSingleton(x =>
{
    return new HttpClient()
    {
        BaseAddress = new Uri(x.GetRequiredService<NavigationManager>().BaseUri)
    };
});
builder.Services.AddSingleton<ApiService>();

await builder.Build().RunAsync();