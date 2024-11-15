namespace Android.Activities.Auth;

[Activity]
public class ProfileActivity : Activity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        StartActivity(typeof(HomeActivity));
    }
}