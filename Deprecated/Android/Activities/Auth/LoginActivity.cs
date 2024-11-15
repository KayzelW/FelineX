using Android.Content;
using Android.Content.Res;
using Android.Services;
using Android.Window;
using Microsoft.Extensions.DependencyInjection;

namespace Android.Activities.Auth;

[Activity(NoHistory = true)]
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

        _authBtn = FindViewById<Button>(Resource.Id.buttonLogin);
        _loginEditText = FindViewById<EditText>(Resource.Id.editTextLogin);
        _passwordEditText = FindViewById<EditText>(Resource.Id.editTextPassword);

        _authBtn.Click += AuthBtnOnClick;
        ActionBar?.Hide();
    }

    public override void OnBackPressed()
    {
        Toast.MakeText(this, "Отсюда нельзя вернуться", ToastLength.Short)?.Show();
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

    private bool IsUserAuthorized()
    {
        return _apiService.Claims != null;
    }
}