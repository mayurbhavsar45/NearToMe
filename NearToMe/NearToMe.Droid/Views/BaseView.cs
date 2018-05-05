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
using MvvmCross.Droid.Views;
using NearToMe.Core.ViewModels;

namespace NearToMe.Droid.Views
{
    [Activity(Label = "BaseView")]
    public class BaseView<TViewModel> : MvxActivity<TViewModel> where TViewModel :BaseViewModel
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature(WindowFeatures.NoTitle);
        }
    }
}