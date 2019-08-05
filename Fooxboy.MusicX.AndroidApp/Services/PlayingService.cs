using Android.Media;
using Fooxboy.MusicX.AndroidApp.Delegates;
using System;
using System.Threading.Tasks;
using Android.App;
using Android.Support.V4.Media;
using Android.Support.V4.Media.App;
using Android.Support.V4.Media.Session;
using Com.Google.Android.Exoplayer2.UI;
using Fooxboy.MusicX.AndroidApp.Models;
using MediaManager;
using MediaManager.Media;
using MediaManager.Playback;

namespace Fooxboy.MusicX.AndroidApp.Services
{
    public class PlayingService
    {
        private readonly IMediaManager player;
        private AudioPlaylist currentPlaylist;
        private long currentPlaylistId = 0;
        private AudioFile currentTrack;
        public event Delegates.EventHandler<TimeSpan> PositionChanged;
        public event Delegates.EventHandler<AudioFile> CurrentAudioChanged;
        public event Delegates.EventHandler<Exception> ItemFailed; 

        public PlayingService()
        {
            player = CrossMediaManager.Current;
            player.PositionChanged += PlayerOnPositionChanged;
            player.MediaItemFailed += PlayerOnMediaItemFailed;
        }

        private void PlayerOnMediaItemFailed(object sender, MediaItemFailedEventArgs e)
        {
            var exception = e.Exeption;
            ItemFailed?.Invoke(this, exception);
        }

        private void PlayerOnPositionChanged(object sender, PositionChangedEventArgs e)
        {
            var position = e.Position;
            PositionChanged?.Invoke(this, position);
        }


        public bool IsPlay => player.IsPlaying();
        public TimeSpan Duration => player.Duration;
        
        public TimeSpan Position => player.Position;

        public void Play(PlaylistFile playlist = null, AudioFile audio = null)
        {
            if (playlist == null)
            {
                if (currentPlaylist == null && currentTrack == null) return;
                player.Play();
            }
            else
            {
                if (playlist.Id != currentPlaylistId)
                {
                    var audioPlaylist = new AudioPlaylist(playlist, audio, StaticContentService.RepeatPlaylist, StaticContentService.RepeatTrack);
                    currentPlaylist = audioPlaylist;
                    currentTrack = audio;
                    currentPlaylist.OnCurrentItemChanged += CurrentPlaylistOnOnCurrentItemChanged;
                    CurrentAudioChanged?.Invoke(this, audio);
                }
                else
                {
                    currentPlaylist.SetCurrentTrack(audio);
                    currentTrack = audio;
                    CurrentAudioChanged?.Invoke(this, audio);
                }
                //var playerNotificationManager = (CrossMediaManager.Android.NotificationManager as MediaManager.Platforms.Android.Notifications.NotificationManager).PlayerNotificationManager;
                

                TaskService.RunOnUI(async () =>
                {
                    var media = await player.Play(currentTrack.SourceString);
                    media.Title = currentTrack.Title;
                    media.AlbumArtUri = currentTrack.Cover;
                    media.Artist = currentTrack.Artist;
                    media.AlbumArtist = currentTrack.Artist;
                    media.ArtUri = currentTrack.Cover;
                    CrossMediaManager.Android.NotificationManager.UpdateNotification();
                });

                
                //player.MediaQueue.Current.Title = audio.Title;
                //player.MediaQueue.Current.Artist = audio.Title;
                //player.MediaQueue.Current.AlbumArtUri = audio.Cover;
                //CrossMediaManager.Android.
                //player.NotificationManager = null;
            }
        }

        private void CurrentPlaylistOnOnCurrentItemChanged(object sender, AudioFile args)
        {
            currentTrack = args;
            CurrentAudioChanged?.Invoke(this, args);
            player.Play(currentTrack.SourceString);
            CurrentAudioChanged?.Invoke(this, args);
            //throw new NotImplementedException();
        }


        public void Pause()
        {
            player.Pause();
        }

        public void SeekTo(TimeSpan time)
        {
            player.SeekTo(time);
        }

        public void SeekToStart()
        {
            player.SeekToStart();
        }

        public void NextTrack()
        {
            Pause();
            SeekToStart();
            
            currentPlaylist.Next(true);

        }

        public void BackTrack()
        {
            Pause();
            SeekToStart();
            currentPlaylist.Back();
        }
        
    }
}