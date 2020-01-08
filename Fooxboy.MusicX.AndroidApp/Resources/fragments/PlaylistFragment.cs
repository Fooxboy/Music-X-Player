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
using Fooxboy.MusicX.AndroidApp.Converters;
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

        public List<Track> Tracks;
        public Album playlist;


        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.activity_playlist, container, false);
            //var tracks_in_playlist_i_know_im_really_bad_at_naming = Core.VKontakte.Music.Playlist.GetById(playlist.Id);
            Activity.FindViewById<TextView>(Resource.Id.titlebar_title).Text = "Плейлист";
            Handler handler = new Handler(Looper.MainLooper);
            var tracks = new List<Track>();
            adapter = new TrackAdapter(tracks, new Block()); //TODO: Здесь в блок передавался не налл вообще хз почему. ЭТОТ ТУДУ НА СЛУЧАЙ ПРОБЛЕМ

            var plists_recycler = view.FindViewById<RecyclerView>(Resource.Id.tracksPlaylistView);
            var trackscount = view.FindViewById<TextView>(Resource.Id.countTracksPlaylistView);
            var title = view.FindViewById<TextView>(Resource.Id.namePlaylistView);
            var genre = view.FindViewById<TextView>(Resource.Id.genrePlaylistView);
            var author = view.FindViewById<TextView>(Resource.Id.artistPlaylistView);
            var year = view.FindViewById<TextView>(Resource.Id.yearPlaylistView);
            var cover = view.FindViewById<RoundedImageView>(Resource.Id.coverPlaylistView);
            adapter.ItemClick += AdapterOnItemClick;

            if (playlist.Cover != "playlist_placeholder") cover.SetImageString(playlist.Cover, 50, 50);
            if (playlist.Cover == "playlist_placeholder") cover.SetImageResource(Resource.Drawable.playlist_placeholder);


            title.Text = playlist.Title;
            string genres = "";
            foreach (var g in playlist.Genres) {
                if (g != playlist.Genres.Last()) genres += $"{g}, ";
                else genres += g;
            }
                
            genre.Text = genres;
            string artists = "";
            foreach (var a in playlist.Artists) {
                if (a != playlist.Artists.Last()) genres += $"{a.Name}, ";
                else artists += a.Name;
            }
                
            author.Text = artists;
            year.Text = playlist.Year.ToString();

            plists_recycler.SetAdapter(adapter);
            plists_recycler.Clickable = true;
            plists_recycler.SetLayoutManager(new LinearLayoutManager(Application.Context, LinearLayoutManager.Vertical, false));

            var actualtracks = Task.Run(async() =>
            {
                return await Core.Api.GetApi().VKontakte.Music.Tracks.GetAsync(999, 0, playlist.AccessKey, playlist.Id, playlist.OwnerId);
                //return await Core.VKontakte.Music.Playlist.GetTracks;

            });
            try { 
                tracks = Converters.TracksConverter.ToTracksList(actualtracks.Result);
                this.playlist.Tracks = tracks;
                adapter.AddItems(tracks);
                adapter.ItemClick += AdapterOnItemClick;

                if (playlist.Cover != "playlist_placeholder") cover.SetImageString(playlist.Cover, 50, 50);
                if (playlist.Cover == "playlist_placeholder") cover.SetImageResource(Resource.Drawable.playlist_placeholder);

                trackscount.Text = $"{actualtracks.Result.Count} треков";
                title.Text = playlist.Title;
                genre.Text = genres;
                author.Text = artists;
                year.Text = playlist.Year.ToString();

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

        private void AdapterOnItemClick(object sender, Track args, Block block = null)
        {
            try
            {
                Album album;
                var player = PlayerService.Instanse;

                if (block is null)
                {
                    album = playlist;
                }else
                {
                    album = new Album();
                    album.Id = 0;
                    album.IsAvailable = true;
                    album.MainArtist = "Music X";
                    album.Title = block.Title;
                    album.Tracks = block.Tracks.ToTracksList();
                }

                player.Play(album, playlist.Tracks.First(t => t.Url == args.Url));

                Toast.MakeText(Application.Context, $"Ты тыкнул: {args.Artist} - {args.Title} ", ToastLength.Long).Show();
                //Создание плейлиста из локальных трекаф
            }
            catch (System.Exception e)
            {
                Toast.MakeText(Application.Context, $"Произошла ошибка: {e.ToString()}", ToastLength.Long).Show();
            }
        }

    }
}