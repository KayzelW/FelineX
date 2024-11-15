using Android.Activities.Auth;
using Android.Activities.Tests;
using Android.Content;
using Android.Views;

namespace Android.Activities
{
    [Activity]
    public class HomeActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_home);

            // Получаем доступ к элементам разметки
            var titleTextView = FindViewById<TextView>(Resource.Id.textViewTitle);
            var stickerImageView = FindViewById<ImageView>(Resource.Id.imageViewSticker);
            var workingTextView = FindViewById<TextView>(Resource.Id.textViewWorking);

            var button = FindViewById<Button>(Resource.Id.myButton);
            button.Click += (sender, e) =>
            {
                // Создание Intent для перехода на SecondActivity
                var intent = new Intent(this, typeof(LoginActivity));
                StartActivity(intent);
            };
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu, menu);
            return true;
        }

        public override bool OnContextItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.nav_my_tests:
                    StartActivity(typeof(MyTestsActivity));
                    return true;
                case Resource.Id.nav_home:
                    StartActivity(typeof(HomeActivity));
                    return true;
                case Resource.Id.nav_classes:
                    StartActivity(typeof(ClassesActivity));
                    return true;
                case Resource.Id.nav_profile:
                    StartActivity(typeof(ProfileActivity));
                    return true;
                case Resource.Id.nav_game:
                    StartActivity(typeof(GameActivity));
                    return true;
                default:
                    return base.OnContextItemSelected(item);
            }
        }
    }
}