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
            options.UseSqlite(connectionString));

#if DEBUG
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo(){ Title = "Dev API", Version = "v1"});
        });
#endif

        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        #endregion

        #region app

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Dev API v1");
            });
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();
        app.MapControllers();


#if DEBUG
        app.UseDeveloperExceptionPage();
        app.UseRouting();
#endif

        app.Run();

        #endregion
    }
}