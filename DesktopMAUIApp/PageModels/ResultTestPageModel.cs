using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Shared.Data.Test;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Shared.Data.Test.Answers;

namespace DesktopMAUIApp.PageModels;

[QueryProperty(nameof(TestAnswerId), "testAnswerId")]
public partial class TestResultPageModel : ObservableObject
{
    [ObservableProperty] private string _testAnswerId;
    private readonly ApiService _apiService;
    private readonly ILogger<TestResultPageModel> _logger;

    [ObservableProperty] private double? _testAnswerScore;
    
    [ObservableProperty] private string _catScoreImage;

    [ObservableProperty] private bool _isLoading;

    [ObservableProperty] private bool _isLoaded;

    [RelayCommand]
    private async Task Appearing()
    {
        try
        {
            TestAnswerScore = await _apiService.GetTestScore(Guid.Parse(TestAnswerId));
            if (TestAnswerScore.HasValue)
            {
                var catScore = (int)Math.Floor(TestAnswerScore.Value / 10);
                CatScoreImage = $"images/cats/{catScore}.png";
            }
            IsLoaded = true;
            IsLoading = false;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception while getting test results with id {TestAnswerId}: {ex}");
            IsLoading = false;
        }
    }

   
}
