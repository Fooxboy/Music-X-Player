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
using MediaManager;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Xamarin.Essentials;

namespace Fooxboy.MusicX.AndroidApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/CustomTheme")]
    public class MainActivity : AppCompatActivity, BottomNavigationView.IOnNavigationItemSelectedListener
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            AppCenter.Start("ee629636-643f-425c-9ce1-6444adada296",
                   typeof(Analytics), typeof(Crashes));

            CrossMediaManager.Current.Init(this);

            SetContentView(Resource.Layout.activity_main);

            BottomNavigationView navigation = FindViewById<BottomNavigationView>(Resource.Id.navigation);
            navigation.SetOnNavigationItemSelectedListener(this);


            if (Connectivity.NetworkAccess != NetworkAccess.Internet) { 
                Intent intent = new Intent(this.ApplicationContext, typeof(Activities.OfflineActivity));
                intent.SetFlags(ActivityFlags.NewTask);
                StartActivity(intent);
                this.Finish();
            }
            else
            {
                if (AuthService.IsLoggedIn())
                {
                    Core.Api.GetApi().VKontakte.Auth.Auto(AuthService.GetToken(), null);
                    //var f = new HomeFragment();
                    var f = new RecommendationsFragment();
                    FragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, f).Commit();
                    SetTitle(Resource.String.title_home);
                    var title = FindViewById<TextView>(Resource.Id.titlebar_title);
                    title.Text = "Рекомендации";
                }
                else
                {

                    Intent intent = new Intent(this.ApplicationContext, typeof(AuthActivity));
                    intent.SetFlags(ActivityFlags.NewTask);
                    StartActivity(intent);
                    Finish();
                }
            }
            var miniplayerFragment = new MiniPlayerFragment();
            FragmentManager.BeginTransaction().Replace(Resource.Id.miniplayer_frame, miniplayerFragment).Commit();
            
            MiniPlayerService.SetFrame(FindViewById<FrameLayout>(Resource.Id.miniplayer_frame));
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        public bool OnNavigationItemSelected(IMenuItem item)
        {
            Fragment f;
            var title = FindViewById<TextView>(Resource.Id.titlebar_title);
            switch (item.ItemId)
            {
                case Resource.Id.navigation_home:
                    // textMessage.SetText(Resource.String.title_home);
                    f = new RecommendationsFragment();
                    FragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, f).Commit();
                    title.Text = "Рекомендации";
                    return true;
                case Resource.Id.navigation_tracks:
                    f = new TracksFragment();
                    FragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, f).Commit();
                    title.Text = "Ваша музыка";
                    return true;
                case Resource.Id.navigation_popular:
                    f = new ToDoFragment();
                    FragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, f).Commit();
                    title.Text = "Популярное";
                    return true;
                case Resource.Id.navigation_search:
                    f = new SearchFragment();
                    FragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, f).Commit();
                    title.Text = "Поиск";
                    return true;
                case Resource.Id.navigation_settings:
                    f = new SettingsFragment();
                    FragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, f).Commit();
                    title.Text = "Настройки";
                    return true;
            }
            return false;
        }
    }
}

