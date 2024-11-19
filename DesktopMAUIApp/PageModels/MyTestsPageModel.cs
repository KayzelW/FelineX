using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Shared.Data.Test;

namespace DesktopMAUIApp.PageModels;

public partial class MyTestsPageModel : ObservableObject
{
    private ApiService apiService;
    [ObservableProperty] private List<UniqueTest> _tests = [];

    public MyTestsPageModel(ApiService apiService)
    {
        this.apiService = apiService;
    }
    
    [RelayCommand]
    private async Task Appearing()
    {
        Tests = await apiService.GetTestsAsync() ?? [];
    }
    
    [RelayCommand]
    private Task NavigateToItem(UniqueTest test)
        => Shell.Current.GoToAsync($"test?id={test.Id}");

    [RelayCommand]
    private async Task AddItem()
    {
        // await Shell.Current.GoToAsync("main");
    }
    
}