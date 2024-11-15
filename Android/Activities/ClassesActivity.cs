namespace Android.Activities;

[Activity]
public class ClassesActivity : Activity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        StartActivity(typeof(HomeActivity));
    }
}