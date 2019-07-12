using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    public class HomeFragment : Fragment
    {

        private int preLast;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
        }

        TrackAdapter adapter = null;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.homeActivity, container, false);

            var progressBar = view.FindViewById<ProgressBar>(Resource.Id.progressBarLoagingTracks);

            progressBar.Visibility = ViewStates.Visible;

            List<AudioFile> tracks = new List<AudioFile>();
            adapter = new TrackAdapter(tracks);

            var tracksView = view.FindViewById<RecyclerView>(Resource.Id.TracksView);
            Handler handler = new Handler(Looper.MainLooper);

            tracksView.SetAdapter(adapter);
            tracksView.SetLayoutManager(new LinearLayoutManager(Application.Context, LinearLayoutManager.Vertical, false));
            tracksView.AddOnScrollListener(new Listeners.OnScrollToBottomListener());

            //Tracks;
            var task = Task.Run(() =>
            {
                tracks = MusicService.GetMusicLibrary(20, 0);
                var i = 1 + 1;
            });


            task.ContinueWith((t) =>
            {
                while (tracks.Count == 0)
                {
                    System.Threading.Thread.Sleep(500);
                }

                handler.Post(new Runnable(() =>
                {
                    adapter.AddItems(tracks);
                    adapter.NotifyDataSetChanged();
                    progressBar.Visibility = ViewStates.Invisible;
                }));
                
            });

            var a = task.ConfigureAwait(false);

            /* плейлисты ебац */

            var playlistlistview = view.FindViewById<RecyclerView>(Resource.Id.playlists);
            var plist = new List<PlaylistFile>();
            var adapterPlaylists = new PlaylistAdapter(plist);
            playlistlistview.SetLayoutManager(new LinearLayoutManager(Application.Context, LinearLayoutManager.Horizontal, false));
            playlistlistview.SetAdapter(adapterPlaylists);


            var task2 = Task.Run(() =>
            {
                plist = PlaylistsService.GetPlaylistLibrary();
                var i = 1 + 1; //Без этого говна почему-то не работает, лол

            });

            task2.ContinueWith((t) =>
            {
                while (plist.Count == 0)
                {
                    System.Threading.Thread.Sleep(500);
                }

                handler.Post(new Runnable(() =>
                {
                    adapterPlaylists.AddItems(plist);
                    adapterPlaylists.NotifyDataSetChanged();
                    //progressBar.Visibility = ViewStates.Invisible;
                }));
            });

            task2.ConfigureAwait(false);

            return view;

        }

    

        //Toast.MakeText(Application.Context, "Оно скролитЬся", ToastLength.Long).Show();
        //
        //if (view.LastVisiblePosition == view.Adapter.Count - 1 && view.GetChildAt(view.ChildCount - 1).Bottom <= view.Height)
        //{
        //    Toast.MakeText(Application.Context, "Мы на дне", ToastLength.Long).Show();
        //}

    }
}