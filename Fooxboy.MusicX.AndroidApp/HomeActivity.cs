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

            var i = 0;
            while(i < 50)
            {
                list.Add(new Models.AudioFile()
                {
                    Title = $"Название трека {i}",
                    Artist = $"Имя исполнителя {i}"
                });
                i++;
            }


            tracksListView.Adapter = new TrackAdapter(this, list, tracksListView);

            tracksListView.ItemClick += TracksListView_ItemClick;
        }

        private void TracksListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Toast.MakeText(this, "ТЫК НАХУЙ", ToastLength.Short);
        }
    }
}