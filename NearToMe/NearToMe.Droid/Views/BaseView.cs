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
    public abstract class BaseView<TViewModel> : MvxActivity<TViewModel> where TViewModel :BaseViewModel
    {
        protected abstract int LayoutResource
        {
            get;
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(LayoutResource);
        }
        
    }
}