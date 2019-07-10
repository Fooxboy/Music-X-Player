using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Fooxboy.MusicX.AndroidApp.Adapters;
using Fooxboy.MusicX.AndroidApp.Services;

namespace Fooxboy.MusicX.AndroidApp
{
    [Activity(Label = "Домашний экран")]
    public class HomeActivity:Activity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.homeActivity);

            var tracksListView = FindViewById<ListView>(Resource.Id.tracks);

            var tracks = MusicService.GetMusicLibrary(20, 0);


            tracksListView.Adapter = new TrackAdapter(this, tracks, tracksListView);
        }

    }
}