namespace DesktopMAUIApp.Pages;

public partial class TestResult : ContentPage
{
    private readonly TestResultPageModel _viewModel;
    public TestResult(TestResultPageModel model)
	{
		//InitializeComponent();
        BindingContext = model;
        
    }


}