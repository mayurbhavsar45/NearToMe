using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using NearToMe.Core.ViewModels;


namespace NearToMe.Droid.Views
{
    [Activity(Label = "LoginView",Theme = "@style/NTMTheme")]
    public class LoginView : BaseView<LoginViewModel>
    {
       
        protected override int LayoutResource => Resource.Layout.LoginView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetUI();
            
           
        }

        private void SetUI()
        {
            Button loginBtn = FindViewById<Button>(Resource.Id.btnFacebook);
            loginBtn.Click += LoginBtn_Click;
        }

        private void LoginBtn_Click(object sender, EventArgs e)
        {
            ShowHomeView();
        }

        private void ShowHomeView()
        {
            Intent homeViewIntent = new Intent(this, typeof(MainView));
            homeViewIntent.AddFlags(ActivityFlags.ClearTop);
            StartActivity(homeViewIntent);
            Finish();
        }
      
     
    }
}