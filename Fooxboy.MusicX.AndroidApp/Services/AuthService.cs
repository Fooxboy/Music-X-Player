using Android.App;
using System;
using Android.Content;

namespace Fooxboy.MusicX.AndroidApp.Services
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