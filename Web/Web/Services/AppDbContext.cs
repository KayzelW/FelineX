using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shared.Data;
using Shared.Data.Test;
using Shared.Data.Test.Answers;
using Shared.Data.Test.Task;

namespace Web.Services;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityDbContext<ApplicationUser, ApplicationRole, string>(options)
{
    public DbSet<ApplicationUser> Users { get; set; }
    public DbSet<ThemeTask> ThemeTasks { get; set; }
    public DbSet<UniqueTest> Tests { get; set; }
    public DbSet<TestSettings> TestSettings { get; set; }
    public DbSet<TestLink> TestLinks { get; set; }
    public DbSet<UniqueTask> Tasks { get; set; }
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

        // if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
        // {
        //     optionsBuilder.EnableSensitiveDataLogging();
        //     optionsBuilder.EnableDetailedErrors();
        // }

        optionsBuilder.UseNpgsql(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        #region TestConfigure

        modelBuilder.Entity<UniqueTest>()
            .HasMany(x => x.Tasks)
            .WithMany(x => x.Tests);
        modelBuilder.Entity<UniqueTest>()
            .HasOne(x => x.Creator)
            .WithMany();
        modelBuilder.Entity<UniqueTest>()
            .HasOne(x => x.Creator)
            .WithMany()
            .HasForeignKey(x => x.CreatorId);
        modelBuilder.Entity<UniqueTest>()
            .HasMany(x => x.Settings)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<UniqueTest>()
            .Navigation(x => x.Tasks)
            .AutoInclude();
        modelBuilder.Entity<UniqueTest>()
            .Navigation(x => x.Settings)
            .AutoInclude();
        
        #endregion

        #region TaskConfigure

        modelBuilder.Entity<UniqueTask>()
            .HasMany(x => x.Thematics)
            .WithMany(task => task.Tasks);
        modelBuilder.Entity<UniqueTask>()
            .HasMany(x => x.VariableAnswers)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<UniqueTask>()
            .Navigation(x => x.Settings)
            .AutoInclude();
        modelBuilder.Entity<UniqueTask>()
            .Navigation(x => x.VariableAnswers)
            .AutoInclude();

        modelBuilder.Entity<UniqueTask>()
            .HasOne(x => x.Creator)
            .WithMany()
            .HasForeignKey(x => x.CreatorId);
        modelBuilder.Entity<UniqueTask>()
            .HasOne(x => x.Settings)
            .WithOne(y => y.AssignedTask)
            .HasForeignKey<TaskSettings>()
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<UniqueTask>()
            .Navigation(x => x.Settings)
            .AutoInclude();
        
        #endregion

        #region SettingsConfigure

        modelBuilder.Entity<TestSettings>()
            .HasMany(x => x.TasksThemes)
            .WithMany();
        modelBuilder.Entity<TestSettings>()
            .HasMany(x => x.TestGroups)
            .WithMany();
        modelBuilder.Entity<TestSettings>()
            .HasMany(x => x.TestUsers)
            .WithMany();
        modelBuilder.Entity<TestSettings>()
            .Navigation(x => x.TestGroups)
            .AutoInclude();
        modelBuilder.Entity<TestSettings>()
            .Navigation(x => x.TestUsers)
            .AutoInclude();
        modelBuilder.Entity<TestSettings>()
            .Navigation(x => x.TasksThemes)
            .AutoInclude();

        #endregion

        #region TestLinkConfigure

        modelBuilder.Entity<TestLink>()
            .HasOne(x => x.Test)
            .WithMany()
            .HasForeignKey(u => u.TestId);
        modelBuilder.Entity<TestLink>()
            .HasOne(x => x.TestSettings)
            .WithMany()
            .HasForeignKey(u => u.TestSettingsId);

        #endregion

        #region UserConfigure

        modelBuilder.Entity<ApplicationUser>((t) =>
        {
            t.HasMany(x => x.UserGroups)
                .WithMany(x => x.Students);
        });

        #endregion

        #region AnswerConfigure

        modelBuilder.Entity<TestAnswer>()
            .HasOne(x => x.AnsweredTest)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<TestAnswer>()
            .HasMany(testAns => testAns.TaskAnswers)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TaskAnswer>()
            .HasMany(x => x.MarkedVariables)
            .WithMany();

        modelBuilder.Entity<TaskAnswer>()
            .HasOne(x => x.TestAnswer)
            .WithMany(x => x.TaskAnswers)
            .HasForeignKey(x => x.TestAnswerId)
            .OnDelete(DeleteBehavior.Cascade);

        #endregion

        #region usingId

        


        

        modelBuilder.Entity<TaskAnswer>()
            .HasOne(x => x.Student)
            .WithMany()
            .HasForeignKey(x => x.StudentId);
        modelBuilder.Entity<TaskAnswer>()
            .HasOne(x => x.AnsweredTask)
            .WithMany()
            .HasForeignKey(x => x.AnsweredTaskId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<UserGroup>()
            .HasOne(x => x.GroupCreator)
            .WithMany()
            .HasForeignKey(x => x.GroupCreatorId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<UserGroup>()
            .Navigation(x => x.Students)
            .AutoInclude();
        modelBuilder.Entity<TestAnswer>()
            .HasOne(x => x.Student)
            .WithMany()
            .HasForeignKey(x => x.StudentId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<TestAnswer>()
            .HasOne(x => x.AnsweredTest)
            .WithMany()
            .HasForeignKey(x => x.AnsweredTestId)
            .OnDelete(DeleteBehavior.Cascade);

        #endregion
    }
}