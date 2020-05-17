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
using Fooxboy.MusicX.Core;

namespace Fooxboy.MusicX.Uwp.ViewModels
{
    public class PlayerViewModel:BaseViewModel
    {
        public PlayerService PlayerSerivce { get; set; }

        public RelayCommand PlayCommand { get; set; }
        public RelayCommand PauseCommand { get; set; }
        public RelayCommand NextCommand { get; set; }
        public RelayCommand PreviousCommand { get; set; }
        public bool IsPlay { get; set; }
        public bool VisibilityPlay { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Cover { get; set; }

        public Action CloseBigPlayer { get; set; }

        public ObservableCollection<Track> CurrentNowPlaing { get; set; }

        public bool IsShuffle
        {
            get => PlayerSerivce.IsShuffle;
            set => PlayerSerivce.SetShuffle(value);
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

        public PlayerViewModel(IContainer container)
        {
            this._container = container;
            _logger = _container.Resolve<LoggerService>();
            CurrentNowPlaing = new ObservableCollection<Track>();
            PlayerSerivce = _container.Resolve<PlayerService>();
            PlayCommand = new RelayCommand(() => PlayerSerivce.Play());
            PauseCommand = new RelayCommand(() => PlayerSerivce.Pause());
            NextCommand = new RelayCommand(() => PlayerSerivce.NextTrack());
            PreviousCommand = new RelayCommand(() => PlayerSerivce.PreviousTrack());

            PlayerSerivce.PlayStateChangedEvent += PlayStateChanged;
            PlayerSerivce.PositionTrackChangedEvent += PositionTrackChanged;
            PlayerSerivce.TrackChangedEvent += TrackChanged;

            VisibilityPlay = true;
            IsPlay = false;

            foreach (var track in PlayerSerivce.Tracks) CurrentNowPlaing.Add(track);
            var discordService = _container.Resolve<DiscordService>();
            discordService.Init();
        }

        private void TrackChanged(object sender, EventArgs e)
        {
            _logger.Trace("Трек изменен.");
            CurrentNowPlaing.Clear();
            foreach (var track in PlayerSerivce.Tracks) CurrentNowPlaing.Add(track);
            Title = PlayerSerivce.CurrentTrack.Title;
            //foreach(var artist in PlayerSerivce.CurrentTrack.Artists) Artist += $", {artist.Name}";
            Artist = PlayerSerivce.CurrentTrack.Artist;
            Cover = PlayerSerivce.CurrentTrack.Album?.Cover;
            SecondsAll = PlayerSerivce.Duration.TotalSeconds;
            Changed("Title");
            Changed("Artist");
            Changed("Cover");
            Changed("SecondsAll");
            Changed("CurrentNowPlaing");
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
