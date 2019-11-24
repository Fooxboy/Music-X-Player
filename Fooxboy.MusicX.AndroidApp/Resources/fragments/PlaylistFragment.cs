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
using ImageViews.Rounded;
using Java.Lang;

namespace Fooxboy.MusicX.AndroidApp.Resources.fragments
{
    public class PlaylistFragment : Fragment
    {

        TrackAdapter adapter = null;
        bool HasLoading = true;

        public List<AudioFile> Tracks;
        public PlaylistFile playlist;


        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.activity_playlist, container, false);
            //var tracks_in_playlist_i_know_im_really_bad_at_naming = Core.VKontakte.Music.Playlist.GetById(playlist.Id);
            var actualtracks = Task.Run(() =>
            {

                return Core.VKontakte.Music.Playlist.GetTracks(playlist.Id, playlist.OwnerId, playlist.AccessKey).Result;

            });

            while (!actualtracks.IsCompleted) continue;
            try { 
                var tracks = MusicService.ConvertToAudioFile(actualtracks.Result);
                this.playlist.TracksFiles = tracks;
                adapter = new TrackAdapter(tracks);
                adapter.ItemClick += AdapterOnItemClick;
                var plists_recycler = view.FindViewById<RecyclerView>(Resource.Id.tracksPlaylistView);
                var trackscount = view.FindViewById<TextView>(Resource.Id.countTracksPlaylistView);
                var title = view.FindViewById<TextView>(Resource.Id.namePlaylistView);
                var genre = view.FindViewById<TextView>(Resource.Id.genrePlaylistView);
                var author = view.FindViewById<TextView>(Resource.Id.artistPlaylistView);
                var year = view.FindViewById<TextView>(Resource.Id.yearPlaylistView);
                var cover = view.FindViewById<RoundedImageView>(Resource.Id.coverPlaylistView);

                if (playlist.Cover != "playlist_placeholder") cover.SetImageString(playlist.Cover, 50, 50);
                if (playlist.Cover == "playlist_placeholder") cover.SetImageResource(Resource.Drawable.playlist_placeholder);

                trackscount.Text = $"{actualtracks.Result.Count} треков";
                title.Text = playlist.Name;
                genre.Text = playlist.Genre;
                author.Text = playlist.Artist;
                year.Text = playlist.Year;

                plists_recycler.SetAdapter(adapter);
                plists_recycler.Clickable = true;
                plists_recycler.SetLayoutManager(new LinearLayoutManager(Application.Context, LinearLayoutManager.Vertical, false));
            }
            catch(System.Exception e)
            {
                Toast.MakeText(Application.Context, $"Произошла ошибка. Видимо, ВКонтакте не дает нам доступ к этому плейлисту.", ToastLength.Long).Show();
                view = inflater.Inflate(Resource.Layout.errorActivity, container, false);
            }
            
            
            


            return view;


        }

        private void AdapterOnItemClick(object sender, AudioFile args)
        {
            try
            {
                Toast.MakeText(Application.Context, $"Ты тыкнул: {args.Artist} - {args.Title} ", ToastLength.Long).Show();
                //Создание плейлиста из локальных трекаф
                var player = PlayerService.Instanse;
                player.Play(playlist, playlist.TracksFiles.First(t => t.SourceString == args.SourceString));
            }
            catch (System.Exception e)
            {
                Toast.MakeText(Application.Context, $"Произошла ошибка: {e.ToString()}", ToastLength.Long).Show();
            }
        }

    }
}