using System;
using System.Collections.Generic;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Auth.Api;
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Android.OS;
using Android.Widget;
using Java.Security;
using NearToMe.Core.ViewModels;
using Xamarin.Facebook;
using Xamarin.Facebook.Login;
using Xamarin.Facebook.Login.Widget;

namespace NearToMe.Droid.Views
{
    [Activity(Label = "LoginView",Theme = "@style/NTMTheme.Base")]
    public class LoginView : BaseView<LoginViewModel>,IFacebookCallback, GoogleApiClient.IOnConnectionFailedListener
    {
        private ICallbackManager mCallBackManager;
        private GoogleApiClient mGoogleApiClient;
        public MyProfileTracker mProfileTracker;
        Button btnGoogle;
        const int RC_SIGN_IN = 2;

        protected override int LayoutResource => Resource.Layout.LoginView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //for facebook
            FacebookSdk.SdkInitialize(this.ApplicationContext);
            try
            {
                PackageInfo info = this.PackageManager.GetPackageInfo("natrix.NTM.droid",PackageInfoFlags.Signatures);
                foreach (Android.Content.PM.Signature signature in info.Signatures)
                {
                    MessageDigest md = MessageDigest.GetInstance("SHA");
                    md.Update(signature.ToByteArray());
                    string keyhash = Convert.ToBase64String(md.Digest());
                    Console.WriteLine("keyhash:", keyhash);
                }
            }
            catch (PackageManager.NameNotFoundException e)
            {

            }
            catch (NoSuchAlgorithmException e)
            {

            }
            mProfileTracker = new MyProfileTracker();
            mProfileTracker.mOnProfileChanged += OnProfileChanged;              
            mProfileTracker.StartTracking();

            //for google
            GoogleSignInOptions gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn)
                   .RequestEmail()
                   .Build();

            mGoogleApiClient = new GoogleApiClient.Builder(this)
                    .EnableAutoManage(this, this)
                    .AddApi(Auth.GOOGLE_SIGN_IN_API, gso)
                    .Build();

            

            SetUI();                
        }

        private void OnProfileChanged(object sender, OnProfileChangedEventArgs e)
        {
            //facebook
            if(e.mProfile!=null)
            {
                Toast.MakeText(this, e.mProfile.FirstName.ToString(), ToastLength.Short).Show();
            }
            else
            {
                Toast.MakeText(this, "User logged out", ToastLength.Short).Show();
            }
         
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            //facebook
            mCallBackManager.OnActivityResult(requestCode, (int)resultCode, data);
            if (requestCode == RC_SIGN_IN)
            {
                var result = Auth.GoogleSignInApi.GetSignInResultFromIntent(data);
                HandleSignInResult(result);
            }
        }

        private void HandleSignInResult(GoogleSignInResult result)
        {
            //google
            if (result.IsSuccess)
            {
                // Signed in successfully, show authenticated UI.
                var acct = result.SignInAccount;
                ShowHomeView();
                Toast.MakeText(this, acct.DisplayName + " " + acct.Email + " " + acct.FamilyName, ToastLength.Long).Show();               
            }
        }

        private void SetUI()
        {
            LoginButton loginBtn = FindViewById<LoginButton>(Resource.Id.btnFacebook);
            loginBtn.SetReadPermissions(new List<string>() { "email", "public_profile", "user_friends" });
            mCallBackManager = CallbackManagerFactory.Create();
            loginBtn.RegisterCallback(mCallBackManager, this);
            btnGoogle = FindViewById<Button>(Resource.Id.btnGoogle);
            btnGoogle.Click += BtnGoogle_Click;
        }

        private void BtnGoogle_Click(object sender, EventArgs e)
        {
            SignInWithGoogle();
        }

        private void SignInWithGoogle()
        {
            var signInIntent = Auth.GoogleSignInApi.GetSignInIntent(mGoogleApiClient);
            StartActivityForResult(signInIntent, RC_SIGN_IN);
        }

        private void ShowHomeView()
        {
            Intent homeViewIntent = new Intent(this, typeof(MainView));
            homeViewIntent.AddFlags(ActivityFlags.ClearTop);
            StartActivity(homeViewIntent);
            Finish();
        }
        protected override void OnDestroy()
        {
            mProfileTracker.StopTracking();
            base.OnDestroy();
        }
        public void OnCancel()
        {}

        public void OnError(FacebookException error)
        {}  
        
        public void OnSuccess(Java.Lang.Object result)
        {
            ShowHomeView();
            LoginResult loginResult = result as LoginResult;
            Toast.MakeText(this, loginResult.AccessToken.UserId.ToString(),ToastLength.Short).Show();
        }
        public void OnConnectionFailed(ConnectionResult result)
        {
            //throw new NotImplementedException();
        }
    }

    public class MyProfileTracker : ProfileTracker
    {
        public EventHandler<OnProfileChangedEventArgs> mOnProfileChanged;
        protected override void OnCurrentProfileChanged(Profile oldProfile, Profile currentProfile)
        {
            if(mOnProfileChanged!=null)
            {
                mOnProfileChanged.Invoke(this, new OnProfileChangedEventArgs(currentProfile));
            }
           
        }
    }
    public class OnProfileChangedEventArgs:EventArgs
    {
        public Profile mProfile;
        public OnProfileChangedEventArgs(Profile profile)
        {
            mProfile = profile;
        }
    }
}