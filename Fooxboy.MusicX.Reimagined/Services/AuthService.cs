using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Fooxboy.MusicX.Core;
using Java.Lang;

namespace Fooxboy.MusicX.Reimagined.Services
{
    public static class AuthService
    {

        public static void LogOut()
        {
            var prefs = Application.Context.GetSharedPreferences("MusicXReimagined", FileCreationMode.Private);
            var editor = prefs.Edit();
            editor.PutString("VKToken", null);
            editor.Commit();
            InitiateLogin();
        }

        public static bool IsLoggedIn()
        {
            var prefs = Application.Context.GetSharedPreferences("MusicXReimagined", FileCreationMode.Private);
            return System.String.IsNullOrEmpty(prefs.GetString("VKToken", null)) ? false : true;
        }

        public static void InitiateLogin()
        {
            Intent intent = new Intent(Application.Context, typeof(Activities.AuthActivity));
            intent.SetFlags(ActivityFlags.NewTask);
            Application.Context.StartActivity(intent);
        }




        public static string GetToken()
        {
            return Application.Context.GetSharedPreferences("MusicXReimagined", FileCreationMode.Private).GetString("VKToken", null);
        }
    }
}