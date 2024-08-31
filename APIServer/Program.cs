using APIServer.Database;
using APIServer.Middlewares;
using APIServer.Services;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace APIServer;

public sealed class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        ConfigureServices(builder);

        var app = builder.Build();
        ConfigureApplication(app);

        app.Run();
        
        using var dbContext = app.Services.GetRequiredService<AppDbContext>();
        {
            Console.WriteLine("Trying to verify DB");
            dbContext.Database.EnsureCreated();
        }

    }


    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        #region Database connection

        try
        {
            //postrges
            var connectionString = builder.Configuration.GetConnectionString("postgres")
                                   ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            builder.Services.AddDbContextFactory<AppDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
#if DEBUG
                options.EnableDetailedErrors();
                options.EnableSensitiveDataLogging();
#endif
            });
        }
        catch (Exception e) 
        {
            if (e is InvalidOperationException)
            {
                throw;
            }
            Console.WriteLine($"Failed connect to Postgres");
            
            // mysql
            var connectionString = builder.Configuration.GetConnectionString("mysql") 
                                   ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            builder.Services.AddDbContextFactory<AppDbContext>(options =>
            {
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
#if DEBUG
                options.EnableDetailedErrors();
                options.EnableSensitiveDataLogging();
#endif
            });
        }

        #endregion

        
        builder.Services.AddHttpContextAccessor();
        // builder.Services.AddHostedService<TokenService>();
        builder.Services.AddSingleton<TokenService>();
        builder.Services.AddSingleton<ITestWarriorQueue, TestWarrior>();
        builder.Services.AddSingleton<CheckQueueService>();
        builder.Services.AddHostedService<TestWarrior>();
        
        builder.Services.AddHttpLogging(logging =>
        {
            logging.LoggingFields = HttpLoggingFields.All;
            logging.RequestBodyLogLimit = 4096;
            logging.ResponseBodyLogLimit = 4096;
        });
        
        builder.Services.AddLogging(logging =>
        {
            logging.AddConsole();
            if (builder.Environment.IsDevelopment())
            {
                logging.AddDebug();
            }
        });

        
        #region swagger

        if (builder.Environment.IsDevelopment())
        {
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo() { Title = "Dev API", Version = "v1" });
            });
        }

        #endregion

        
        #region Cors

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin",
                config =>
                {
                    config.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
        });

        #endregion

        
        // builder.WebHost.ConfigureKestrel(options => //https working
        // {
        //     options.ListenAnyIP(7281, listenOptions => { listenOptions.UseHttps(); });
        // });

        
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
    }


    private static void ConfigureApplication(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Dev API v1"); });

            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();
        app.UseHttpLogging();

        app.UseMiddleware<TokenCheckingMiddleware>();
        app.UseHttpsRedirection();
        app.MapControllers();
        app.UseCors("AllowSpecificOrigin");
    }
}