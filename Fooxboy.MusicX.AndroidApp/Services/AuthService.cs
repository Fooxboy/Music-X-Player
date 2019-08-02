using Android.App;
using System;
using Android.Content;
using Fooxboy.MusicX.AndroidApp.DialogFragments;

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

        public static void ShowIncorrectLoginDialog(FragmentManager fm)
        {
            FragmentTransaction ft = fm.BeginTransaction();
            //Remove fragment else it will crash as it is already added to backstack
            Fragment prev = fm.FindFragmentByTag("dialog");
            if (prev != null)
            {
                ft.Remove(prev);
            }
            ft.AddToBackStack(null);
            // Create and show the dialog.
            IncorrectLoginDialogFragment newFragment = IncorrectLoginDialogFragment.NewInstance(null);
            
            //Add fragment
            newFragment.Show(ft, "dialog");
        }
    }
}