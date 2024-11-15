using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shared.DB.Test;
using Shared.DB.Test.Answers;
using Task = Shared.DB.Test.Task.Task;

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

    
}