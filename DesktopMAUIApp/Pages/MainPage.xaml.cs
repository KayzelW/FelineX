using DesktopMAUIApp.Models;
using DesktopMAUIApp.PageModels;

namespace DesktopMAUIApp.Pages;

public partial class MainPage : ContentPage
{
    public MainPage(MainPageModel model)
    {
        InitializeComponent();
        BindingContext = model;
    }
}