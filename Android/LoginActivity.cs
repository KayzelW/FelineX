using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using System.Threading.Tasks;


namespace Android;


    [Activity(Label = "LoginActivity")]
        public class LoginActivity : Activity
        {
            
            protected override void OnCreate(Bundle savedInstanceState)
            {
                base.OnCreate(savedInstanceState);
                SetContentView(Resource.Layout.activity_login);

                
            }

            private void NavigateToSignUp()
            {
                // Navigate to sign-up activity
                StartActivity(typeof(SignUpActivity));
            }
        }
