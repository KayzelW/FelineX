using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using System.Threading.Tasks;
using Android.Services;
using Microsoft.Extensions.DependencyInjection;
using Android.Content;


namespace Android;

[Activity(Label = "LoginActivity")]
internal class LoginActivity : Activity
{
    private ApiService _apiService { get; set; }

    private Button _authBtn { get; set; }
    private EditText _loginEditText { get; set; }
    private EditText _passwordEditText { get; set; }


    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        SetContentView(Resource.Layout.activity_login);

        _apiService = MainActivity.ServiceProvider.GetRequiredService<ApiService>();

        _authBtn = FindViewById<Button>(Resource.Id.LoginForm_AuthBtn);
        _loginEditText = FindViewById<EditText>(Resource.Id.LoginForm_LoginEdit);
        _passwordEditText = FindViewById<EditText>(Resource.Id.LoginForm_PasswordEdit);

        _authBtn.Click += AuthBtnOnClick;
    }

    private void NavigateToSignUp()
    {
        // Navigate to sign-up activity
        StartActivity(typeof(SignUpActivity));
    }


    private async void AuthBtnOnClick(object? sender, EventArgs e)
    {
        var auth = await _apiService.AuthAsync(_loginEditText.Text, _passwordEditText.Text);
        if (auth) 
        {
            var intent = new Intent(this, typeof(HomeActivity));
            StartActivity(intent);
        }
        
    }
}