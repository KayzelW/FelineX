using APIServer.Database;
using APIServer.Middlewares;
using APIServer.Services;
using APIServer.Services.Interfaces;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics.Internal;
using Microsoft.OpenApi.Models;

namespace APIServer;

public sealed class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        #region Aspire

        // builder.AddServiceDefaults();
        // builder.Services.AddProblemDetails();

        #endregion

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
                                   ?? throw new InvalidOperationException(
                                       "Connection string 'DefaultConnection' not found.");

            builder.Services.AddNpgsql<AppDbContext>(connectionString,
                npgOptions => { }, options =>
                {
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
                                   ?? throw new InvalidOperationException(
                                       "Connection string 'DefaultConnection' not found.");

            builder.Services.AddMySql<AppDbContext>(connectionString, ServerVersion.AutoDetect(connectionString),
                mySqlOptions => { }, options =>
                {
#if DEBUG
                    options.EnableDetailedErrors();
                    options.EnableSensitiveDataLogging();
#endif
                });
        }

        #endregion

//        builder.Services.AddStackExchangeRedisCache(options =>
//        {
//            options.Configuration = "redis:6379";
//            options.InstanceName = "SampleInstance";
//        });
        
        // builder.Services.AddHttpContextAccessor();
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

        #region CORS

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll",
                builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            options.DefaultPolicyName = "AllowAll";
        });

        #endregion


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

        // app.UseHttpLogging();
        // app.UseHttpsRedirection();
        app.UseRouting();
        app.UseCors("AllowAll");
        app.UseExceptionHandler();

        app.UseMiddleware<ListenerMiddleware>();
        app.UseMiddleware<AllowCorsOptionsMiddleware>();
        app.UseMiddleware<TokenCheckingMiddleware>();


        app.MapControllers();
    }
}