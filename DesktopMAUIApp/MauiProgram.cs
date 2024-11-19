using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Toolkit.Hosting;

namespace DesktopMAUIApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureSyncfusionToolkit()
            .ConfigureMauiHandlers(handlers => { })
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("SegoeUI-Semibold.ttf", "SegoeSemibold");
                fonts.AddFont("FluentSystemIcons-Regular.ttf", FluentUI.FontFamily);
            });

#if DEBUG
        builder.Logging.AddDebug();
        builder.Services.AddLogging(configure => configure.AddDebug());
#endif

        #region default
        
        builder.Services.AddSingleton<MainPageModel>();

        
        #endregion


        builder.Services.AddSingleton<ApiService>();
        builder.Services.AddSingleton<ProfilePageModel>();
        builder.Services.AddSingleton<AppShellViewModel>();

        builder.Services.AddTransientWithShellRoute<LoginPage, LoginPageModel>("login");
        builder.Services.AddTransientWithShellRoute<ProfilePage, ProfilePageModel>("profile");
        builder.Services.AddTransientWithShellRoute<MyTests, MyTestsPageModel>("my_tests");
        builder.Services.AddTransientWithShellRoute<Test, TestPageViewModel>("test");

        return builder.Build();
    }
}