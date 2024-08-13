using APIServer.Database;
using APIServer.Middlewares;
using APIServer.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace APIServer;

public sealed class Program
{
    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        #region Database connection

        try
        {
            //postrges
            var connectionString = builder.Configuration.GetConnectionString("postgres") ??
                                   throw new InvalidOperationException(
                                       "Connection string 'DefaultConnection' not found.");

            builder.Services.AddDbContextFactory<AppDbContext>(options =>
                options.UseNpgsql(connectionString));
            // builder.Services.AddDbContext<AppDbContext>(options =>
            //     options.UseNpgsql(connectionString));
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed connect to Postgres");

            //mysql
            var connectionString = builder.Configuration.GetConnectionString("mysql") ??
                                   throw new InvalidOperationException(
                                       "Connection string 'DefaultConnection' not found.");

            builder.Services.AddDbContextFactory<AppDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
            // builder.Services.AddDbContext<AppDbContext>(options =>
            //     options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
        }

        #endregion

        builder.Services.AddHttpContextAccessor();
        // builder.Services.AddHostedService<TokenService>();
        builder.Services.AddSingleton<TokenService>();
        builder.Services.AddSingleton<ITestWarriorQueue,TestWarrior>();
        builder.Services.AddSingleton<CheckQueueService>();
        builder.Services.AddHostedService<TestWarrior>();


        builder.Services.AddLogging(logging =>
        {
            logging.AddConsole();
            logging.AddDebug();
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

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
    }

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        ConfigureServices(builder);

        var app = builder.Build();
        ConfigureApplication(app);

        using var dbContext = app.Services.GetRequiredService<AppDbContext>();
        Console.WriteLine("Trying to verify DB");
        dbContext.Database.EnsureCreated();
    }

    private static void ConfigureApplication(WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Dev API v1"); });

            app.UseDeveloperExceptionPage();
            app.UseRouting();
        }

        app.UseHttpsRedirection();
        app.MapControllers();
        app.UseCors("AllowSpecificOrigin");
        app.UseMiddleware<TokenCheckingMiddleware>();

        app.Run();
    }
}