using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Fooxboy.MusicX.AndroidApp.Adapters;
using Fooxboy.MusicX.AndroidApp.Resources.fragments;
using Fooxboy.MusicX.AndroidApp.Services;
using ImageViews.Rounded;
using MediaManager;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Xamarin.Essentials;

namespace Fooxboy.MusicX.AndroidApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/CustomTheme")]
    public class MainActivity : AppCompatActivity, BottomNavigationView.IOnNavigationItemSelectedListener, Android.Views.View.IOnClickListener
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

            var themeFlags = Application.Context.Resources.Configuration.UiMode & UiMode.NightMask;
            if (themeFlags == UiMode.NightYes) Window.DecorView.SystemUiVisibility = 0;
            else Window.DecorView.SystemUiVisibility = (StatusBarVisibility)SystemUiFlags.LightStatusBar;

            var BackButton = FindViewById<ImageView>(Resource.Id.backbuttonMain);
            BackButton.Visibility = ViewStates.Gone;
            StaticContentService.NavigationService = new NavigationService(BackButton);
            BackButton.SetOnClickListener(this);
            var title = FindViewById<TextView>(Resource.Id.titlebar_title);


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
                    var user = Core.Api.GetApi().VKontakte.Users.Info.CurrentUser();
                    Services.StaticContentService.UserId = user.Id;
                    StaticContentService.UserName = user.FirstName;
                    var pfp = FindViewById<RoundedImageView>(Resource.Id.profilepicture_main);
                    pfp.SetImageString(ImagesService.PhotoUser(user.PhotoUser), 50, 50);
                    pfp.SetOnClickListener(this);
                    //var f = new HomeFragment();
                    var f = new RecommendationsFragment();
                    StaticContentService.NavigationService.SetCurrent(f, "Рекомендации");
                    FragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, f).Commit();
                    SetTitle(Resource.String.title_home);
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
            var exitBtn = FindViewById<Button>(Resource.Id.ExitButton);
            exitBtn.Visibility = ViewStates.Gone;
            switch (item.ItemId)
            {
                case Resource.Id.navigation_home:
                    // textMessage.SetText(Resource.String.title_home);
                    f = new RecommendationsFragment();
                    StaticContentService.NavigationService.SetCurrent(f, "Рекомендации");
                    FragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, f).Commit();
                    title.Text = "Рекомендации";
                    return true;
                case Resource.Id.navigation_tracks:
                    f = new TracksFragment();
                    StaticContentService.NavigationService.SetCurrent(f, "Ваша Музыка");
                    FragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, f).Commit();
                    title.Text = "Ваша музыка";
                    return true;
                case Resource.Id.navigation_popular:
                    f = new ToDoFragment();
                    StaticContentService.NavigationService.SetCurrent(f, "Популярное");
                    FragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, f).Commit();
                    title.Text = "Популярное";
                    return true;
                case Resource.Id.navigation_search:
                    f = new SearchFragment();
                    StaticContentService.NavigationService.SetCurrent(f, "Поиск");
                    FragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, f).Commit();
                    title.Text = "Поиск";
                    return true;
                case Resource.Id.navigation_downloads:
                    //TODO: загрузОчки
                    
                    title.Text = "Загрузки";
                    return true;
            }
            return false;
        }

        public void OnClick(View v)
        {
            if(v.Id == Resource.Id.profilepicture_main)
            {
                var title = FindViewById<TextView>(Resource.Id.titlebar_title);
                var exitBtn = FindViewById<Button>(Resource.Id.ExitButton);
                exitBtn.Visibility = ViewStates.Visible;
                Fragment f = new SettingsFragment();
                FragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, f).Commit();
                title.Text = StaticContentService.UserName;
            }else if(v.Id == Resource.Id.backbuttonMain)
            {
                var f = StaticContentService.NavigationService.GoBack();
                FragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, f.Fragment).Commit();
                var title = FindViewById<TextView>(Resource.Id.titlebar_title);
                title.Text = f.Title;

            }
        }
    }
}

