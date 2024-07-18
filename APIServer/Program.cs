using APIServer.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace APIServer;

public sealed class Program
{
    public static void Main(string[] args)
    {
        #region builder

        var builder = WebApplication.CreateBuilder(args);

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                               throw new InvalidOperationException(
                                   "Connection string 'DefaultConnection' not found.");
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));


        builder.Services.AddLogging(logging =>
        {
            logging.AddConsole();
            logging.AddDebug();
        });

#if DEBUG
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo() { Title = "Dev API", Version = "v1" });
        });
#else
        builder.Services.AddSwaggerGen();
#endif

        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();

        #endregion

        #region app

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Dev API v1"); });
        }

        app.UseHttpsRedirection();
        app.MapControllers();

#if DEBUG
        app.UseDeveloperExceptionPage();
        app.UseRouting();
#endif

        app.Run();

        #endregion

        using var dbContext = app.Services.GetRequiredService<AppDbContext>();
        Console.WriteLine("Trying to verify DB");
        dbContext.Database.EnsureCreated();
    }
}