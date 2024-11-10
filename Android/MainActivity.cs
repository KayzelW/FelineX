using Android.Content;
using Android.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Android;

[Activity(Label = "@string/app_name", MainLauncher = true)]
public class MainActivity : Activity
{
    internal static IServiceProvider ServiceProvider { get; private set; }

    private Button _authBtn { get; set; }
    private EditText _loginEditText { get; set; }
    private EditText _passwordEditText { get; set; }

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        #region Services

        var builder = new ServiceCollection();

        builder.AddSingleton<ApiService>();
        ServiceProvider = builder.BuildServiceProvider();

        ServiceProvider.GetRequiredService<ApiService>().LoadCookies();

        #endregion

        // Set our view from the "main" layout resource
        SetContentView(Resource.Layout.activity_main);
        CheckAuth();
        
    }

    private async void CheckAuth()
    {
        var api = ServiceProvider.GetRequiredService<ApiService>();
        Intent? intent = null;
        if (!await api.CheckAuth())
        {
            intent = new Intent(this, typeof(LoginActivity));
            StartActivity(intent);
            return;
            
        } 
        intent = new Intent(this, typeof(HomeActivity));
        StartActivity(intent);
    }


}