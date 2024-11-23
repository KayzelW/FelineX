using Android.Content;
using Android.Services;
using Android.Views;
using Microsoft.Extensions.DependencyInjection;

namespace Android;

[Activity(Label = "@string/app_name", MainLauncher = true)]
public class MainActivity : Activity
{
    

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        // Set our view from the "main" layout resource
        SetContentView(Resource.Layout.activity_main);
        //CheckAuth();
        Console.WriteLine("CLENNNNNNNNNNNN");


    }

    
}