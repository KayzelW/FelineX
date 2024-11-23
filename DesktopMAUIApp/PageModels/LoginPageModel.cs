using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;

namespace DesktopMAUIApp.PageModels;

public partial class LoginPageModel : ObservableObject
{
    private readonly ApiService _apiService;
    private readonly ILogger<LoginPageModel> _logger;
    private readonly AppShellViewModel _appShellViewModel;

    [ObservableProperty] private string _username;
    [ObservableProperty] private string _password;

    public LoginPageModel(ApiService apiService, ILogger<LoginPageModel> logger, AppShellViewModel appShellViewModel)
    {
        _apiService = apiService;
        _logger = logger;
        _appShellViewModel = appShellViewModel;

        Username = "";
        Password = "";
    }

    [RelayCommand]
    private async Task Login()
    {
        if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
        {
            _logger.LogInformation("Username или Password пусты");
        }

        var result = await _apiService.LoginAsync(Username, Password);
        if (result)
        {
            _appShellViewModel.IsLoggedIn = true;
            Username = "";
            Password = "";
        }
    }
}