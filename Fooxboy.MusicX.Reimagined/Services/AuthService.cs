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

namespace Fooxboy.MusicX.Reimagined.Services
{
    public static class AuthService
    {
        public static bool IsLoggedIn()
        {
            var prefs = Application.Context.GetSharedPreferences("MusicX", FileCreationMode.Private);
            return String.IsNullOrEmpty(prefs.GetString("VKToken", null)) ? false : true;
        }

        public static string GetToken()
        {
            return Application.Context.GetSharedPreferences("MusicX", FileCreationMode.Private).GetString("VKToken", null);
        }
    }
}