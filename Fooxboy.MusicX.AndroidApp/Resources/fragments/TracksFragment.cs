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
using Android.Views;
using Android.Widget;
using Fooxboy.MusicX.AndroidApp.Adapters;
using Fooxboy.MusicX.AndroidApp.Converters;
using Fooxboy.MusicX.AndroidApp.Models;
using Fooxboy.MusicX.AndroidApp.Services;
using Java.Lang;
using Exception = System.Exception;

namespace Fooxboy.MusicX.AndroidApp.Resources.fragments
{
    public class TracksFragment : Fragment
    {

        TrackAdapter adapter = null;
        PlaylistAdapter pAdapter = null;
        bool HasLoading = true;

        public List<Track> Tracks;
        public List<Album> Playlists;
        bool plistsHasLoading = true;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public List<Track> TracksInLibrary= new List<Track>();
        public List<Album> PlaylistsInLibrary = new List<Album>();


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            var view = inflater.Inflate(Resource.Layout.activity_tracks, container, false);

            List<Track> tracks = new List<Track>();
            Tracks = tracks;
            adapter = new TrackAdapter(tracks);

            List<Album> plists = new List<Album>();
            Playlists = plists;
            pAdapter = new PlaylistAdapter(plists);

            var tracksView = view.FindViewById<RecyclerView>(Resource.Id.list_tracks);
            var progressBar = view.FindViewById<ProgressBar>(Resource.Id.progressBar_tracks);

            var playlistsView = view.FindViewById<RecyclerView>(Resource.Id.list_plists);
            var playlistsProgressBar = view.FindViewById<ProgressBar>(Resource.Id.progressBar_library_plists);

            Handler handler = new Handler(Looper.MainLooper);

            adapter.ItemClick += TrackAdapterOnItemClick;
            pAdapter.ItemClick += PlaylistAdapterOnItemClick;

            tracksView.SetAdapter(adapter);
            tracksView.SetLayoutManager(new LinearLayoutManager(Application.Context, LinearLayoutManager.Vertical, false));
            RegisterForContextMenu(tracksView);
            tracksView.Clickable = true;

            playlistsView.SetAdapter(pAdapter);
            playlistsView.SetLayoutManager(new LinearLayoutManager(Application.Context, LinearLayoutManager.Horizontal, false));
            playlistsView.Clickable = true;

            var scrollListener = new Listeners.OnScrollToBottomListener(() =>
            {
                //if (!HasLoading) return;
                var task = Task.Run(() =>
                {
                    handler.Post(new Runnable(() =>
                    {
                        progressBar.Visibility = ViewStates.Visible;
                    }));
                    try
                    {
                        tracks = MusicService.GetMusicLibrary(15, adapter.ItemCount);
                        //var i = 1 + 1; //Без этого нихуя не работает.
                        //Fooxboy.MusicX.Core.Log.Debug(i.ToString());
                    }
                    catch (Exception e)
                    {
                        Toast.MakeText(Application.Context, $"Произошла ошибка: {e.ToString()}", ToastLength.Long).Show();

                    }

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


            var plistsScrollListener = new Listeners.OnScrollToBottomListener(() =>
            {
                if (!HasLoading) return;
                var task = Task.Run(() =>
                {
                    handler.Post(new Runnable(() =>
                    {
                        playlistsProgressBar.Visibility = ViewStates.Visible;
                    }));
                    try
                    {
                        plists = PlaylistsService.GetPlaylistLibrary();
                    }
                    catch (Exception e)
                    {
                        Toast.MakeText(Application.Context, $"Произошла ошибка: {e.ToString()}", ToastLength.Long).Show();

                    }

                });

                bool end = false;
                task.ContinueWith((t) =>
                {
                    while (plists.Count == 0)
                    {
                        System.Threading.Thread.Sleep(300);
                    }

                    HasLoading = !(plists.Count < 15);
                    handler.Post(new Runnable(() =>
                    {
                        var count = pAdapter.ItemCount;
                        pAdapter.AddItems(plists);
                        PlaylistsInLibrary.AddRange(plists);
                        pAdapter.NotifyItemRangeChanged(count, plists.Count);
                        playlistsProgressBar.Visibility = ViewStates.Invisible;
                        end = true;
                    }));

                });
                var a = task.ConfigureAwait(false);
                while (!end)
                {
                    System.Threading.Thread.Sleep(300);
                }
            });

            playlistsView.AddOnScrollListener(plistsScrollListener);

            if (pAdapter.ItemCount == 0) plistsScrollListener.InvokeCallback();
            if (adapter.ItemCount == 0) scrollListener.InvokeCallback();

            return view;

        }

        private void PlaylistAdapterOnItemClick(object sender, Album args, Block b = null)
        {
            var fragment = new PlaylistFragment();
            fragment.playlist = args;
            FragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, fragment).Commit();
        }

        private void TrackAdapterOnItemClick(object sender, Track args, Block b = null)
        {
            try
            {
                Toast.MakeText(Application.Context, $"Ты тыкнул: {args.Artist} - {args.Title} ", ToastLength.Long).Show();
                //Создание плейлиста из локальных трекаф
                var playlist = new Album();
                playlist.Artists.Add(new Artist()
                {
                    Name = "Music X"
                });
                playlist.Cover = "playlist_placeholder";
                playlist.Genres = null;
                playlist.Id = 1000;
                playlist.Tracks = TracksInLibrary;
                var player = PlayerService.Instanse;
                player.Play(playlist, playlist.Tracks.First(t => t.Url == args.Url));
            }
            catch (Exception e)
            {
                Toast.MakeText(Application.Context, $"Произошла ошибка: {e.ToString()}", ToastLength.Long).Show();
            }
        }

        public override bool OnContextItemSelected(IMenuItem i)
        {
            var t = Tracks[adapter.GetPosition()];
            switch (i.ItemId)
            {
                case 0:
                    Toast.MakeText(Application.Context, $"Переходим к исполнителю {t.Artist}", ToastLength.Long).Show();
                    var artist = new ArtistFragment();
                    if (Convert.ToInt32(t.Artists[0].Id) != 0)
                    {
                        artist.ArtistID = Convert.ToInt32(t.Artists[0].Id);
                        FragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, artist).Commit();
                    }
                    else
                    {
                        Toast.MakeText(Application.Context, "Ошибка: невозможно перейти к исполнителю.", ToastLength.Long).Show();
                    }

                    break;
                case 1:
                    Toast.MakeText(Application.Context, "Временно недоступно. ВК опять все сломали", ToastLength.Long).Show();
                break;
            }
            return true;
        }

    }
}