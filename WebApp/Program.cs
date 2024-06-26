using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Shared.DB.Classes.User;
using WebApp.Components;
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

        // Add services to the container.
        services.AddRazorComponents()
            .AddInteractiveServerComponents();

        
        services.AddSingleton<ApiService>();
        services.AddSingleton<AuthService>();

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