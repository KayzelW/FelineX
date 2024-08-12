using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shared.DB.Test;
using Shared.DB.Test.Task;
using Shared.DB.User;
using Shared.DB.Test.Answers;
using Shared.DB.Test.Task;
using Task = Shared.DB.Test.Task.Task;
using TestSettings = Shared.DB.Test.TestSettings;

namespace APIServer.Database;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<ThemeTask> ThemeTasks { get; set; }
    public DbSet<Test> Tests { get; set; }
    public DbSet<TestSettings> TestSettings { get; set; }
    public DbSet<Task> Tasks { get; set; }
    public DbSet<TaskSettings> TaskSettings { get; set; }
    public DbSet<VariableAnswer> VariableAnswers { get; set; }
    public DbSet<UserGroup> Groups { get; set; }
    public DbSet<TestAnswer> TestAnswers { get; set; }
    public DbSet<TaskAnswer> TaskAnswers { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured) return;
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        var connectionString = config.GetConnectionString("postgres");
        if (!string.IsNullOrEmpty(connectionString))
        {
            optionsBuilder.UseNpgsql(connectionString);
        }
        else
        {
            Console.WriteLine($"Failed to read PostgreSQL connection string. Try load MySQL");
            connectionString = config.GetConnectionString("mysql");
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region SettingsConfigure
        
        modelBuilder.Entity<TestSettings>()
            .HasMany(x => x.TasksThemes)
            .WithMany();
        modelBuilder.Entity<TestSettings>()
            .HasMany(x => x.TestGroups)
            .WithMany();
        
        #endregion
        
        #region TaskConfigure

        modelBuilder.Entity<Task>()
            .HasMany(x => x.Thematics)
            .WithMany(task => task.Tasks);
        modelBuilder.Entity<Task>()
            .HasMany(x => x.VariableAnswers)
            .WithOne();
        modelBuilder.Entity<Task>()
            .HasOne(x => x.Settings)
            .WithOne();
        modelBuilder.Entity<Task>()
            .Navigation(x => x.Settings)
            .AutoInclude();
        modelBuilder.Entity<Task>()
            .Navigation(x => x.VariableAnswers)
            .AutoInclude();

        #endregion

        #region TestConfigure

        modelBuilder.Entity<Test>()
            .HasMany(x => x.Tasks)
            .WithMany(x => x.Tests);
        modelBuilder.Entity<Test>()
            .HasOne(x => x.Creator)
            .WithOne();
        modelBuilder.Entity<Test>()
            .Navigation(x => x.Tasks)
            .AutoInclude();
        modelBuilder.Entity<Test>()
            .Navigation(x => x.Settings)
            .AutoInclude();
        
        #endregion

        #region UserConfigure

        modelBuilder.Entity<User>()
            .HasIndex(e => e.UserName)
            .IsUnique();
        modelBuilder.Entity<User>()
            .HasMany(x => x.UserGroups)
            .WithMany(x => x.Students);

        #endregion

        #region AnswerConfigure

        modelBuilder.Entity<TestAnswer>()
            .HasMany(testAns => testAns.TaskAnswers)
            .WithOne();
        modelBuilder.Entity<TaskAnswer>()
            .HasMany(x => x.MarkedVariables)
            .WithMany();

        #endregion

        #region usingId

        modelBuilder.Entity<Test>()
            .HasOne(x => x.Creator)
            .WithMany()
            .HasForeignKey(x => x.CreatorId);
        modelBuilder.Entity<Test>()
            .HasOne<TestSettings>(x => x.Settings)
            .WithMany()
            .HasForeignKey(x => x.SettingsId);
        
        modelBuilder.Entity<Task>()
            .HasOne(x => x.Creator)
            .WithMany()
            .HasForeignKey(x => x.CreatorId);

        modelBuilder.Entity<TaskAnswer>()
            .HasOne(x => x.Student)
            .WithMany()
            .HasForeignKey(x => x.StudentId);
        modelBuilder.Entity<TaskAnswer>()
            .HasOne(x => x.AnsweredTask)
            .WithMany()
            .HasForeignKey(x => x.AnsweredTaskId);
        modelBuilder.Entity<UserGroup>()
            .HasOne(x => x.GroupCreator)
            .WithMany()
            .HasForeignKey(x => x.GroupCreatorId);
        modelBuilder.Entity<UserGroup>()
            .Navigation(x => x.Students)
            .AutoInclude();
        modelBuilder.Entity<TestAnswer>()
            .HasOne(x => x.Student)
            .WithMany()
            .HasForeignKey(x => x.StudentId);
        modelBuilder.Entity<TestAnswer>()
            .HasOne(x => x.AnsweredTest)
            .WithMany()
            .HasForeignKey(x => x.AnsweredTestId);

        #endregion
    }
}