using Android.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Android;

internal class MyTestsActivity
{
    
    public partial class MyTestsPage : Activity
    {
        private ApiService _apiService { get; set; }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_my_tests);


            _apiService = MainActivity.ServiceProvider.GetRequiredService<ApiService>();



        }


    }


}