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
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
        }

        TrackAdapter adapter = null;
        bool HasLoading = true;

        public List<AudioFile> TracksInLibrary;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.homeActivity, container, false);

            var progressBar = view.FindViewById<ProgressBar>(Resource.Id.progressBarLoagingTracks);

            progressBar.Visibility = ViewStates.Visible;

            List<AudioFile> tracks = new List<AudioFile>();
            TracksInLibrary = tracks;
            adapter = new TrackAdapter(tracks);

            var tracksView = view.FindViewById<RecyclerView>(Resource.Id.TracksView);
            Handler handler = new Handler(Looper.MainLooper);
            
            adapter.ItemClick += AdapterOnItemClick;
            
            tracksView.SetAdapter(adapter);
            tracksView.SetLayoutManager(new LinearLayoutManager(Application.Context, LinearLayoutManager.Vertical, false));

            tracksView.Clickable = true;
            
            var scrollListener = new Listeners.OnScrollToBottomListener(() =>
            {
                if (!HasLoading) return;
                var task = Task.Run(() =>
                {
                    handler.Post(new Runnable(() =>
                    {
                        progressBar.Visibility = ViewStates.Visible;
                    }));
                    tracks = MusicService.GetMusicLibrary(15, adapter.ItemCount);
                    var i = 1 + 1; //Без этого нихуя не работает.
                    Fooxboy.MusicX.Core.Log.Debug(i.ToString());
                });

                bool end = false;
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
                        TracksInLibrary.AddRange(tracks);
                        adapter.NotifyItemRangeChanged(count, tracks.Count);
                        progressBar.Visibility = ViewStates.Invisible;
                        end = true;
                    }));

                });
                var a = task.ConfigureAwait(false);
                while (!end)
                {
                    System.Threading.Thread.Sleep(300);
                }
            });
            tracksView.AddOnScrollListener(scrollListener);

            if (adapter.ItemCount == 0) scrollListener.InvokeCallback();


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
                Fooxboy.MusicX.Core.Log.Debug(i.ToString());

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

        private void AdapterOnItemClick(object sender, AudioFile args)
        {
            Toast.MakeText(Application.Context, $"Ты тыкнул: {args.Artist} - {args.Title} ", ToastLength.Long).Show();
            //Создание плейлиста из локальных трекаф
            var playlist = new PlaylistFile();
            playlist.Artist = "Music X";
            playlist.Cover = "playlist_placeholder";
            playlist.Genre = "";
            playlist.Id = 1000;
            playlist.IsAlbum = false;
            playlist.TracksFiles = TracksInLibrary;
            var player = PlayerService.Instanse;
            player.Play(playlist, playlist.TracksFiles.First(t => t.SourceString == args.SourceString));
        
        }

        //Toast.MakeText(Application.Context, "Оно скролитЬся", ToastLength.Long).Show();
        //
        //if (view.LastVisiblePosition == view.Adapter.Count - 1 && view.GetChildAt(view.ChildCount - 1).Bottom <= view.Height)
        //{
        //    Toast.MakeText(Application.Context, "Мы на дне", ToastLength.Long).Show();
        //}

    }
}