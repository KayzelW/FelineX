using System.Net.Http.Headers;
using System.Net.Mime;
using Blazored.Toast;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shared.DB.User;
using Shared.Extensions;
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

        builder.Services.AddSingleton(sp => new HttpClient
        {
            BaseAddress = new Uri(builder.Configuration.GetConnectionString("ApiUrl")
                                  ?? throw new ApplicationException("ApiUrl are not existing")),
            
        });
        
        
        // if (builder.HostEnvironment.BaseAddress.StartsWith("https"))
        // {
        //     builder.Services.AddSingleton(sp => new HttpClient
        //     {
        //         BaseAddress = new Uri(builder.Configuration.GetConnectionString("ApiUrlHttp")
        //                               ?? throw new ApplicationException("ApiUrl are not existing")),
        //     });
        //
        // }
        // else
        // {
        //     builder.Services.AddSingleton(sp => new HttpClient
        //     {
        //         BaseAddress = new Uri(builder.Configuration.GetConnectionString("ApiUrl")
        //                               ?? throw new ApplicationException("ApiUrl are not existing")),
        //     });
        //     
        // }
        
        builder.Services.AddLogging();
        builder.Services.AddSingleton<ILocalStorageService, LocalStorageService>();
        builder.Services.AddSingleton<ApiService>();
        builder.Services.AddSingleton<IUserContextService, UserContextService>();
        builder.Services.AddScoped<SearchService<User>>();
        builder.Services.AddScoped<SearchService<UserGroup>>();
        
        builder.Services.AddBlazoredToast();
            

        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");


        _tasks.Add(builder.Build().RunAsync());

        await Task.WhenAll(_tasks);
    }
}