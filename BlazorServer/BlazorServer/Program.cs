using BlazorServer.Client;
using BlazorServer.Client.Pages;
using BlazorServer.Components;
using Microsoft.AspNetCore.Components;


namespace BlazorServer;

public sealed class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        ConfigureServices(builder);

        var app = builder.Build();

        ConfigureApplication(app);

        app.Run();
    }


    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveWebAssemblyComponents();

        builder.Services.AddControllers();
        builder.Services.AddScoped(x =>
        {
            return new HttpClient()
            {
                BaseAddress = new Uri(x.GetRequiredService<NavigationManager>().BaseUri)
            };
        });
        builder.Services.AddScoped<ApiService>();
    }

    private static void ConfigureApplication(WebApplication app)
    {
        app.MapControllers();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseWebAssemblyDebugging();
        }
        else
        {
            app.UseExceptionHandler("/Error", createScopeForErrors: true);
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.UseAntiforgery();

        app.MapRazorComponents<App>()
            .AddInteractiveWebAssemblyRenderMode()
            .AddAdditionalAssemblies(typeof(BlazorServer.Client._Imports).Assembly);
    }
}