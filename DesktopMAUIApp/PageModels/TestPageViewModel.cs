using System.Collections.ObjectModel;
using System.Text.Json;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Shared.Data.Test;
using Shared.Data.Test.Task;
using Shared.Extensions;
using Shared.Models;

namespace DesktopMAUIApp.PageModels;

[QueryProperty(nameof(TestId), "id")]
public partial class TestPageViewModel : ObservableObject
{
    private readonly ApiService _apiService;
    [ObservableProperty] private string _testId;
    [ObservableProperty] private bool _isUnauthorized = true;
    [ObservableProperty] private bool _isNotSended = true;
    [ObservableProperty] private string _userName;
    [ObservableProperty] private TestDTO _test;
    [ObservableProperty] private ObservableCollection<UniqueTask> _tasks;

    public TestPageViewModel(ApiService apiService)
    {
        _apiService = apiService;
    }

    [RelayCommand]
    private async Task Appearing()
    {
        Test = await _apiService.GetTest(TestId.ToGuid());
        if (Test?.Tasks != null)
        {
            Tasks = Test.Tasks.ToObservableCollection();
        }
    }

    [RelayCommand]
    private async Task SaveAndSubmit()
    {
        IsNotSended = false;
        var ansTestId = await _apiService.SubmitTest(Test);
        await Shell.Current.GoToAsync($"test_result?testAnswerId={ansTestId}").ConfigureAwait(false);
    }
}

