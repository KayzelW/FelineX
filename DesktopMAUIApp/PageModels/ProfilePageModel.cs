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

        ProfileName = ApiService.GetName ?? "notAuthorized";
        RoleName = ApiService.GetRole ?? "notAuthorizedRole";
    }
    
    public void Login()
    {
        ProfileName = ApiService.GetName ?? "notAuthorized";
        RoleName = ApiService.GetRole ?? "notAuthorizedRole";
    }

    public void Logout()
    {
        ProfileName = "notAuthorized";
        RoleName = "notAuthorizedRole";
    }
}