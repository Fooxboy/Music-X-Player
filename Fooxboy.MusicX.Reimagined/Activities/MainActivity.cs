using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Fooxboy.MusicX.Reimagined.Services;
using Fooxboy.MusicX.Core;
using Android.Graphics.Drawables;
using Android.Content;
using Fooxboy.MusicX.Reimagined.Fragments;
using Fooxboy.MusicX.Core.Models;
using System.Collections.Generic;
using ImageViews.Rounded;

namespace Fooxboy.MusicX.Reimagined
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, BottomNavigationView.IOnNavigationItemSelectedListener
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            //--------set up base
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            StaticContentService.Recommendations = new List<Block>();

            //--------set up navig
            BottomNavigationView navigation = FindViewById<BottomNavigationView>(Resource.Id.navigation);
            navigation.SetOnNavigationItemSelectedListener(this);

            //--------login check
            if (AuthService.IsLoggedIn())
            {
                Api.GetApi().VKontakte.Auth.Auto(AuthService.GetToken(), null);
                var user = Api.GetApi().VKontakte.Users.Info.CurrentUser();
                StaticContentService.UserId = user.Id;
                StaticContentService.UserName = user.FirstName;
                FindViewById<RoundedImageView>(Resource.Id.profilepicture_main).SetImageString(ImagesService.PhotoUser(user.PhotoUser), 30, 30);


                var navBackButton = FindViewById<ImageView>(Resource.Id.backbuttonMain);
                navBackButton.Visibility = ViewStates.Gone;
                var Title = FindViewById<TextView>(Resource.Id.titlebar_title);
                Title.Text = "Music X";
                var MF = new MainFragment();
                SupportFragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, MF).Commit();
            }
            else
            {

                AuthService.InitiateLogin();
            }

        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            var title = FindViewById<TextView>(Resource.Id.titlebar_title);
            switch (item.ItemId)
            {
                case Resource.Id.navigation_home:
                    title.Text = "Рекомендации";
                    return true;
                case Resource.Id.navigation_tracks:
                    title.Text = "Ваша музыка";
                    return true;
                /*case Resource.Id.navigation_favourite:
                    title.Text = "Избранное";
                    return true;
                case Resource.Id.navigation_search:
                    title.Text = "Поиск";
                    return true;*/
                case Resource.Id.navigation_downloads:
                    title.Text = "Загрузки";
                    return true;
            }
            return false;
        }
    }
}

