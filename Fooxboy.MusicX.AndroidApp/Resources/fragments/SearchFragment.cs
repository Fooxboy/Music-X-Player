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
using Fooxboy.MusicX.Core.Interfaces;

namespace Fooxboy.MusicX.AndroidApp.Resources.fragments
{
    public class SearchFragment : Fragment
    {

        TrackAdapter adapter;
        List<AudioFile> tracksInResult = new List<AudioFile>();

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.searchActivity, container, false);
            var edittext = view.FindViewById<EditText>(Resource.Id.searchEditText);
            var resultsList = view.FindViewById<RecyclerView>(Resource.Id.searchResults);
            var placeholderLayout = view.FindViewById<RelativeLayout>(Resource.Id.searchPlaceholder);
            var progress = view.FindViewById<ProgressBar>(Resource.Id.searchProgress);
            this.adapter = new TrackAdapter(new List<Models.AudioFile>());
            resultsList.Clickable = true;
            resultsList.SetLayoutManager(new LinearLayoutManager(Application.Context, LinearLayoutManager.Vertical, false));
            edittext.KeyPress += (sender, e) =>
            {
                e.Handled = false;
                if(e.Event.Action == KeyEventActions.Down && e.KeyCode == Keycode.Enter)
                {
                    
                    placeholderLayout.Visibility = ViewStates.Gone;
                    //var results = Core.VKontakte.Music.Search.TracksSync(edittext.Text);
                    progress.Visibility = ViewStates.Visible;
                    adapter = new TrackAdapter(new List<Models.AudioFile>());
                    adapter.ItemClick += AdapterOnItemClick;
                    var results = Core.VKontakte.Music.Search.TracksSync(edittext.Text);
                    this.tracksInResult = Services.MusicService.ConvertToAudioFile(results);
                    adapter.AddItems(this.tracksInResult);
                    resultsList.RemoveAllViewsInLayout();
                    resultsList.RemoveAllViews();
                    resultsList.SwapAdapter(adapter, true);
                    progress.Visibility = ViewStates.Gone;
                    e.Handled = true;

                }
            };


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
                playlist.TracksFiles = tracksInResult;
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