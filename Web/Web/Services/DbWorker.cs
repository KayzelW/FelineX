using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Data;
namespace Web.Services;

public class DbWorker(IServiceProvider serviceProvider, ILogger<DbWorker> logger, IRecurringJobManager jobManager)
    : BackgroundService
{
    private async Task EnsureRole(RoleManager<ApplicationRole> roleManager, string name)
    {
        if (!await roleManager.RoleExistsAsync(name))
        {
            await roleManager.CreateAsync(new ApplicationRole() { Name = name });
        }
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        jobManager.AddOrUpdate<TestWarrior>("testAnswersProcessing",
            x => x.ProcessTestAnswers(), Cron.Minutely());

        jobManager.AddOrUpdate<TestWarrior>("sqlTasksProcessing",
            x => x.ProcessSqlTasks(), Cron.Minutely());

        return base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        {
            using var scope = serviceProvider.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await dbContext.Database.MigrateAsync(stoppingToken);
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            await EnsureRole(roleManager, "Student");
            await EnsureRole(roleManager, "Teacher");
            await EnsureRole(roleManager, "Admin");

            if (!dbContext.Users.Any())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var user = new ApplicationUser() { UserName = "root" };
                var result = await userManager.CreateAsync(user, "Qwerty78$");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
                else
                {
                    logger.LogError(
                        $"Failed to add user {string.Join(" ", result.Errors.Select(x => x.Code + " : " + x.Description))}");
                }
            }
        }
    }
}