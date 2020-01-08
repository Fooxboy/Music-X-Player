using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using Fooxboy.MusicX.AndroidApp.Models;
using ImageViews.Rounded;
using System;

namespace Fooxboy.MusicX.AndroidApp.Services
{
    public class PlayerService
    {
        public PlayingService MainService;
        private static PlayerService inst;
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Cover { get; set; }
        public Track CurrentAudioFile { get; set; } 


        public static PlayerService Instanse => inst ?? (inst = new PlayerService());


        public void Play(Album playlist = null, Track audio= null)
        {
            try
            {
                StaticContentService.NowPlay = playlist?.Tracks;
                MainService.Play(playlist, audio);
            }catch(Exception e)
            {
                Toast.MakeText(Application.Context, $"Произошла ошибка: {e.ToString()}", ToastLength.Long).Show();
            }
        }

        public void Pause()
        {
            MainService.Pause();
        }

        private PlayerService()
        {
            MainService = new PlayingService();
            MainService.CurrentAudioChanged += MainServiceOnCurrentAudioChanged;
        }

        private void MainServiceOnCurrentAudioChanged(object sender, Track args)
        {
            
            Title = args.Title;
            Artist = args.Artist;
            Cover = args.Album?.Cover;
            CurrentAudioFile = args;
            var prefs = Application.Context.GetSharedPreferences("MusicX", FileCreationMode.Private);
            var streamToStatus = prefs.GetBoolean("StreamToStatus", false);

            if (MiniPlayerService.MiniPlayer != null)
            {
                var view = MiniPlayerService.MiniPlayer;
                var title = view.FindViewById<TextView>(Resource.Id.player_min_trackName);
                var artist = view.FindViewById<TextView>(Resource.Id.player_min_artist);
                var cover = view.FindViewById<RoundedImageView>(Resource.Id.player_min_cover);
                var playbtn = view.FindViewById<Button>(Resource.Id.miniPlayer_Playbtn);
                playbtn.SetBackgroundResource(Resource.Drawable.outline_pause_black_24dp);
                title.Text = Title;
                artist.Text = Artist;
                if (Cover != null) cover.SetImageString(Cover, 50, 50);
                else cover.SetImageResource(Resource.Drawable.placeholder);
            }
        }
    }
}