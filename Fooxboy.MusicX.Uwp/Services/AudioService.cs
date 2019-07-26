using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight.Threading;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Core;
using Fooxboy.MusicX.Uwp.Enums;
using Fooxboy.MusicX.Uwp.Interfaces;
using Fooxboy.MusicX.Uwp.Models;
using Fooxboy.MusicX.Uwp.Utils.Extensions;
using Windows.ApplicationModel;
using Windows.Media.Playback;
using Windows.UI.Xaml;
using Windows.Media;
using Windows.Storage.Streams;
using System.Threading;
using Windows.Media.Core;
using Windows.Storage;
using Fooxboy.MusicX.Uwp.Resources.ContentDialogs;
using System.IO;

namespace Fooxboy.MusicX.Uwp.Services
{
    public class AudioService
    {
        private AudioPlaylist currentPlaylist = new AudioPlaylist();
        DispatcherTimer positionTimer;
        MediaPlayer mediaPlayer = new MediaPlayer();

        public event EventHandler PlayStateChanged;
        public event EventHandler<TimeSpan> PositionChanged;
        public event EventHandler CurrentAudioChanged;

        public static AudioService Instance { get; } = new AudioService();

        /// <summary>
        /// Играет ли что-то сейчас
        /// </summary>
        public bool IsPlaying => mediaPlayer.PlaybackSession.PlaybackState == MediaPlaybackState.Playing
            || mediaPlayer.PlaybackSession.PlaybackState == MediaPlaybackState.Opening
            || mediaPlayer.PlaybackSession.PlaybackState == MediaPlaybackState.Buffering;

        /// <summary>
        /// Текущий плейлист
        /// </summary>
        public AudioPlaylist CurrentPlaylist => currentPlaylist;

        /// <summary>
        /// Текущая позиция
        /// </summary>
        public TimeSpan Position => mediaPlayer.PlaybackSession.Position;

        /// <summary>
        /// Продолжительность аудио
        /// </summary>
       public TimeSpan Duration => currentPlaylist.CurrentItem?.Duration ?? TimeSpan.Zero;

        /// <summary>
        /// Громкость
        /// </summary>
        public double Volume
        {
            get { return mediaPlayer.Volume; }
            set
            {
                if (mediaPlayer.Volume == value)
                    return;

                mediaPlayer.Volume = value;
            }
        }

        /// <summary>
        /// Repeat mode
        /// </summary>
        public RepeatMode Repeat
        {
            get { return currentPlaylist.Repeat; }
            set
            {
                currentPlaylist.Repeat = value;
            }
        }

        /// <summary>
        /// перемшать
        /// </summary>
        public bool Shuffle
        {
            get { return currentPlaylist.Shuffle; }
            set
            {
                currentPlaylist.Shuffle = value;
            }
        }

        private AudioService()
        {
            Application.Current.Resuming += AppResuming;
            Application.Current.Suspending += AppSuspending;
            Application.Current.EnteredBackground += Current_EnteredBackground;
            Application.Current.LeavingBackground += Current_LeavingBackground;
            mediaPlayer.AudioCategory = MediaPlayerAudioCategory.Media;
            Initialize();
        }

        private void Current_LeavingBackground(object sender, LeavingBackgroundEventArgs e)
        {
            Initialize();
        }

        private void Current_EnteredBackground(object sender, EnteredBackgroundEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Resume or start playing current track
        /// </summary>
        public void Play()
        {
            if (CurrentPlaylist.CurrentItem == null)
                return;

            if (mediaPlayer.PlaybackSession.PlaybackState == MediaPlaybackState.None || mediaPlayer.Source == null)
            {
                if (CurrentPlaylist.CurrentItem.IsLocal) PlayFrom(CurrentPlaylist.CurrentItem.Source);
                else PlayFrom(new Uri(CurrentPlaylist.CurrentItem.SourceString));

            }
                
            else
                mediaPlayer.Play();
        }

        /// <summary>
        /// Pause current track
        /// </summary>
        public void Pause()
        {
            mediaPlayer.Pause();
        }

        /// <summary>
        /// Stops playback
        /// </summary>
        public void Stop()
        {
            //will automatically pause and seek to 0
            CurrentPlaylist.CurrentItem = null;
            CurrentPlaylist.ClearAll();
        }

        /// <summary>
        /// Switch to next track
        /// </summary>
        public void SwitchNext(bool skip = false)
        {
            Seek(TimeSpan.Zero);
            Seek(TimeSpan.Zero);
            Seek(TimeSpan.Zero);
            Seek(TimeSpan.Zero);

            currentPlaylist.MoveNext(skip: skip);
        }

        /// <summary>
        /// Switch to previous track
        /// </summary>
        public void SwitchPrev()
        {
            
            if (Position > TimeSpan.FromSeconds(3))
                PlayAudio(currentPlaylist.CurrentItem, currentPlaylist.Items);
            else
            {
                Seek(TimeSpan.Zero);
                Seek(TimeSpan.Zero);
                Seek(TimeSpan.Zero);
                Seek(TimeSpan.Zero);
                Seek(TimeSpan.Zero);
                currentPlaylist.MovePrevious();
            }
                
        }

        /// <summary>
        /// Seek to position
        /// </summary>
        public void Seek(TimeSpan position)
        {

            mediaPlayer.PlaybackSession.Position = position;
        }

        /// <summary>
        /// Sets current playlist. Used to update UI on the first data load.
        /// </summary>
        public async void SetCurrentPlaylist(AudioPlaylist playlist, bool play = true)
        {
            try
            {
                Seek(TimeSpan.Zero);
                currentPlaylist = null;
                Pause();
                if (currentPlaylist != null)
                    currentPlaylist.OnCurrentItemChanged -= CurrentPlaylistOnCurrentItemChanged;

                currentPlaylist = playlist;

                if (currentPlaylist != null)
                {
                    currentPlaylist.Repeat = Repeat;
                    currentPlaylist.Shuffle = Shuffle;

                    currentPlaylist.OnCurrentItemChanged += CurrentPlaylistOnCurrentItemChanged;

                    CurrentAudioChanged?.Invoke(this, EventArgs.Empty);
                }
                UpdateTransportControl();

                if(play)
                {
                    if (playlist.CurrentItem.IsLocal) PlayFrom(playlist.CurrentItem.Source);
                    else PlayFrom(new Uri(playlist.CurrentItem.SourceString));

                }

            }
            catch(Exception e)
            {
                await ContentDialogService.Show(new ExceptionDialog("Ошибка при установке текущего плейлиста", "Возможно, плейлист поврежден", e));
            }
            

        }

        /// <summary>
        /// Play audio with playlist
        /// </summary>
        public async void PlayAudio(AudioFile audio, IList<AudioFile> sourcePlaylist)
        {
            try
            {
                //check if it's a new playlist
                if (!currentPlaylist.Items.AreSame(sourcePlaylist))
                {
                    currentPlaylist.OnCurrentItemChanged -= CurrentPlaylistOnCurrentItemChanged;

                    var shuffle = Shuffle;
                    var repeat = Repeat;

                    currentPlaylist = new AudioPlaylist(sourcePlaylist);
                    currentPlaylist.Repeat = repeat;
                    currentPlaylist.Shuffle = shuffle;

                    currentPlaylist.CurrentItem = audio;

                    currentPlaylist.OnCurrentItemChanged += CurrentPlaylistOnCurrentItemChanged;
                    CurrentPlaylistOnCurrentItemChanged(this, audio);
                }
                else
                {
                    if (currentPlaylist.CurrentItem == audio)
                    {
                        if (currentPlaylist.CurrentItem.IsLocal) PlayFrom(currentPlaylist.CurrentItem.Source);
                        else PlayFrom(new Uri(currentPlaylist.CurrentItem.SourceString));
                    }
                       
                    else
                        currentPlaylist.CurrentItem = audio;
                }
            }catch(Exception e)
            {
                await ContentDialogService.Show(new ExceptionDialog("Ошибка при воспроизведении трека", "Возможно трек поврежден или нет доступа к нему.", e));
            }
            
        }
        private void Initialize()
        {
            mediaPlayer.PlaybackSession.PlaybackStateChanged += MediaPlayerOnCurrentStateChanged;
            mediaPlayer.MediaEnded += MediaPlayerOnMediaEnded;
            mediaPlayer.MediaFailed += MediaPlayerOnMediaFailed; ;

            mediaPlayer.CommandManager.NextBehavior.EnablingRule = MediaCommandEnablingRule.Always;
            mediaPlayer.CommandManager.PreviousBehavior.EnablingRule = MediaCommandEnablingRule.Always;

            mediaPlayer.CommandManager.NextReceived += CommandManager_NextReceived;
            mediaPlayer.CommandManager.PreviousReceived += CommandManager_PreviousReceived;
            mediaPlayer.CommandManager.PlayReceived += CommandManager_PlayReceived;
            mediaPlayer.CommandManager.PauseReceived += CommandManager_PauseReceived;

            currentPlaylist.OnCurrentItemChanged += CurrentPlaylistOnCurrentItemChanged;

            positionTimer = new DispatcherTimer();
            positionTimer.Interval = TimeSpan.FromMilliseconds(500);
            positionTimer.Tick += PositionTimerOnTick;
            

            Volume = StaticContent.Volume;
            Repeat = StaticContent.Repeat;
            Shuffle = StaticContent.Shuffle;
        }

        private void Close()
        {
            mediaPlayer.PlaybackSession.PlaybackStateChanged -= MediaPlayerOnCurrentStateChanged;
            mediaPlayer.MediaEnded -= MediaPlayerOnMediaEnded;

            //mediaPlayer.CommandManager.NextReceived -= CommandManager_NextReceived;
            //mediaPlayer.CommandManager.PreviousReceived -= CommandManager_PreviousReceived;
            //mediaPlayer.CommandManager.PlayReceived -= CommandManager_PlayReceived;
            //mediaPlayer.CommandManager.PauseReceived -= CommandManager_PauseReceived;

            currentPlaylist.OnCurrentItemChanged -= CurrentPlaylistOnCurrentItemChanged;

            positionTimer.Stop();
            positionTimer.Tick -= PositionTimerOnTick;
        }

        private void AppResuming(object sender, object e)
        {
            Initialize();
        }

        private void AppSuspending(object sender, SuspendingEventArgs suspendingEventArgs)
        {
            Close();
        }

        private void CurrentPlaylistOnCurrentItemChanged(object sender, AudioFile audio)
        {
            Seek(TimeSpan.Zero);
            if (IsPlaying) Pause();

            UpdateTransportControl();

            if (audio == null)
                return;


            if (audio.IsLocal)
            {
                if(audio.Source != null)
                {
                    PlayFrom(audio.Source);
                }else
                {
                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        CurrentAudioChanged?.Invoke(this, EventArgs.Empty);
                    });

                    TryResolveTrack(audio);
                }
                
            }
            else PlayFrom(new Uri(audio.SourceString));


            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                CurrentAudioChanged?.Invoke(this, EventArgs.Empty);
            });

        }

        private void TryResolveTrack(AudioFile audio)
        {
            //if (resolveCancellationToken != null)
            //    resolveCancellationToken.Cancel();

            //resolveCancellationToken = new CancellationTokenSource();

            //var token = resolveCancellationToken.Token;

            //try
            //{

            //    if ( !token.IsCancellationRequested)
            //    {
            //        audio.Source = resolvedTrack.Source;
            //        audio.Duration = resolvedTrack.Duration;
            //        audio.Id = resolvedTrack.Id;
            //        audio.OwnerId = resolvedTrack.OwnerId;

            //        PlayFrom(resolvedTrack.Source);

            //        UpdateTransportControl();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Log.Error(ex);
            //}
        }

        private async void PlayFrom(IStorageFile file)
        {
            try
            {
                mediaPlayer.Source = MediaSource.CreateFromStorageFile(file);
                mediaPlayer.Play();
            }catch(Exception e)
            {
                await ContentDialogService.Show(new ExceptionDialog("Невозможно воспроизвести файл", "Возможно он поврежден или нет доступа к нему", e));
            }
            
        }

        private async void PlayFrom(Uri file)
        {
            try
            {
                mediaPlayer.Source = MediaSource.CreateFromUri(file);
                mediaPlayer.Play();
            }
            catch (Exception e)
            {
                await ContentDialogService.Show(new ExceptionDialog("Невозможно воспроизвести файл", "Возможно он поврежден или нет доступа к нему", e));
            }

        }

        private void PositionTimerOnTick(object sender, object o)
        {
            PositionChanged?.Invoke(this, Position);
        }

        private void UpdateTransportControl()
        {
            mediaPlayer.SystemMediaTransportControls.PlaybackStatus = IsPlaying ? MediaPlaybackStatus.Playing : MediaPlaybackStatus.Stopped;

            var updater = mediaPlayer.SystemMediaTransportControls.DisplayUpdater;

            if (CurrentPlaylist?.CurrentItem != null)
            {
                updater.Type = MediaPlaybackType.Music;
                updater.MusicProperties.Title = CurrentPlaylist.CurrentItem.Title;
                updater.MusicProperties.Artist = CurrentPlaylist.CurrentItem.Artist;
                    //updater.ImageProperties.Subtitle = CurrentPlaylist.CurrentItem.Artist;
                //updater.ImageProperties.Title = CurrentPlaylist.CurrentItem.Title;
                updater.Thumbnail = RandomAccessStreamReference.CreateFromUri(new Uri(CurrentPlaylist.CurrentItem.Cover));
                updater.Update();
            }
            else
            {
                updater.ClearAll();
            }
        }


        private void MediaPlayerOnCurrentStateChanged(MediaPlaybackSession sender, object args)
        {
            Log.Info(sender.PlaybackState.ToString());

            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                if (sender.PlaybackState == MediaPlaybackState.Playing)
                    positionTimer.Start();
                else
                    positionTimer.Stop();

                PlayStateChanged?.Invoke(this, EventArgs.Empty);
            });

            UpdateTransportControl();
        }

        private void MediaPlayerOnMediaEnded(MediaPlayer sender, object args)
        {
            SwitchNext();
        }

        private void MediaPlayerOnMediaFailed(MediaPlayer sender, MediaPlayerFailedEventArgs args)
        {
            if (args.Error == MediaPlayerError.SourceNotSupported)
            {
                //audio source url may expire
                CurrentPlaylist.CurrentItem.Source = null;
                TryResolveTrack(CurrentPlaylist.CurrentItem);
            }else if(args.Error == MediaPlayerError.NetworkError)
            {
                InternetService.GoToOfflineMode();
            }

            DispatcherHelper.CheckBeginInvokeOnUI(async() =>
            {
                await ContentDialogService.Show(new ExceptionDialog("Невозможно загрузить аудио файл", $"Невозможно загрузить файл по этой причине: {args.Error.ToString()}", new Exception(args.ErrorMessage)));

            });
                //Log.Error("Media failed. " + args.Error + " " + args.ErrorMessage);
        }

        private void CommandManager_NextReceived(MediaPlaybackCommandManager sender, MediaPlaybackCommandManagerNextReceivedEventArgs args)
        {
            SwitchNext(true);
        }

        private void CommandManager_PreviousReceived(MediaPlaybackCommandManager sender, MediaPlaybackCommandManagerPreviousReceivedEventArgs args)
        {
            SwitchPrev();
        }

        private void CommandManager_PlayReceived(MediaPlaybackCommandManager sender, MediaPlaybackCommandManagerPlayReceivedEventArgs args)
        {
            Play();
        }

        private void CommandManager_PauseReceived(MediaPlaybackCommandManager sender, MediaPlaybackCommandManagerPauseReceivedEventArgs args)
        {
            Pause();
        }
    }
}
