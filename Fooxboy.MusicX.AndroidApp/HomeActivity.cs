using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Fooxboy.MusicX.AndroidApp.Adapters;

namespace Fooxboy.MusicX.AndroidApp
{
    [Activity(Label = "Главная", Theme = "@style/AppTheme")]
    public class HomeActivity: Activity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.homeActivity);


            var tracksListView = FindViewById<ListView>(Resource.Id.tracks);

            var list = new List<Models.AudioFile>();

            list.Add(new Models.AudioFile()
                {
                    Title = "Название трека",
                    Artist = "Имя исполнителя"
                });
            


            tracksListView.Adapter = new TrackAdapter(Application.Context, list, tracksListView);
            

        }
    }
}