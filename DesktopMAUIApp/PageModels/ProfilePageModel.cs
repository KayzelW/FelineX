using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DesktopMAUIApp.PageModels;

public partial class ProfilePageModel : ObservableObject
{
    private ApiService ApiService { get; }

    [ObservableProperty] private string _profileName;
    [ObservableProperty] private string _roleName;

    public ProfilePageModel(ApiService apiService)
    {
        ApiService = apiService;

        _profileName = ApiService.Claims![ClaimTypes.Name];
        _roleName = ApiService.Claims![ClaimTypes.Role];

        Shell.Current.IsVisible = true;
    }


    [RelayCommand]
    private async Task Logout()
    {
        var result = await ApiService.LogoutAsync();

        if (result)
        {
            ProfileName = string.Empty;
            RoleName = string.Empty;
            
            Shell.Current.IsVisible = false;
            await Shell.Current.GoToAsync("login");
        }
    }
}
