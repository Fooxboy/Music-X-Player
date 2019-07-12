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

namespace Fooxboy.MusicX.AndroidApp
{
    [Activity(Label = "TwoFactorDialogActivity")]
    public class TwoFactorDialogActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.twofactor_dialog);

            var button = FindViewById<Button>(Resource.Id.twofactor_confirm);
            button.Click += Button_Click;

        }

        private void Button_Click(object sender, EventArgs e)
        {
            var text = FindViewById<EditText>(Resource.Id.authcode);

            Services.StaticContentService.CodeTwoFactorAuth = text.Text;

            Finish();
        }
    }
}