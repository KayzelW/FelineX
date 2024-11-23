using Shared.Data.Test.Task;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace DesktopMAUIApp.Pages;

public partial class Test : ContentPage
{
	public Test(TestPageViewModel model)
	{
		InitializeComponent();
        BindingContext = model;
    }
	
}