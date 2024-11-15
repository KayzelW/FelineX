namespace Android.Activities.Tests;

[Activity]
public class TestResultActivity : Activity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        SetContentView(Resource.Layout.activity_test_result);
    }
}