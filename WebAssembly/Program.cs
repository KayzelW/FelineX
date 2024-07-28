using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WebAssembly.Services;

namespace WebAssembly;

public sealed class Program
{
    public static async Task Main(string[] args)
    {
        var tasks = new List<Task>();

        var builder = WebAssemblyHostBuilder.CreateDefault(args);

        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        builder.Services.AddScoped(sp =>
            new HttpClient
            {
                BaseAddress = new Uri(builder.Configuration.GetConnectionString("ApiUrl")
                                      ?? throw new ApplicationException("ApiUrl are not existing")),
            }
        );

        try
        {
            builder.Services.AddMemoryCache();
            builder.Services.TryAddTransient<CookieService>();
            builder.Services.AddScoped<ApiService>();
            builder.Services.AddTransient<AuthService>();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        tasks.Add(builder.Build().RunAsync());

        await Task.WhenAll(tasks);
    }
}