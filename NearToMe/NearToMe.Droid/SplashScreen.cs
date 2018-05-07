using Android.App;
using Android.Content.PM;
using MvvmCross.Droid.Views;

namespace NearToMe.Droid
{
    [Activity(Label = "NearToMe",MainLauncher = true,Icon = "@drawable/Logo",Theme = "@style/Theme.Splash",NoHistory = true,ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashScreen : MvxSplashScreenActivity
    {
        public SplashScreen(): base(Resource.Layout.SplashScreen)
        {

        }
    }
}
