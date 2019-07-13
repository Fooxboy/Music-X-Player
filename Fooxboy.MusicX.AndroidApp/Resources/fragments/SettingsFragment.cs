using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Fooxboy.MusicX.AndroidApp.Resources.fragments
{
    public class SettingsFragment : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.activity_settings, container, false);
            var exit_btn = view.FindViewById<ImageView>(Resource.Id.exit_btn);
            //exit_btn.Clickable = true; <--- Строка из-за которой оно падает
            //exit_btn.SetOnClickListener(new Listeners.OnClickListener());
            
            //Строки из-за которых оно падает
            /*exit_btn.Click += (sender, e) =>
            {
                var prefs = Application.Context.GetSharedPreferences("MusicX", FileCreationMode.Private);
                var editor = prefs.Edit();
                editor.PutString("VKToken", null);
                editor.Commit();
                Intent intent = new Intent(Application.Context, typeof(AuthActivity));
                intent.SetFlags(ActivityFlags.NewTask);
                Application.Context.StartActivity(intent);
            };*/
            return view;
        }
    }
}