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
using Fooxboy.MusicX.Reimagined.Services;

namespace Fooxboy.MusicX.Reimagined.Activities
{
    [Activity(Label = "ErrorActivity")]
    public class ErrorActivity : Activity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_error);
            // Create your application here

            var Caption = FindViewById<TextView>(Resource.Id.errorCaption);
            var Description = FindViewById<TextView>(Resource.Id.errorDescription);

            Caption.Text = StaticContentService.ErrorCaption;
            Description.Text = StaticContentService.ErrorDescription;
        }
    }
}