using System.Diagnostics;
using System.Net;
using System.Net.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebApp.Components;
using WebApp.Interceptors;
using WebApp.Services;

namespace WebApp;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        ConfigureServices(builder.Services);

        var app = builder.Build();
        ConfigureApp(app);

        app.Run();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<HttpContextAccessor>();

        // Add services to the container.
        services.AddRazorComponents()
            .AddInteractiveServerComponents();

        services.AddSingleton<ApiService>();
        services.AddScoped<AuthService>();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.FromMinutes(1)
                };
            });

        services.Configure<CookiePolicyOptions>(options =>
        {
            options.CheckConsentNeeded = context => true;
            options.MinimumSameSitePolicy = SameSiteMode.None;
        });

        services.AddControllers();

        services.AddLogging(logging =>
        {
            logging.AddConsole();
            logging.AddDebug();
        });
        services.AddHttpClient();
    }

    private static void ConfigureApp(WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseCookiePolicy();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseRouting();

        app.UseStaticFiles();
        app.UseAntiforgery();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();
    }
}