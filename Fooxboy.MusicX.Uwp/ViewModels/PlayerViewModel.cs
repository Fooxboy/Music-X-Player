using Fooxboy.MusicX.Uwp.Services;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Reactive;
using DynamicData;
using DynamicData.Binding;
using VkNet.Model.Attachments;

namespace Fooxboy.MusicX.Uwp.ViewModels
{
    public class PlayerViewModel : ReactiveObject
    {
	    private readonly PlayerService _playerService;

        public ReactiveCommand<Audio, Unit> PlayCommand { get; set; }
        public ReactiveCommand<Unit, Unit> PauseCommand { get; set; }
        public ReactiveCommand<Unit, Unit> NextCommand { get; set; }
        public ReactiveCommand<Unit, Unit> PreviousCommand { get; set; }

        [Reactive]
        public bool IsPlay { get; set; }

        [Reactive]
        public string Title { get; set; }

        [Reactive]
        public string Artist { get; set; }

        [Reactive]
        public string Cover { get; set; }

        public ObservableCollection<Audio> Tracks { get; set; }

        [Reactive]
        public bool IsShuffle { get; set; }

        [Reactive]
        public int RepeatMode { get; set; }

        [Reactive]
        public double Volume { get; set; }

        [Reactive]
        public double Position { get; set; }

        [Reactive]
        public double Duration { get; set; }

        public PlayerViewModel(PlayerService playerService, DiscordService discordService)
        {
            _playerService = playerService;

            Tracks = new ObservableCollection<Audio>();
            Volume = 100;

            PlayCommand = ReactiveCommand.Create<Audio>(audio => _playerService.Play(audio));
            PauseCommand = ReactiveCommand.Create(() => _playerService.Pause());
            NextCommand = ReactiveCommand.Create(() => _playerService.NextTrack());
            PreviousCommand = ReactiveCommand.Create(() => _playerService.PreviousTrack());

            _playerService.PlayStateChangedEvent += PlayStateChanged;
            _playerService.PositionTrackChangedEvent += PositionTrackChanged;
            _playerService.TrackChangedEvent += TrackChanged;

            this.WhenAnyValue(model => model.Volume).Subscribe(volume => _playerService.Volume = volume / 100);
            this.WhenAnyValue(model => model.IsShuffle).Subscribe(shuffle => _playerService.IsShuffle = shuffle);
            this.WhenAnyValue(model => model.RepeatMode).Subscribe(mode => _playerService.RepeatMode = mode);

            foreach (var track in _playerService.Tracks) Tracks.Add(track);
            //discordService.Init();
        }

        private void TrackChanged(object sender, EventArgs e)
        {
	        Title = _playerService.CurrentTrack.Title;
            Artist = _playerService.CurrentTrack.Artist;
            Cover = _playerService.CurrentTrack.Album?.Cover?.Photo600;
            Duration = _playerService.CurrentTrack.Duration;
        }

        private void PositionTrackChanged(object sender, TimeSpan e)
        {
	        Position = _playerService.Position.TotalSeconds;
        }

        private void PlayStateChanged(object sender, EventArgs e)
        {
            IsPlay = _playerService.IsPlaying;
        }
    }
}