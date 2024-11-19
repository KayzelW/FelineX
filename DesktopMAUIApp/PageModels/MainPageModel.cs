using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;


namespace DesktopMAUIApp.PageModels;

public partial class MainPageModel : ObservableObject
{
    private ApiService _apiService;
    private bool _firstRender = true;

    public MainPageModel(ApiService apiService)
    {
        _apiService = apiService;
    }

    [RelayCommand]
    private async Task Appearing()
    {
        if (_firstRender)
        {
            _firstRender = false;
            if (await _apiService.CheckAuth())
            {
                await Shell.Current.GoToAsync("//my_tests");
            }
            else
            {
                await Shell.Current.GoToAsync("//login");
            }
        }
        
    }
}