
using DryIoc;
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
using Fooxboy.MusicX.Core;
using Windows.Media.Streaming.Adaptive;
using Windows.Storage.Streams;

namespace Fooxboy.MusicX.Uwp.Services
{
    public class PlayerService
    {
        private List<Track> _tracks;
        private Track _currentTrack;
        private int _repeatMode; //0- без повтора, 1 - повтор трека,  2 - повтор альбома
        private DispatcherTimer _positionTimer;
        private MediaPlayer _mediaPlayer;
        private Action<Task<List<Track>>, bool> _loadMore;
        private bool _isShuffle;

        public event EventHandler PlayStateChangedEvent;
        public event EventHandler<TimeSpan> PositionTrackChangedEvent;
        public event EventHandler TrackChangedEvent;

        private NotificationService _notificationService;
        private readonly ConfigService _configService;

        public PlayerService(NotificationService notificationService, ConfigService configService)
        {
            _notificationService = notificationService;
            _mediaPlayer = new MediaPlayer();
            _tracks = new List<Track>();
            _repeatMode = 0;
            _mediaPlayer.AudioCategory = MediaPlayerAudioCategory.Media;

            _mediaPlayer.PlaybackSession.PlaybackStateChanged += MediaPlayerOnCurrentStateChanged;
            _mediaPlayer.MediaEnded += MediaPlayerOnMediaEnded;
            _mediaPlayer.MediaFailed += MediaPlayerOnMediaFailed;

            _mediaPlayer.CommandManager.NextBehavior.EnablingRule = MediaCommandEnablingRule.Always;
            _mediaPlayer.CommandManager.PreviousBehavior.EnablingRule = MediaCommandEnablingRule.Always;

            _mediaPlayer.CommandManager.NextReceived += async(c, e) => await NextTrack() ;
            _mediaPlayer.CommandManager.PreviousReceived += async (c, e)=> await PreviousTrack();
            _mediaPlayer.CommandManager.PlayReceived += (c, e)=> Play();
            _mediaPlayer.CommandManager.PauseReceived += (c, e)=> Pause();
            

            _positionTimer = new DispatcherTimer();
            _positionTimer.Interval = TimeSpan.FromMilliseconds(500);
            _positionTimer.Tick += PositionTimerOnTick;

            _configService = configService;

        }



        public async Task Play(int index, List<Track> tracks= null)
        {
            try
            {
 
                _mediaPlayer.PlaybackSession.Position = TimeSpan.Zero;

                _mediaPlayer.Pause();
                if (tracks != null) _tracks = tracks;

                _currentTrack = _tracks[index];

                if (!_currentTrack.IsAvailable)
                {
                    _notificationService.CreateNotification("Аудиозапись недоступна", "Она была изъята из публичного доступа");
                    await NextTrack();
                    return;
                }

                TrackChangedEvent?.Invoke(this, EventArgs.Empty);

                AdaptiveMediaSourceCreationResult result = await AdaptiveMediaSource.CreateFromUriAsync(_currentTrack.Url);
                var ams = result.MediaSource;

                _mediaPlayer.Source = MediaSource.CreateFromAdaptiveMediaSource(ams);

                _mediaPlayer.SystemMediaTransportControls.DisplayUpdater.MusicProperties.Title = _currentTrack.Title;
                _mediaPlayer.SystemMediaTransportControls.DisplayUpdater.MusicProperties.Artist = _currentTrack.Artist;
                _mediaPlayer.SystemMediaTransportControls.DisplayUpdater.MusicProperties.TrackNumber = (uint)index;
                if(_currentTrack.Album != null)_mediaPlayer.SystemMediaTransportControls.DisplayUpdater.Thumbnail = RandomAccessStreamReference.CreateFromUri(new Uri(_currentTrack.Album.Cover));
                _mediaPlayer.SystemMediaTransportControls.DisplayUpdater.AppMediaId = "Music X";

                _mediaPlayer.SystemMediaTransportControls.DisplayUpdater.Update();
                _mediaPlayer.Play();

                PositionTrackChangedEvent?.Invoke(this, TimeSpan.Zero);

                /* var config  = await _configService.GetConfig();

                config.NowPlayTrack = new NowPlayTrack()
                {
                    Track = CurrentTrack,
                    Album = (Album)CurrentTrack.Album,
                   
                    Index = index,
                    Second = 0
                };

                if (CurrentTrack.Artists.Count > 0) config.NowPlayTrack.Artist = (Core.Models.Artist)CurrentTrack.Artists[0];
               

                await _configService.SetConfig(config);*/

            }
            catch (Exception ex)
            {
                _notificationService.CreateNotification("Невозможно воспроизвести", $"{ex}");
            }
           
        }

        public async Task TryPlay()
        {
            try
            {
                _mediaPlayer.PlaybackSession.Position = TimeSpan.Zero;
                _mediaPlayer.Pause();

                AdaptiveMediaSourceCreationResult result = await AdaptiveMediaSource.CreateFromUriAsync(_currentTrack.Url);
                var ams = result.MediaSource;
                _mediaPlayer.Source = MediaSource.CreateFromAdaptiveMediaSource(ams);
                _mediaPlayer.Play();
                PositionTrackChangedEvent?.Invoke(this, TimeSpan.Zero);
            }catch (Exception ex)
            {
                _notificationService.CreateNotification("Невозможно воспроизвести", $"{ex}");
                await NextTrack();
            }
           
        }

        public async Task NextTrack()
        {
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
            await Play(index, null);
        }


        public void Play()
        {
            if(_currentTrack is null) return;   
            _mediaPlayer.Play();
        }

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

        public List<Track> Tracks => _tracks;
        public Track CurrentTrack => _currentTrack;
        public TimeSpan Position => _mediaPlayer.PlaybackSession.Position;
        public TimeSpan Duration => _currentTrack?.Duration ?? TimeSpan.Zero;
        public int RepeatMode => _repeatMode;
        public bool IsShuffle => _isShuffle;

        public bool IsPlaying => _mediaPlayer.PlaybackSession.PlaybackState == MediaPlaybackState.Playing
             || _mediaPlayer.PlaybackSession.PlaybackState == MediaPlaybackState.Opening
             || _mediaPlayer.PlaybackSession.PlaybackState == MediaPlaybackState.Buffering;

        public void SetShuffle(bool value)
        {
            _isShuffle = value;
        }

        public void SetTracks(List<Track> tracks)
        {
            _tracks = tracks;
        }

        public async void Pause()
        {
            _mediaPlayer.Pause();

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

        public void SetRepeatMode(int i)
        {
            _repeatMode = i;
        }

        public async Task PreviousTrack()
        {
            try
            {
                if (Position.TotalSeconds < 5) Seek(TimeSpan.Zero);
                else
                {

                    var index = _tracks.IndexOf(_currentTrack) - 1;
                    if (index < 0) index = _tracks.Count - 1;

                    TrackChangedEvent?.Invoke(this, EventArgs.Empty);
                    await Play(index);
                }
            }catch(Exception e)
            {
                _notificationService.CreateNotification("Произошла ошибка", e.ToString());

            }

        }



        private void MediaPlayerOnCurrentStateChanged(MediaPlaybackSession sender, object args)
        {
            try
            {
                DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                {
                    if (sender.PlaybackState == MediaPlaybackState.Playing)
                        _positionTimer.Start();
                    else
                        _positionTimer.Stop();

                    PlayStateChangedEvent?.Invoke(this, EventArgs.Empty);
                });
            }catch (Exception e)
            {
                _notificationService.CreateNotification("Произошла ошибка", e.ToString());
            }
            
        }

        private async void MediaPlayerOnMediaEnded(MediaPlayer sender, object args)
        {
            try
            {
                if (_repeatMode != 1) await NextTrack();
                else
                {
                    Seek(TimeSpan.Zero);
                    PositionTrackChangedEvent?.Invoke(this, TimeSpan.Zero);
                    Play();
                }
            }catch (Exception e)
            {
                _notificationService.CreateNotification("Произошла ошибка", e.ToString());

            }
        }



        private async void MediaPlayerOnMediaFailed(MediaPlayer sender, MediaPlayerFailedEventArgs args)
        {



            //if (args.Error == MediaPlayerError.SourceNotSupported)
            //{

            //}

            //Ошибка при загрузке

            if (args.Error == MediaPlayerError.SourceNotSupported)
            {
                //audio source url may expire
                await TryPlay();
                return;
            }
            else if (args.Error == MediaPlayerError.NetworkError)
            {
                InternetService.GoToOfflineMode();
            }

            _notificationService.CreateNotification("Произошла ошибка при загрузке", $"{args.Error}");


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
