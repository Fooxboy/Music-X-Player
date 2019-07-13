using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Fooxboy.MusicX.AndroidApp.Adapters;
using Fooxboy.MusicX.AndroidApp.Resources.fragments;
using Fooxboy.MusicX.AndroidApp.Services;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace Fooxboy.MusicX.AndroidApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/CustomTheme")]
    public class MainActivity : AppCompatActivity, BottomNavigationView.IOnNavigationItemSelectedListener
    {
        private Button textMessage;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            AppCenter.Start("ee629636-643f-425c-9ce1-6444adada296",
                   typeof(Analytics), typeof(Crashes));

            SetContentView(Resource.Layout.activity_main);
            textMessage = FindViewById<Button>(Resource.Id.message);
            BottomNavigationView navigation = FindViewById<BottomNavigationView>(Resource.Id.navigation);
            navigation.SetOnNavigationItemSelectedListener(this);

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);
            ActionBar.Title = "My Toolbar";


            if (AuthService.IsLoggedIn())
            {
                Fooxboy.MusicX.Core.VKontakte.Auth.AutoSync(AuthService.GetToken(), null);
                var f = new HomeFragment();
                FragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, f).Commit();
                SetTitle(Resource.String.title_home);
            }else
            {

                Intent intent = new Intent(this.ApplicationContext, typeof(AuthActivity));
                intent.SetFlags(ActivityFlags.NewTask);
                StartActivity(intent);
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.top_menus, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            Toast.MakeText(this, "ТЫК НАХУЙ",
                ToastLength.Short).Show();
            return base.OnOptionsItemSelected(item);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        public bool OnNavigationItemSelected(IMenuItem item)
        {
            Fragment f;
            switch (item.ItemId)
            {
                case Resource.Id.navigation_home:
                    // textMessage.SetText(Resource.String.title_home);
                    f = new HomeFragment();
                    FragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, f).Commit();
                    SetTitle(Resource.String.title_home);
                    return true;
                case Resource.Id.navigation_popular:
                    Intent intent = new Intent(this.ApplicationContext, typeof(AuthActivity));
                    intent.SetFlags(ActivityFlags.NewTask);
                    StartActivity(intent);
                    return true;
                case Resource.Id.navigation_search:
                    //TODO
                    return true;
                case Resource.Id.navigation_settings:
                    //TODO
                    return true;
            }
            return false;
        }
    }
}

