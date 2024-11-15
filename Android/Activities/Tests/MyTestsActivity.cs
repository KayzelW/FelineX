using Android.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Android.Activities.Tests;

[Activity]
public class MyTestsActivity : Activity
{
    private ApiService _apiService { get; set; }

    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        SetContentView(Resource.Layout.activity_my_tests);


        _apiService = MainActivity.ServiceProvider.GetRequiredService<ApiService>();
    }
}