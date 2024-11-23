namespace DesktopMAUIApp;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var appShellViewModel = activationState!.Context.Services.GetRequiredService<AppShellViewModel>();
        return new Window(new AppShell(appShellViewModel));
    }
}