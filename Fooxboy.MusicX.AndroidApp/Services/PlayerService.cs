using Android.App;
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
        public AudioFile CurrentAudioFile { get; set; } 


        public static PlayerService Instanse => inst ?? (inst = new PlayerService());


        public void Play(PlaylistFile playlist = null, AudioFile audio= null)
        {
            try
            {
                StaticContentService.NowPlay = playlist?.TracksFiles;
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

        private void MainServiceOnCurrentAudioChanged(object sender, AudioFile args)
        {
            
            Title = args.Title;
            Artist = args.Artist;
            Cover = args.Cover;
            CurrentAudioFile = args;

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
                if (Cover != "placeholder") cover.SetImageString(Cover, 50, 50);
                if (Cover == "placeholder") cover.SetImageResource(Resource.Drawable.placeholder);
            }
        }
    }
}