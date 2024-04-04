using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shared.DB.Classes;
using Shared.DB.Classes.Task;
using Shared.DB.Classes.User;
using MyTask = Shared.DB.Classes.Task.Task;

namespace APIServer.Database;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : DbContext(options)
{
    public DbSet<User>? Users { get; set; }
    public DbSet<ThemeTask>? ThemeTasks { get; set; }
    public DbSet<Test>? Tests { get; set; }
    public DbSet<MyTask>? Tasks { get; set; }
    public DbSet<VariableAnswer>? VariableAnswers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var connectionString = config.GetConnectionString("DefaultConnection");
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MyTask>()
            .HasMany(x => x.Thematics)
            .WithMany(task => task.Tasks);
        modelBuilder.Entity<Test>()
            .HasMany(x => x.Tasks);
        modelBuilder.Entity<User>()
            .HasMany(x => x.CreatedTests)
            .WithOne(x => x.Creator);
        modelBuilder.Entity<VariableAnswer>()
            .HasOne(x => x.Task)
            .WithMany(x => x.VariableAnswers);
    }
}