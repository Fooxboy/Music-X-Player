
using Fooxboy.MusicX.Core.VKontakte.Music;
using Fooxboy.MusicX.Uwp.Converters;
using Fooxboy.MusicX.Uwp.Models;
using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.UI.Xaml;

namespace Fooxboy.MusicX.Uwp.Services
{
    public class PlayerService
    {
        private Album _currentAlbum;
        private List<Track> _tracks;
        private Track _currentTrack;
        private readonly int _repeatMode;
        private DispatcherTimer _positionTimer;
        private MediaPlayer _mediaPlayer;
        private Action<Task<List<Track>>> _loadMore;

        public event EventHandler PlayStateChangedEvent;
        public event EventHandler<TimeSpan> PositionTrackChangedEvent;
        public event EventHandler TrackChangedEvent;



        public PlayerService()
        {
            _mediaPlayer = new MediaPlayer();
            _tracks = new List<Track>();
            _repeatMode = 1;
            _mediaPlayer.AudioCategory = MediaPlayerAudioCategory.Media;


            _mediaPlayer.PlaybackSession.PlaybackStateChanged += MediaPlayerOnCurrentStateChanged;
            _mediaPlayer.MediaEnded += MediaPlayerOnMediaEnded;
            _mediaPlayer.MediaFailed += MediaPlayerOnMediaFailed;

            _mediaPlayer.CommandManager.NextBehavior.EnablingRule = MediaCommandEnablingRule.Always;
            _mediaPlayer.CommandManager.PreviousBehavior.EnablingRule = MediaCommandEnablingRule.Always;

            _mediaPlayer.CommandManager.NextReceived += (c, e) => NextTrack() ;
            _mediaPlayer.CommandManager.PreviousReceived += (c, e)=> PreviousTrack();
            _mediaPlayer.CommandManager.PlayReceived += (c, e)=> Play();
            _mediaPlayer.CommandManager.PauseReceived += (c, e)=> Pause();


            _positionTimer = new DispatcherTimer();
            _positionTimer.Interval = TimeSpan.FromMilliseconds(500);
            _positionTimer.Tick += PositionTimerOnTick;

        }

        public bool IsPlaying => _mediaPlayer.PlaybackSession.PlaybackState == MediaPlaybackState.Playing
            || _mediaPlayer.PlaybackSession.PlaybackState == MediaPlaybackState.Opening
            || _mediaPlayer.PlaybackSession.PlaybackState == MediaPlaybackState.Buffering;

        public Album CurrentAlbum => _currentAlbum;
        public List<Track> Tracks => _tracks;
        public Track CurrentTrack => _currentTrack;
        public TimeSpan Position => _mediaPlayer.PlaybackSession.Position;
        public TimeSpan Duration => _currentTrack?.Duration ?? TimeSpan.Zero;
        public int RepeatMode => _repeatMode;

        public double Volume
        {
            get => _mediaPlayer.Volume;
            set
            {
                if (_mediaPlayer.Volume == value)
                    return;

                _mediaPlayer.Volume = value;
            }
        }

        public void Play()
        {
            try
            {
                if (_currentTrack is null) return;

                if (_mediaPlayer.PlaybackSession.PlaybackState == MediaPlaybackState.None || _mediaPlayer.Source == null)
                {
                    _mediaPlayer.Source = MediaSource.CreateFromUri(_currentTrack.Url);
                    _mediaPlayer.Play();
                    TrackChangedEvent?.Invoke(this, EventArgs.Empty);

                } else _mediaPlayer.Play();
            }
            catch (Exception e)
            {
               //Выводим ошибку.
            }
        }

        public void Play(Album album, int index)
        {
            Play(album, index, album.Tracks.ToListTrack());
        }

        public void Play(Album album, int index, List<Track> tracks)
        {
            Pause();
            _currentAlbum = album;
            _tracks = tracks;
            _currentTrack = _tracks[index];

            Seek(TimeSpan.Zero);
            _mediaPlayer.Source = null;
            Play();
        }

        public void Play(Album album, Track track)
        {
            var index = album.Tracks.ToListTrack().IndexOf(track);
            Play(album, index);
        }

        public void Play(Album album, Track track, List<Track> tracks)
        {
            var index = tracks.IndexOf(track);
            Play(album, index, tracks);
        }

        public void Play(Track track)
        {
            Pause();

            _currentAlbum = null;
            _tracks.Clear();
            _tracks.Add(track);
            _currentTrack = track;

            Seek(TimeSpan.Zero);
            Play();

        }


        public void Pause()
        {
            _mediaPlayer.Pause();
        }

        public void NextTrack()
        {
            Pause();
            _mediaPlayer.Source = null;
            var index = _tracks.IndexOf(_currentTrack) + 1;
            if(index > _tracks.Count - 1)
            {
                if (_repeatMode == 2) index = 0;
                else return;
            }
            Seek(TimeSpan.Zero);
            _currentTrack = _tracks[index];
            TrackChangedEvent?.Invoke(this, EventArgs.Empty);
            Play();
        }

        public void PreviousTrack()
        {
            if (Position.TotalSeconds < 4) Seek(TimeSpan.Zero);
            else
            {
                Pause();
                _mediaPlayer.Source = null;
                var index = _tracks.IndexOf(_currentTrack) - 1;
                if (index < 0) index = _tracks.Count - 1;
                Seek(TimeSpan.Zero);
                _currentTrack = _tracks[index];
                TrackChangedEvent?.Invoke(this, EventArgs.Empty);
                Play();
            }
        }

        public void SetLoadMore(Action<Task<List<Track>>> action)
        {
            _loadMore = action;
        }
        public void Seek(TimeSpan position)
        {
            try
            {
                _mediaPlayer.PlaybackSession.Position = position;
            }
            catch (Exception e)
            {
               //ошибка при перемотке
            }
        }

        private void MediaPlayerOnCurrentStateChanged(MediaPlaybackSession sender, object args)
        {

            DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                if (sender.PlaybackState == MediaPlaybackState.Playing)
                    _positionTimer.Start();
                else
                    _positionTimer.Stop();

                PlayStateChangedEvent?.Invoke(this, EventArgs.Empty);
            });
        }

        private void MediaPlayerOnMediaEnded(MediaPlayer sender, object args)
        {
            NextTrack();
        }


        private void MediaPlayerOnMediaFailed(MediaPlayer sender, MediaPlayerFailedEventArgs args)
        {

            //Ошибка при загрузке

            //if (args.Error == MediaPlayerError.SourceNotSupported)
            //{
            //    //audio source url may expire
            //    CurrentPlaylist.CurrentItem.Source = null;
            //    TryResolveTrack(CurrentPlaylist.CurrentItem);
            //    return;
            //}
            //else if (args.Error == MediaPlayerError.NetworkError)
            //{
            //    InternetService.GoToOfflineMode();
            //}

            //DispatcherHelper.CheckBeginInvokeOnUI(async () =>
            //{
            //    await ContentDialogService.Show(new ExceptionDialog("Невозможно загрузить аудио файл", $"Невозможно загрузить файл по этой причине: {args.Error.ToString()}", new Exception(args.ErrorMessage)));

            //});
            //Log.Error("Media failed. " + args.Error + " " + args.ErrorMessage);
        }

        private void PositionTimerOnTick(object sender, object o)
        {
            DispatcherHelper.ExecuteOnUIThreadAsync((() =>
            {
                PositionTrackChangedEvent?.Invoke(this, Position);
            }));
        }

    }
}
