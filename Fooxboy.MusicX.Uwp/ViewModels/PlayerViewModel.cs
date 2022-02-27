using DryIoc;
using Fooxboy.MusicX.Uwp.Converters;
using Fooxboy.MusicX.Uwp.Models;
using Fooxboy.MusicX.Uwp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flurl.Http;
using Fooxboy.MusicX.Core;
using Microsoft.Toolkit.Uwp.Helpers;

namespace Fooxboy.MusicX.Uwp.ViewModels
{
    public class PlayerViewModel:BaseViewModel
    {
        public PlayerService PlayerSerivce { get; set; }
        public RelayCommand PlayCommand { get; set; }
        public RelayCommand PauseCommand { get; set; }
        public RelayCommand NextCommand { get; set; }
        public RelayCommand PreviousCommand { get; set; }
        public RelayCommand LikeDislikeTrackCommand { get; set; }
        public bool IsPlay { get; set; }
        public bool VisibilityPlay { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Cover { get; set; }
        public bool VisibleLike { get; set; }
        public bool VisibleDislike { get; set; }
        public Action CloseBigPlayer { get; set; }

        public ObservableCollection<Track> CurrentNowPlaing { get; set; }

        public bool IsShuffle
        {
            get => PlayerSerivce.IsShuffle;
            set => PlayerSerivce.SetShuffle(value);
        }

        private async void LikeDislikeTrack()
        {
            var track = PlayerSerivce.CurrentTrack;
            try
            {
                if (VisibleLike)
                {
                    _logger.Trace("Добавление трека в библиотеку...");
                    await _api.VKontakte.Music.Tracks.AddTrackAsync(track.Id, track.OwnerId.Value);
                    _notification.CreateNotification("Трек добавлен",
                        $"Трек {track.Artist} - {track.Title} добавлен в Вашу библиотеку.");

                    VisibleLike = false;
                    VisibleDislike = true;
                    Changed("VisibleLike");
                    Changed("VisibleDislike");
                }
                else if (VisibleDislike)
                {
                    _logger.Trace("Удаление трека из библиотеки...");

                    await _api.VKontakte.Music.Tracks.DeleteTrackAsync(track.Id, track.OwnerId.Value);
                    _notification.CreateNotification("Трек удален",
                        $"Трек {track.Artist} - {track.Title} удален из Вашей библиотеки.");

                    VisibleLike = true;
                    VisibleDislike = false;
                    Changed("VisibleLike");
                    Changed("VisibleDislike");

                }
            }
            catch (FlurlHttpException e)
            {
                _logger.Error("Ошибка сети", e);
                _notification.CreateNotification("Произошла ошибка",
                    "Ошибка сети, попробуйте ещё раз или проверте Ваш доступ в Интернет.");
            }
            catch (Exception e)
            {
                _logger.Error("Неизвестная ошибка", e);
                _notification.CreateNotification("Ошибка", "Произошла неизвестная ошибка. Ошибка сохранена в логах.");
            }
           
        }

        public int RepeatMode
        {
            get => PlayerSerivce.RepeatMode;
            set => PlayerSerivce.SetRepeatMode(value);
        }
        public double Volume
        {
            get => PlayerSerivce.Volume *100;
            set
            {
                if (PlayerSerivce.Volume == (value/100)) return;

                PlayerSerivce.Volume = (value/100);
            }
        }
        public double Seconds
        {
            get => PlayerSerivce.Position.TotalSeconds;
            set
            {
                if (PlayerSerivce.Position.TotalSeconds == value)
                    return;

                PlayerSerivce.Seek(TimeSpan.FromSeconds(value));
            }
        }
        public double SecondsAll { get; set; }
        public string Time { get; set; }
        public string AllTime { get; set; }

        private IContainer _container;
        private ILoggerService _logger;
        private readonly CurrentUserService currentUserService;

        private Api _api;
        private NotificationService _notification;
        public PlayerViewModel(IContainer container)
        {
            currentUserService = Container.Get.Resolve<CurrentUserService>();

            this._container = container;
            VisibleLike = false;
            VisibleDislike = false;
            _api = container.Resolve<Api>();
            _notification = container.Resolve<NotificationService>();
            _logger = _container.Resolve<LoggerService>();
            CurrentNowPlaing = new ObservableCollection<Track>();
            PlayerSerivce = _container.Resolve<PlayerService>();
            PlayCommand = new RelayCommand(() => PlayerSerivce.Play());
            PauseCommand = new RelayCommand(() => PlayerSerivce.Pause());
            NextCommand = new RelayCommand(() => PlayerSerivce.NextTrack());
            PreviousCommand = new RelayCommand(() => PlayerSerivce.PreviousTrack());
            LikeDislikeTrackCommand = new RelayCommand(LikeDislikeTrack);
            PlayerSerivce.PlayStateChangedEvent += PlayStateChanged;
            PlayerSerivce.PositionTrackChangedEvent += PositionTrackChanged;
            PlayerSerivce.TrackChangedEvent += TrackChanged;

            VisibilityPlay = true;
            IsPlay = false;

            foreach (var track in PlayerSerivce.Tracks) CurrentNowPlaing.Add(track);
           // var discordService = _container.Resolve<DiscordService>();
           // discordService.Init();
        }

        private async void TrackChanged(object sender, EventArgs e)
        {
            _logger.Trace("Трек изменен.");

            long userId = currentUserService.UserId;
            if (PlayerSerivce.CurrentTrack.OwnerId == userId)
            {
                VisibleDislike = true;
                VisibleLike = false;
            }
            else
            {
                VisibleDislike = false;
                VisibleLike = true;
            }

            await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
            {
                CurrentNowPlaing.Clear();
                foreach (var track in PlayerSerivce.Tracks) CurrentNowPlaing.Add(track);
                Title = PlayerSerivce.CurrentTrack.Title;
                Artist = PlayerSerivce.CurrentTrack.Artist;
                Cover = PlayerSerivce.CurrentTrack.Album?.Cover;
                SecondsAll = PlayerSerivce.Duration.TotalSeconds;
                Changed("Title");
                Changed("Artist");
                Changed("Cover");
                Changed("SecondsAll");
                Changed("CurrentNowPlaing");
                Changed("VisibleLike");
                Changed("VisibleDislike");
            });

            
        }

        private void PositionTrackChanged(object sender, TimeSpan e)
        {
            SecondsAll = PlayerSerivce.Duration.TotalSeconds;
            Seconds = PlayerSerivce.Position.TotalSeconds;
            Time = Seconds.ConvertToTime();
            AllTime = SecondsAll.ConvertToTime();
            Changed("SecondsAll");
            Changed("Seconds");
            Changed("Time");
            Changed("AllTime");
        }

        private void PlayStateChanged(object sender, EventArgs e)
        {
            IsPlay = PlayerSerivce.IsPlaying;
            VisibilityPlay = !IsPlay;
            Changed("IsPlay");
            Changed("VisibilityPlay");
        }

        public void SeekToPosition(long position)
        {
            PlayerSerivce.Seek(TimeSpan.FromSeconds(position));
        }
    }
}
