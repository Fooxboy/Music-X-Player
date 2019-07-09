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

namespace Fooxboy.MusicX.AndroidApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme")]
    public class MainActivity : AppCompatActivity, BottomNavigationView.IOnNavigationItemSelectedListener
    {
        Button textMessage;
        private Bundle sIS;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            textMessage = FindViewById<Button>(Resource.Id.message);
            BottomNavigationView navigation = FindViewById<BottomNavigationView>(Resource.Id.navigation);
            navigation.SetOnNavigationItemSelectedListener(this);
            /*textMessage.Click += (e, a) =>
            {
                Intent intent = new Intent(this, typeof(Resource.Layout.homeActivity));
                StartActivity(intent);
            };*/


            

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

