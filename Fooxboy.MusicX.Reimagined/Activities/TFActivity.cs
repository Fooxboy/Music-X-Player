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
    [Activity(Label = "TFActivity")]
    public class TFActivity : Activity
    {

        public override void OnBackPressed()
        {
            return;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_twofactor);
            // Create your application here

            var submitButton = FindViewById<Button>(Resource.Id.tfButton);
            var codeField = FindViewById<EditText>(Resource.Id.tfCodeField);

            submitButton.Click += (sender, e) =>
            {
                if(codeField.Text == "")
                {
                    Toast.MakeText(Application.Context, "Введите пожалуйста код, блять.", ToastLength.Long).Show();
                }
                else
                {
                    StaticContentService.TwoFactorAuthCode = codeField.Text;
                    this.Finish();
                }
            };
        }
    }
}