using Fooxboy.MusicX.Uwp.Converters;
using Fooxboy.MusicX.Uwp.Services;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Reactive;
using VkNet.Model.Attachments;

namespace Fooxboy.MusicX.Uwp.ViewModels
{
    public class PlayerViewModel : ReactiveObject
    {
        public PlayerService PlayerSerivce { get; set; }

        public ReactiveCommand<Unit, Unit> PlayCommand { get; set; }
        public ReactiveCommand<Unit, Unit> PauseCommand { get; set; }
        public ReactiveCommand<Unit, Unit> NextCommand { get; set; }
        public ReactiveCommand<Unit, Unit> PreviousCommand { get; set; }

        [Reactive]
        public bool IsPlay { get; set; }

        [Reactive]
        public bool VisibilityPlay { get; set; }

        [Reactive]
        public string Title { get; set; }

        [Reactive]
        public string Artist { get; set; }

        [Reactive]
        public string Cover { get; set; }

        public Action CloseBigPlayer { get; set; }

        public ObservableCollection<Audio> CurrentNowPlaing { get; set; }

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
            get => PlayerSerivce.Volume * 100;
            set
            {
                if (PlayerSerivce.Volume == (value / 100)) return;

                PlayerSerivce.Volume = (value / 100);
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

        [Reactive]
        public double SecondsAll { get; set; }

        [Reactive]
        public string Time { get; set; }

        [Reactive]
        public string AllTime { get; set; }


        public PlayerViewModel(PlayerService playerService, DiscordService discordService)
        {
            CurrentNowPlaing = new ObservableCollection<Audio>();
            PlayerSerivce = playerService;
            PlayCommand = ReactiveCommand.Create(() => PlayerSerivce.Play());
            PauseCommand = ReactiveCommand.Create(() => PlayerSerivce.Pause());
            NextCommand = ReactiveCommand.Create(() => PlayerSerivce.NextTrack());
            PreviousCommand = ReactiveCommand.Create(() => PlayerSerivce.PreviousTrack());

            PlayerSerivce.PlayStateChangedEvent += PlayStateChanged;
            PlayerSerivce.PositionTrackChangedEvent += PositionTrackChanged;
            PlayerSerivce.TrackChangedEvent += TrackChanged;

            VisibilityPlay = true;
            IsPlay = false;

            foreach (var track in PlayerSerivce.Tracks) CurrentNowPlaing.Add(track);
            //discordService.Init();
        }

        private void TrackChanged(object sender, EventArgs e)
        {
            CurrentNowPlaing.Clear();
            foreach (var track in PlayerSerivce.Tracks) CurrentNowPlaing.Add(track);
            Title = PlayerSerivce.CurrentTrack.Title;
            //foreach(var artist in PlayerSerivce.CurrentTrack.Artists) Artist += $", {artist.Name}";
            Artist = PlayerSerivce.CurrentTrack.Artist;
            Cover = PlayerSerivce.CurrentTrack.Album?.Cover?.Photo600;
            SecondsAll = PlayerSerivce.Duration;
        }

        private void PositionTrackChanged(object sender, TimeSpan e)
        {
            SecondsAll = PlayerSerivce.Duration;
            Seconds = PlayerSerivce.Position.TotalSeconds;
            Time = Seconds.ConvertToTime();
            AllTime = SecondsAll.ConvertToTime();
        }

        private void PlayStateChanged(object sender, EventArgs e)
        {
            IsPlay = PlayerSerivce.IsPlaying;
            VisibilityPlay = !IsPlay;
        }

        public void SeekToPosition(long position)
        {
            PlayerSerivce.Seek(TimeSpan.FromSeconds(position));
        }
    }
}