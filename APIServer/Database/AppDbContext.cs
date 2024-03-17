using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shared.DB.Classes.Task;
using Shared.DB.Classes.User;
using MyTask = Shared.DB.Classes.Task.Task;

namespace APIServer.Database;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : DbContext(options)
{
    public DbSet<User>? Users { get; set; }
    public DbSet<ThemeTask>? ThemeTasks { get; set; }
    public DbSet<MyTask>? Tasks { get; set; }
    public DbSet<VariableAnswer> VariableAnswers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            optionsBuilder.UseSqlite(config.GetConnectionString("DefaultConnection"));
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}