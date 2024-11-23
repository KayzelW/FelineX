namespace DesktopMAUIApp.Pages;

public partial class TestResult : ContentPage
{

    public double? TestAnswerScore;
    public string CatScoreImage;

    private bool _isLoading;
    private bool _isLoaded;
    public TestResult()
	{
        InitializeComponent();
        TestAnswerScore = 70; //await _apiService.GetTestScore(Guid.Parse(answeredTestId));
        if (TestAnswerScore.HasValue)
        {
            var catScore = (int)Math.Floor(TestAnswerScore.Value / 10);
            CatScoreImage = $"cat{catScore}.png";
            //CatScoreImage = $"DesktopMAUIApp/Resources/images/cats/{catScore}.png";
        }
        CatScoreImage = "cat5.png";

        _isLoaded = false;
        _isLoaded = true;



    }


}