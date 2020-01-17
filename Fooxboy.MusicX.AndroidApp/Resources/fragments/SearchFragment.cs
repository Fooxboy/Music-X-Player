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
using Fooxboy.MusicX.AndroidApp.Converters;
using Fooxboy.MusicX.Core.Interfaces;
using Java.Lang;

namespace Fooxboy.MusicX.AndroidApp.Resources.fragments
{
    public class SearchFragment : Fragment
    {

        TrackAdapter adapter;
        List<Track> tracksInResult = new List<Track>();
        RecyclerView resultsList;
        ProgressBar progress;
        RelativeLayout placeholderLayout;
        EditText edittext;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.searchActivity, container, false);
            edittext = view.FindViewById<EditText>(Resource.Id.searchEditText);
            resultsList = view.FindViewById<RecyclerView>(Resource.Id.searchResults);
            placeholderLayout = view.FindViewById<RelativeLayout>(Resource.Id.searchPlaceholder);
            progress = view.FindViewById<ProgressBar>(Resource.Id.searchProgress);
            this.adapter = new TrackAdapter(new List<Track>());
            resultsList.Clickable = true;
            RegisterForContextMenu(resultsList);
            resultsList.SetLayoutManager(new LinearLayoutManager(Application.Context, LinearLayoutManager.Vertical, false));
            Handler handler = new Handler(Looper.MainLooper);
            edittext.KeyPress += (sender, e) =>
            {
                e.Handled = false;
                if (e.Event.Action == KeyEventActions.Down && e.KeyCode == Keycode.Enter && edittext.Text != "")
                {

                    placeholderLayout.Visibility = ViewStates.Gone;
                    //var results = Core.VKontakte.Music.Search.TracksSync(edittext.Text);
                    progress.Visibility = ViewStates.Visible;
                    Task.Run(() => RefreshResults(new Handler(Looper.MainLooper)));
                    e.Handled = true;
                }
                /*if(e.Event.Action == KeyEventActions.Down && e.KeyCode == Keycode.Enter)
                {
                    
                    placeholderLayout.Visibility = ViewStates.Gone;
                    //var results = Core.VKontakte.Music.Search.TracksSync(edittext.Text);
                    progress.Visibility = ViewStates.Visible;
                    adapter = new TrackAdapter(new List<Models.AudioFile>(), "false");
                    adapter.ItemClick += AdapterOnItemClick;
                    var results = Core.VKontakte.Music.Search.TracksSync(edittext.Text);
                    this.tracksInResult = Services.MusicService.ConvertToAudioFile(results);
                    adapter.AddItems(this.tracksInResult);
                    resultsList.RemoveAllViewsInLayout();
                    resultsList.RemoveAllViews();
                    resultsList.SwapAdapter(adapter, true);
                    progress.Visibility = ViewStates.Gone;
                    e.Handled = true;

                }*/
            };


            return view;
        }

        private async void RefreshResults(Handler handler)
        {
            //List<ITrack> results = new List<ITrack>();
            var results = Core.Api.GetApi().VKontakte.Music.Search.Tracks(edittext.Text);
            //var results = Core.VKontakte.Music.Search.TracksSync(edittext.Text);
            tracksInResult = results.ToTracksList();
            adapter = new TrackAdapter(tracksInResult, new Block()); //TODO: тут блок был не налл но фолс
            adapter.ItemClick += AdapterOnItemClick;
            handler.Post(new Runnable(() =>
            {
                resultsList.RemoveAllViewsInLayout();
                resultsList.RemoveAllViews();
                resultsList.SwapAdapter(adapter, true);
                progress.Visibility = ViewStates.Gone;
            }));
        }

        private void AdapterOnItemClick(object sender, Track args, Block block = null)
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
                playlist.Tracks = tracksInResult;
                var player = PlayerService.Instanse;
                player.Play(playlist, playlist.Tracks.First(t => t.Url == args.Url));
            }
            catch (System.Exception e)
            {
                Toast.MakeText(Application.Context, $"Произошла ошибка: {e.ToString()}", ToastLength.Long).Show();
            }
        }

        public override bool OnContextItemSelected(IMenuItem i)
        {
            var t = tracksInResult[adapter.GetPosition()];
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
                    Toast.MakeText(Application.Context, "Удаляем...", ToastLength.Long).Show();
                    break;
            }
            return true;
        }

    }
}