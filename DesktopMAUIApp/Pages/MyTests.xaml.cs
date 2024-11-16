using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopMAUIApp.Pages;

public partial class MyTests : ContentPage
{
    public MyTests(MyTestsPageModel model)
    {
        InitializeComponent();
        BindingContext = model;
    }
}