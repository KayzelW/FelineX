using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Data;
using Web.Components;
using Web.Components.Account;
using Web.Services;
using Web.Services.Interfaces;
using _Imports = Web.Client._Imports;

var builder = WebApplication.CreateBuilder(args);

#region Services

builder.Services.AddControllers();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, PersistingServerAuthenticationStateProvider>();

builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("Admin", policy => policy.RequireAuthenticatedUser().RequireRole("Admin"));
    opt.AddPolicy("Teacher", policy => policy.RequireAuthenticatedUser().RequireRole("Teacher", "Admin"));
    opt.AddPolicy("Student", policy => policy.RequireAuthenticatedUser().RequireRole("Student", "Teacher", "Admin"));
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;

    // options.DefaultChallengeScheme = IdentityConstants.BearerScheme;
});

builder.Services.AddHangfire(configuration => configuration
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseMemoryStorage());
builder.Services.AddHangfireServer();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services
    .AddIdentity<ApplicationUser, ApplicationRole>(options => { options.SignIn.RequireConfirmedAccount = false; })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(opt =>
{
    {
        opt.Events.OnRedirectToAccessDenied = OnRedirectToAccessDenied;

        async Task OnRedirectToAccessDenied(RedirectContext<CookieAuthenticationOptions> context)
        {
            if (context.Request.Path.HasValue
                && context.Request.Path.Value.Contains("/api/"))
            {
                context.Response.StatusCode = 403;
                await context.Response.CompleteAsync();
                return;
            }

            context.Response.Redirect("/Account/AccessDenied");
        }
    }
});

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

builder.Services.AddHostedService<DbWorker>();
builder.Services.AddSingleton<ITestWarriorQueue, TestWarrior>();
builder.Services.AddSingleton<TestWarrior>();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();
// builder.Services.AddHttpLogging(opt =>
// {
//     opt.
// });
// builder.Services.AddHostedService<TestWarrior>();

#endregion

var app = builder.Build();

#region app

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // app.UseWebAssemblyDebugging();
    app.UseMigrationsEndPoint();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.MapHangfireDashboardWithAuthorizationPolicy("Admin", "/hangfire"); //.RequireAuthorization("Admin");

app.MapControllers();

// app.UseHttpLogging();

app.UseStaticFiles();
app.UseAntiforgery();

// app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(_Imports).Assembly);

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

#endregion

app.Run();