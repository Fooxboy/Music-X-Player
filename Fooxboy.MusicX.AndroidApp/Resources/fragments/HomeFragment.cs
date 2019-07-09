using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Fooxboy.MusicX.AndroidApp.Adapters;

namespace Fooxboy.MusicX.AndroidApp.Resources.fragments
{
    public class HomeFragment : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.homeActivity, container, false);

            var tracksListView = view.FindViewById<ListView>(Resource.Id.tracks);

            var list = new List<Models.AudioFile>();
            for (int i = 0; i < 15; i++)
            {
                list.Add(new Models.AudioFile()
                {
                    Title = $"Название трека {i}",
                    Artist = $"Имя исполнителя  {i}"
                });
            }


            tracksListView.Adapter = new TrackAdapter(Application.Context, list, tracksListView);

            return view;
        }
    }
}