
using Android.App;
using Android.Content.PM;
using Android.OS;

namespace ImpressoApp.Droid
{
    [Activity(Label = "Impresso", Icon = "@mipmap/icon", Theme = "@style/ImpressoSplashTheme", ScreenOrientation = ScreenOrientation.Portrait, MainLauncher = true, NoHistory = true)]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            StartActivity(typeof(MainActivity));
        }
    }
}
