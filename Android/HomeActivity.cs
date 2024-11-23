using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace Android
{
    [Activity(Label = "@string/app_name", MainLauncher = true)]
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
            Intent? intent = null;
            Button button = FindViewById<Button>(Resource.Id.myButton);

            button.Click += Button2_Click;

        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(ProfileActivity));
            StartActivity(intent);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            // set the menu layout on Main Activity  
            MenuInflater.Inflate(Resource.Menu.mainMenu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            Intent? intent = null;
            switch (item.ItemId)
            {
                case Resource.Id.menuItem1:
                    intent = new Intent(this, typeof(HomeActivity));
                    break;
                case Resource.Id.menuItem2:
                    intent = new Intent(this, typeof(ProfileActivity));
                    break;
                case Resource.Id.menuItem3:
                    intent = new Intent(this, typeof(MyTestsActivity));
                    break;
                case Resource.Id.menuItem4:
                    intent = new Intent(this, typeof(HomeActivity));
                    break;
            }

            if (intent != null)
            {
                StartActivity(intent);
                return true;
            }
            return true;

        }



    }
}