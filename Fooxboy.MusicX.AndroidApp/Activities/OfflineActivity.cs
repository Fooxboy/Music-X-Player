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
using Java.Lang;

namespace Fooxboy.MusicX.AndroidApp.Activities
{
    [Activity(Label = "@string/app_name")]
    public class OfflineActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.offlineActivity);

            var exitBtn = FindViewById<Button>(Resource.Id.giveupButton);
            exitBtn.Click += (sender, e) => {
                JavaSystem.Exit(0);
            };
            // Create your application here
        }
    }
}