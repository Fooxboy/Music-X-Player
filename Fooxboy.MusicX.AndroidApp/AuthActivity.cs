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
using Fooxboy.MusicX.Core.VKontakte;

namespace Fooxboy.MusicX.AndroidApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme")]
    public class AuthActivity : Activity
    {
        private EditText loginText;
        private EditText passwordText;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_auth);
            loginText = FindViewById<EditText>(Resource.Id.vklogin);
            passwordText = FindViewById<EditText>(Resource.Id.vkpassword);

            Button btn = FindViewById<Button>(Resource.Id.log_in_btn);
            btn.Click += (sender, e) => 
            {
                Toast.MakeText(this, "Пытаюсь связаться с ВКонтакте...", ToastLength.Long).Show();
                var token = Auth.User(loginText.Text, passwordText.Text, TwoFactorAuth, null);
                Toast.MakeText(this, "Ваш токен " + token, ToastLength.Long).Show();
            };

        }

        private string TwoFactorAuth()
        {
            Intent intent = new Intent(this.ApplicationContext, typeof(TwoFactorDialogActivity));
            intent.SetFlags(ActivityFlags.NewTask);
            StartActivity(intent);
            

            return null;
        }
    }
}