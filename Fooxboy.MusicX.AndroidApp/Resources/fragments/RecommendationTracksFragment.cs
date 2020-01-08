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

        public List<Track> tracks;
        TrackAdapter adapter;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.activity_tracks, container, false);
            adapter = new TrackAdapter(tracks, new Block()); //TODO: тут передавался Фолс а не налл ну тип да
            var tracksView = view.FindViewById<RecyclerView>(Resource.Id.list_tracks);
            var progressBar = view.FindViewById<ProgressBar>(Resource.Id.progressBar_tracks);
            progressBar.Visibility = ViewStates.Gone;
            adapter.ItemClick += AdapterOnItemClick;
            tracksView.SetAdapter(adapter);
            
            tracksView.SetLayoutManager(new LinearLayoutManager(Application.Context, LinearLayoutManager.Vertical, false));

            RegisterForContextMenu(tracksView);
            tracksView.Clickable = true;
            return view;
        }

        private void AdapterOnItemClick(object sender, Track args, Block block)
        {
            try
            {
                Toast.MakeText(Application.Context, $"Ты тыкнул: {args.Artist} - {args.Title} ", ToastLength.Long).Show();
                //Создание плейлиста из локальных трекаф
                var playlist = new Album();
                playlist.Artists[0] = new Artist()
                {
                    Name = "Music X"
                };
                playlist.Cover = "playlist_placeholder";
                playlist.Genres = null;
                playlist.Id = 1000;
                playlist.Tracks = tracks;
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
            var t = tracks[adapter.GetPosition()];
            switch (i.ItemId)
            {
                case 0:
                    Toast.MakeText(Application.Context, $"Переходим к исполнителю {t.Artist}", ToastLength.Long).Show();
                    var artist = new ArtistFragment();
                    if (t.Artists[0].Id != "0")
                    {
                        artist.ArtistID = Convert.ToInt64(t.Artists[0].Id);
                        FragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, artist).Commit();
                    }
                    else
                    {
                        Toast.MakeText(Application.Context, "Ошибка: невозможно перейти к исполнителю.", ToastLength.Long).Show();
                    }

                    break;
                case 1:
                    Toast.MakeText(Application.Context, "Удаляем...", ToastLength.Long).Show();
                    break;
            }
            return true;
        }
    }
}