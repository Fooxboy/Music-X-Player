﻿using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.UI.Xaml;
using VkNet.Model.Attachments;

namespace Fooxboy.MusicX.Uwp.Services
{
    public class PlayerService
    {
        private AudioPlaylist _currentAlbum;
        private List<Audio> _tracks;
        private Audio _currentTrack;
        private int _repeatMode; //0- без повтора, 1 - повтор трека,  2 - повтор альбома
        private DispatcherTimer _positionTimer;
        private MediaPlayer _mediaPlayer;
        private Action<Task<IEnumerable<Audio>>, bool> _loadMore;
        private bool _isShuffle;

        public event EventHandler PlayStateChangedEvent;
        public event EventHandler<TimeSpan> PositionTrackChangedEvent;
        public event EventHandler TrackChangedEvent;

        private NotificationService _notificationService;


        public PlayerService(NotificationService notificationService)
        {
            _notificationService = notificationService;
            _mediaPlayer = new MediaPlayer();
            _tracks = new List<Audio>();
            _repeatMode = 1;
            _mediaPlayer.AudioCategory = MediaPlayerAudioCategory.Media;


            _mediaPlayer.PlaybackSession.PlaybackStateChanged += MediaPlayerOnCurrentStateChanged;
            _mediaPlayer.MediaEnded += MediaPlayerOnMediaEnded;
            _mediaPlayer.MediaFailed += MediaPlayerOnMediaFailed;

            _mediaPlayer.CommandManager.NextBehavior.EnablingRule = MediaCommandEnablingRule.Always;
            _mediaPlayer.CommandManager.PreviousBehavior.EnablingRule = MediaCommandEnablingRule.Always;

            _mediaPlayer.CommandManager.NextReceived += (c, e) => NextTrack();
            _mediaPlayer.CommandManager.PreviousReceived += (c, e) => PreviousTrack();
            _mediaPlayer.CommandManager.PlayReceived += (c, e) => Play();
            _mediaPlayer.CommandManager.PauseReceived += (c, e) => Pause();


            _positionTimer = new DispatcherTimer();
            _positionTimer.Interval = TimeSpan.FromMilliseconds(500);
            _positionTimer.Tick += PositionTimerOnTick;
        }


        public bool IsPlaying => _mediaPlayer.PlaybackSession.PlaybackState == MediaPlaybackState.Playing
                                 || _mediaPlayer.PlaybackSession.PlaybackState == MediaPlaybackState.Opening
                                 || _mediaPlayer.PlaybackSession.PlaybackState == MediaPlaybackState.Buffering;

        public AudioPlaylist CurrentAlbum => _currentAlbum;
        public List<Audio> Tracks => _tracks;
        public Audio CurrentTrack => _currentTrack;
        public TimeSpan Position => _mediaPlayer.PlaybackSession.Position;
        public int Duration => _currentTrack?.Duration ?? 0;
        public int RepeatMode => _repeatMode;
        public bool IsShuffle => _isShuffle;

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

        public void SetShuffle(bool value)
        {
            _isShuffle = value;
        }

        public void SetTracks(List<Audio> tracks)
        {
            _tracks = tracks;
        }

        public void Play()
        {
            try
            {
                if (_currentTrack is null) return;

                if (_currentTrack.ContentRestricted.HasValue)
                {
                    _notificationService.CreateNotification("Аудиозапись недоступна",
                        "Она была изъята из публичного доступа");
                    return;
                }

                if (_mediaPlayer.PlaybackSession.PlaybackState == MediaPlaybackState.None ||
                    _mediaPlayer.Source == null)
                {
                    _mediaPlayer.Source = MediaSource.CreateFromUri(_currentTrack.Url);
                    _mediaPlayer.Play();
                    TrackChangedEvent?.Invoke(this, EventArgs.Empty);
                }
                else _mediaPlayer.Play();
            }
            catch (Exception e)
            {
                _notificationService.CreateNotification("Произошла ошибка при воспроизведении", e.ToString());
            }
        }

        public void Play(AudioPlaylist album, int index)
        {
            Pause();
            _currentAlbum = album;
            _currentTrack = _tracks[index];

            Seek(TimeSpan.Zero);
            _mediaPlayer.Source = null;
            Play();
        }


        public void Play(Audio track)
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
            Seek(TimeSpan.Zero);
            PositionTrackChangedEvent?.Invoke(this, TimeSpan.Zero);
            _mediaPlayer.Source = null;
            int index;
            if (!_isShuffle)
            {
                index = _tracks.IndexOf(_currentTrack) + 1;
                if (index > _tracks.Count - 1)
                {
                    if (_repeatMode == 2) index = 0;
                    else return;
                }
            }
            else
            {
                index = new Random().Next(0, _tracks.Count);
            }

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
                PositionTrackChangedEvent?.Invoke(this, TimeSpan.Zero);
                _currentTrack = _tracks[index];
                TrackChangedEvent?.Invoke(this, EventArgs.Empty);
                Play();
            }
        }

        public void SetLoadMore(Action<Task<IEnumerable<Audio>>, bool> action)
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
                _notificationService.CreateNotification("Произошла ошибка при перемотке", e.ToString());
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
            if (_repeatMode != 1) NextTrack();
            else
            {
                Seek(TimeSpan.Zero);
                PositionTrackChangedEvent?.Invoke(this, TimeSpan.Zero);
                Play();
            }
        }

        public void SetRepeatMode(int i)
        {
            _repeatMode = i;
        }


        private void MediaPlayerOnMediaFailed(MediaPlayer sender, MediaPlayerFailedEventArgs args)
        {
            _notificationService.CreateNotification("Произошла ошибка при загрузке", $"{args.Error}");

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
            DispatcherHelper.ExecuteOnUIThreadAsync((() => { PositionTrackChangedEvent?.Invoke(this, Position); }));
        }
    }
}