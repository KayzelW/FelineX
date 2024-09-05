using System.Net.Http.Headers;
using System.Net.Mime;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using Blazored.Toast;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
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

        // var adress = builder.Configuration.GetConnectionString("ApiUrl");
        // var baseAdress = adress == null ? null : new Uri(adress, UriKind.RelativeOrAbsolute);

        builder.Services.AddSingleton(x =>
        {
            return new HttpClient()
            {
                BaseAddress = new Uri(builder.Configuration.GetConnectionString("ApiUrl")!)
            };
        });
            

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