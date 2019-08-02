using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using Fooxboy.MusicX.AndroidApp.Adapters;
using Fooxboy.MusicX.AndroidApp.Models;
using Fooxboy.MusicX.AndroidApp.Services;
using Java.Lang;

namespace Fooxboy.MusicX.AndroidApp.Resources.fragments
{
    public class PlaylistFragment : Fragment
    {

        TrackAdapter adapter = null;
        bool HasLoading = true;

        public List<AudioFile> Tracks;


        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.homeActivity, container, false);
            List<AudioFile> tracks = new List<AudioFile>();
            Tracks = tracks;
            adapter = new TrackAdapter(tracks);

            var tracksView = view.FindViewById<RecyclerView>(Resource.Id.tracksPlaylistView);
            Handler handler = new Handler(Looper.MainLooper);

            var progressBar = view.FindViewById<ProgressBar>(Resource.Id.progressBarLoagingTracks);
            var task = Task.Run(() =>
            {
                handler.Post(new Runnable(() =>
                {
                    progressBar.Visibility = ViewStates.Visible;
                }));
               // tracks = 
               //TODO: получение треков плейлиста.
                tracks = MusicService.GetMusicLibrary(15, adapter.ItemCount);
                var i = 1 + 1; //Без этого нихуя не работает.
                Fooxboy.MusicX.Core.Log.Debug(i.ToString());
            });


            task.ContinueWith((t) =>
            {
                while (tracks.Count == 0)
                {
                    System.Threading.Thread.Sleep(300);
                }

                HasLoading = !(tracks.Count < 15);
                handler.Post(new Runnable(() =>
                {
                    var count = adapter.ItemCount;
                    adapter.AddItems(tracks);
                    Tracks.AddRange(tracks);
                    adapter.NotifyItemRangeChanged(count, tracks.Count);
                    progressBar.Visibility = ViewStates.Invisible;
                }));

            });
            return view;


        }
    }
}