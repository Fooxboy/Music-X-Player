using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Fooxboy.MusicX.AndroidApp.Adapters;
using Fooxboy.MusicX.AndroidApp.Models;
using Fooxboy.MusicX.AndroidApp.Services;

namespace Fooxboy.MusicX.AndroidApp.Resources.fragments
{
    class RecommendationTracksFragment : Fragment
    {

        public List<AudioFile> tracks;
        TrackAdapter adapter;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.activity_tracks, container, false);
            adapter = new TrackAdapter(tracks);
            var tracksView = view.FindViewById<RecyclerView>(Resource.Id.list_tracks);
            var progressBar = view.FindViewById<ProgressBar>(Resource.Id.progressBar_tracks);
            progressBar.Visibility = ViewStates.Gone;

            adapter.ItemClick += AdapterOnItemClick;

            tracksView.SetAdapter(adapter);
            tracksView.SetLayoutManager(new LinearLayoutManager(Application.Context, LinearLayoutManager.Vertical, false));

            tracksView.Clickable = true;
            return view;
        }

        private void AdapterOnItemClick(object sender, AudioFile args)
        {
            try
            {
                Toast.MakeText(Application.Context, $"Ты тыкнул: {args.Artist} - {args.Title} ", ToastLength.Long).Show();
                //Создание плейлиста из локальных трекаф
                var playlist = new PlaylistFile();
                playlist.Artist = "Music X";
                playlist.Cover = "playlist_placeholder";
                playlist.Genre = "";
                playlist.Id = 1000;
                playlist.IsAlbum = false;
                playlist.TracksFiles = tracks;
                var player = PlayerService.Instanse;
                player.Play(playlist, playlist.TracksFiles.First(t => t.SourceString == args.SourceString));
            }
            catch (Exception e)
            {
                Toast.MakeText(Application.Context, $"Произошла ошибка: {e.ToString()}", ToastLength.Long).Show();
            }
        }
    }
}