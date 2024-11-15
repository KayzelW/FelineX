using Android.Content;
using Android.Util;
using Android.Views;
using Android.Window;

namespace Android.Activities.Tests;

[Activity]
public class TestActivity : Activity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        SetContentView(Resource.Layout.activity_test);
    }

    public override void OnBackPressed()
    {
        if (IsTestAllowExit())
        {
            base.OnBackPressed();
        }
        else
        {
            Toast.MakeText(this, "Пока уйти нельзя", ToastLength.Long)?.Show();
        }
    }

    private bool IsTestAllowExit()
    {
        return false;
    }
}