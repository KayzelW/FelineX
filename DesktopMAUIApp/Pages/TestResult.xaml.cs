namespace DesktopMAUIApp.Pages;

public partial class TestResult : ContentPage
{
    public TestResult(TestResultPageModel model)
    {
        InitializeComponent();
        BindingContext = model;
    }
}