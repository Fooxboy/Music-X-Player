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

namespace Fooxboy.MusicX.AndroidApp
{
    [Activity(Label = "Домашний экран")]
    public class HomeActivity:Activity
    {

        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            base.OnCreate(savedInstanceState, persistentState);

            SetContentView(Resource.Layout.homeActivity);

            var tracksListView = FindViewById<ListView>(Resource.Id.listView2);

            var list = new List<Models.AudioFile>();

            for (var i= 0; i<50; i++)
            {
                list.Add(new Models.AudioFile()
                {
                    Title = $"Название трека {i}",
                    Artist = $"Имя исполнителя {i}"
                });
            }


            tracksListView.Adapter = new TrackAdapter(this, list, tracksListView);
            
        }
    }
}