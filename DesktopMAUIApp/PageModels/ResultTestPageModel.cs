using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Shared.Data.Test;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DesktopMAUIApp.PageModels;

public partial class TestResultPageModel : ObservableObject
{
    private ApiService apiService;
    private readonly ApiService _apiService;
    private readonly ILogger<TestResultPageModel> _logger;

    /*
    public TestResultPageModel(ApiService apiService)
    {
        this.apiService = apiService;
        _logger = App.GetLogger<TestResultPageModel>();

        IsLoading = true;
        IsLoaded = false;
    }*/

    private double? _testAnswerScore;
    public double? TestAnswerScore
    {
        get => _testAnswerScore;
        set => SetProperty(ref _testAnswerScore, value);
    }

    private string _catScoreImage;
    public string CatScoreImage
    {
        get => _catScoreImage;
        set => SetProperty(ref _catScoreImage, value);
    }

    private bool _isLoading;
    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    private bool _isLoaded;
    public bool IsLoaded
    {
        get => _isLoaded;
        set => SetProperty(ref _isLoaded, value);
    }

    public async Task InitializeAsync(string answeredTestId)
    {
        try
        {
            TestAnswerScore = 70; //await _apiService.GetTestScore(Guid.Parse(answeredTestId));
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
            _logger.LogError($"Exception while getting test results with id {answeredTestId}: {ex}");
            IsLoading = false;
        }
    }
}
