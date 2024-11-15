namespace Android.Activities.Auth;

[Activity]
public class SignUpActivity : Activity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        StartActivity(typeof(HomeActivity));
    }
}