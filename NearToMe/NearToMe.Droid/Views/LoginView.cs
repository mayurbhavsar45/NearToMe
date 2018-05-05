using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
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
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.LoginView);
            SetUI();
           
        }

        private void SetUI()
        {
            Button loginBtn = FindViewById<Button>(Resource.Id.loginBtn);
            loginBtn.Click += LoginBtn_Click;
        }

        private void LoginBtn_Click(object sender, EventArgs e)
        {
            ShowHomeView();
        }

        private void ShowHomeView()
        {
            Intent homeViewIntent = new Intent(this, typeof(BaseHomeView));
            homeViewIntent.AddFlags(ActivityFlags.ClearTop);
            StartActivity(homeViewIntent);
            Finish();
        }
    }
}