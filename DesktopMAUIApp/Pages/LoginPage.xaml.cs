using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopMAUIApp.Pages;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginPageModel model)
    {
        InitializeComponent();
        BindingContext = model;
    }
}