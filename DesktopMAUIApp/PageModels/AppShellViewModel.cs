using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;

namespace DesktopMAUIApp.PageModels;

public partial class AppShellViewModel : ObservableObject
{
    public AppShellViewModel(ProfilePageModel profilePageModel, ApiService apiService,
        ILogger<AppShellViewModel> logger)
    {
        this.profilePageModel = profilePageModel;
        _apiService = apiService;
        _logger = logger;
    }

    private readonly ProfilePageModel profilePageModel;
    private readonly ApiService _apiService;
    private readonly ILogger<AppShellViewModel> _logger;

    [ObservableProperty] private bool _isLoggedIn = false;

    partial void OnIsLoggedInChanged(bool value)
    {
        _logger.LogInformation($"IsLoggedInChanged: {value}");
        if (!OperatingSystem.IsWindows())
        {
            Toast.Make($"IsLoggedInChanged: {value}").Show();
        }
        if (value)
        {
            profilePageModel.Login();
            Shell.Current.GoToAsync("//my_tests");
        }
        else
        {
            profilePageModel.Logout();
            Shell.Current.GoToAsync("//login");
        }
        
    }

    [RelayCommand]
    private async Task Logout()
    {
        var result = await _apiService.LogoutAsync();

        if (result)
        {
            IsLoggedIn = false;
        }
    }
}

