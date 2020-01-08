using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Fooxboy.MusicX.AndroidApp.Services;
using ImageViews.Rounded;
using Java.Lang;

namespace Fooxboy.MusicX.AndroidApp.Resources.fragments
{
    public class SettingsFragment : Fragment, View.IOnClickListener, Android.Widget.CompoundButton.IOnCheckedChangeListener
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.activity_settings, container, false);
            var exitBtn = view.FindViewById<Button>(Resource.Id.exit_btn);
            var displayname = view.FindViewById<TextView>(Resource.Id.vkname);
            var vkPfp = view.FindViewById<RoundedImageView>(Resource.Id.vk_pfp);
            var fooxboy = view.FindViewById<Button>(Resource.Id.fooxboy_button);
            var ver = view.FindViewById<TextView>(Resource.Id.settings_version);
            var checkbox = view.FindViewById<CheckBox>(Resource.Id.streamtovk_checkbox);

            var prefs = Application.Context.GetSharedPreferences("MusicX", FileCreationMode.Private);
            checkbox.Checked = prefs.GetBoolean("StreamToStatus", false);
            checkbox.SetOnCheckedChangeListener(this);

            ver.Text = $"Версия приложения {Application.Context.ApplicationContext.PackageManager.GetPackageInfo(Application.Context.ApplicationContext.PackageName, 0).VersionName}";
            fooxboy.Click += (sender, e) =>
            {
                var intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse("https://t.me/fooxboy")) ;
                StartActivity(intent);
            };
            var dumbcat = view.FindViewById<Button>(Resource.Id.dumbcat_button);
            dumbcat.Click += (sender, e) =>
            {
                var intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse("https://t.me/Luckyca7"));
                StartActivity(intent);
            };
            Task.Run(() =>
                {
                    //TODO получение юзердаты
                    //var userdata = Core.Api.GetApi().VKontakte.Auth.Auto(AuthService.GetToken(), null);
                    /*var first = userdata.FirstName;
                    var last = userdata.LastName;
                    var im = ImagesService.PhotoUser(userdata.PhotoUser);
                    Handler handler = new Handler(Looper.MainLooper);
                    handler.Post(new Runnable(() =>
                    {
                        displayname.Text = $"{first} {last}"; 
                        vkPfp.SetImageString(im, vkPfp.Width, vkPfp.Height);
                    }));*/
                }
                );
            exitBtn.SetOnClickListener(this);
            return view;
        }

        public void OnClick(View v)
        {
            var prefs = Application.Context.GetSharedPreferences("MusicX", FileCreationMode.Private);
            var editor = prefs.Edit();
            editor.PutString("VKToken", null);
            editor.Commit();
            Intent intent = new Intent(Application.Context, typeof(AuthActivity));
            intent.SetFlags(ActivityFlags.NewTask);
            Application.Context.StartActivity(intent);
        }

        public void OnCheckedChanged(CompoundButton buttonView, bool isChecked)
        {
            var prefs = Application.Context.GetSharedPreferences("MusicX", FileCreationMode.Private).Edit();
            prefs.PutBoolean("StreamToStatus", isChecked);
            prefs.Commit();

        }
    }
}