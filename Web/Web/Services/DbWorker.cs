using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Data;
using Shared.Data.Test;
using Shared.Data.Test.Task;
using Shared.Extensions;
using Shared.Types;
using Web.Extensions;

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
            
            if (dbContext.Tests.ToList().Count==0)
            {
                var root = dbContext.Users.FirstOrDefault(x => x.UserName == "root");
                var test = new UniqueTest();
                await dbContext.Tests.AddAsync(test, stoppingToken);
                var task1 = new UniqueTask()
                {
                    Creator = root,
                    CreatorId = root.Id,
                    Settings = new TaskSettings(),
                    Question = "Как дышать",
                    InteractionType = InteractionType.ShortStringTask,
                    Thematics = null,
                    Tests = null,
                    DatabaseType = null,
                    DataRows = null,
                };
                task1.VariableAnswers!.Add(new VariableAnswer("Как дышать", true));
                test.Tasks!.Add(task1);
                
                var task2 = new UniqueTask()
                {
                    Creator = root,
                    CreatorId = root.Id,
                    Settings = new TaskSettings(),
                    Question = "Много выбора",
                    InteractionType = InteractionType.ManyVariantsTask,
                    Thematics = null,
                    Tests = null,
                    DatabaseType = null,
                    DataRows = null,
                };
                task2.VariableAnswers!.AddRange([
                    new VariableAnswer("Правда", true),
                    new VariableAnswer("Правда2", true),
                    new VariableAnswer("Правда3", true),
                    new VariableAnswer("kj;m", false)
                ]);
                test.Tasks!.Add(task2);
                
                var task3 = new UniqueTask()
                {
                    Creator = root,
                    CreatorId = root.Id,
                    Settings = new TaskSettings(),
                    Question = "Один выбор",
                    InteractionType = InteractionType.OneVariantTask,
                    Thematics = null,
                    Tests = null,
                    DatabaseType = null,
                    DataRows = null,
                };
                task3.VariableAnswers!.AddRange([
                    new VariableAnswer("Правда", true),
                    new VariableAnswer("fg", false),
                    new VariableAnswer("fgjkh", false),
                    new VariableAnswer("kj;m", false)
                ]);
                test.Tasks!.Add(task3);
                
                test.Creator = root;
                test.CreatorId = root.Id;
                test.TestName = "CringeTest";
                test.CreationTime = DateTime.Now;
                
                
                await dbContext.SaveChangesAsync(stoppingToken);
            }
        }
    }
}