using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;

namespace DesktopMAUIApp.PageModels;

public partial class LoginPageModel : ObservableObject
{
    public ApiService ApiService { get; }
    public ILogger<LoginPageModel> Logger { get; }

    [ObservableProperty] private string _username;
    [ObservableProperty] private string _password;

    public LoginPageModel(ApiService apiService, ILogger<LoginPageModel> logger)
    {
        ApiService = apiService;
        Logger = logger;
        Shell.Current.IsVisible = true;
    }

    [RelayCommand]
    private async Task Login()
    {
        if(string.IsNullOrWhiteSpace(_username) || string.IsNullOrWhiteSpace(_password))
        {
            Logger.LogInformation("Username или Password пусты");
        }

        var result = await ApiService.LoginAsync(_username, _password);
        if (result)
        {
            Shell.Current.IsVisible = false;
            await Shell.Current.GoToAsync("profile");
        }
    }
}