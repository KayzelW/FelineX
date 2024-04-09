using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shared.DB.Classes;
using Shared.DB.Classes.Test;
using Shared.DB.Classes.Test.Task;
using Shared.DB.Classes.Test.Task.TaskAnswer;
using Shared.DB.Classes.User;
using Task = Shared.DB.Classes.Test.Task.Task;

namespace APIServer.Database;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : DbContext(options)
{
    public DbSet<User>? Users { get; set; }
    public DbSet<ThemeTask>? ThemeTasks { get; set; }
    public DbSet<Test>? Tests { get; set; }
    public DbSet<Task>? Tasks { get; set; }
    public DbSet<VariableAnswer>? VariableAnswers { get; set; }
    public DbSet<UserGroup>? Groups { get; set; }
    public DbSet<TestAnswer>? TestAnswers { get; set; }
    public DbSet<TaskAnswer>? TaskAnswers { get; set; }


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
        modelBuilder.Entity<Task>()
            .HasMany(x => x.Thematics)
            .WithMany(task => task.Tasks);
        modelBuilder.Entity<Test>()
            .HasMany(x => x.Tasks);
        modelBuilder.Entity<Test>()
            .HasOne(x => x.Creator);
        modelBuilder.Entity<User>()
            .HasMany(x => x.CreatedTests)
            .WithOne(x => x.Creator);
        modelBuilder.Entity<User>()
            .HasIndex(e => e.UserName)
            .IsUnique();
        modelBuilder.Entity<User>()
            .HasMany(u => u.UserGroups)
            .WithMany(ug => ug.Student);
        modelBuilder.Entity<TestAnswer>()
            .HasMany(testAns => testAns.TaskAnswers);
        modelBuilder.Entity<TaskAnswer>()
            .HasMany(taskAns => taskAns.GotVariables);
        modelBuilder.Entity<TaskAnswer>()
            .HasMany(taskAns => taskAns.MarkedVariables);
        modelBuilder.Entity<TaskAnswer>()
            .HasOne(x => x.Student);
        modelBuilder.Entity<TaskAnswer>()
            .HasOne(x => x.AnsweredTask);
        modelBuilder.Entity<VariableAnswer>()
            .HasOne(x => x.Task)
            .WithMany(x => x.VariableAnswers);
    }
}