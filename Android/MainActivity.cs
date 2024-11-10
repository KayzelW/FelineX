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

        _authBtn = FindViewById<Button>(Resource.Id.LoginForm_AuthBtn);
        _loginEditText = FindViewById<EditText>(Resource.Id.LoginForm_LoginEdit);
        _passwordEditText = FindViewById<EditText>(Resource.Id.LoginForm_PasswordEdit);


        _authBtn.Click += AuthBtnOnClick;
    }

    private async void AuthBtnOnClick(object? sender, EventArgs e)
    {
        var api = ServiceProvider.GetRequiredService<ApiService>();

        await api.AuthAsync(_loginEditText.Text, _passwordEditText.Text);
    }
}