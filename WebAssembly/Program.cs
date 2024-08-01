using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shared.DB.Classes.User;
using Shared.Interfaces;
using WebAssembly.Auth;
using WebAssembly.Services;

namespace WebAssembly;

public sealed class Program
{
    private static readonly List<Task> _tasks = [];
    
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        
        builder.Services.AddMemoryCache();
        builder.Services.AddSingleton<LocalStorageService>();
        builder.Services.AddSingleton<ApiService>();
        builder.Services.AddSingleton<AuthService>();
        
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        builder.Services.AddSingleton(sp =>
            new HttpClient
            {
                BaseAddress = new Uri(builder.Configuration.GetConnectionString("ApiUrl")
                                      ?? throw new ApplicationException("ApiUrl are not existing")),
            }
        );
        
        builder.Services.AddScoped<AuthenticationStateProvider, AccessLevelAuthenticationStateProvider>();

        builder.Services.AddAuthorizationCore(options =>
        {
            options.AddPolicy("Exists", policy => policy.AddRequirements(new AccessLevelRequirement(AccessLevel.Exists)));
            options.AddPolicy("Student", policy => policy.Requirements.Add(new AccessLevelRequirement(AccessLevel.Student)));
            options.AddPolicy("Teacher", policy => policy.Requirements.Add(new AccessLevelRequirement(AccessLevel.Teacher)));
            // options.AddPolicy("Level2Access", policy => policy.Requirements.Add(new AccessLevelRequirement(2))); for future
        });
        builder.Services.AddSingleton<IAuthorizationHandler, AccessLevelAuthorizationHandler>();
        
        
        _tasks.Add(builder.Build().RunAsync());

        await Task.WhenAll(_tasks);
    }
}

