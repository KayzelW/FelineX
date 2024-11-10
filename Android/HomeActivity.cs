using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace Android
{
    [Activity(Label = "Main Activity", MainLauncher = true)]
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

            Button button = FindViewById<Button>(Resource.Id.myButton);
            button.Click += (sender, e) =>
            {
                // Создание Intent для перехода на SecondActivity
                Intent intent = new Intent(this, typeof(LoginActivity));
                StartActivity(intent);
            };

        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu, menu); // Замените "drawer_menu" на имя вашего файла меню
            return true;
        }

    }
}