using DryIoc;
using Fooxboy.MusicX.Uwp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fooxboy.MusicX.Uwp.ViewModels
{
    public class PlayerViewModel:BaseViewModel
    {
        public PlayerService PlayerSerivce { get; set; }

        public RelayCommand PlayCommand { get; set; }
        public RelayCommand PauseCommand { get; set; }
        public RelayCommand NextCommand { get; set; }
        public RelayCommand PreviousCommand { get; set; }
        public bool IsPlay => PlayerSerivce.IsPlaying;
        public bool VisibilityPause => !IsPlay;
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Cover { get; set; }
        public double Seconds { get; set; }
        public double SecondsAll { get; set; }
        public string Time { get; set; }
        public string AllTime { get; set; }

        public PlayerViewModel()
        {
            PlayerSerivce = Container.Get.Resolve<PlayerService>();
            PlayCommand = new RelayCommand(() => PlayerSerivce.Play());
            PauseCommand = new RelayCommand(() => PlayerSerivce.Pause());
            NextCommand = new RelayCommand(() => PlayerSerivce.NextTrack());
            PreviousCommand = new RelayCommand(() => PlayerSerivce.PreviousTrack());

            PlayerSerivce.PlayStateChangedEvent += PlayStateChanged;
            PlayerSerivce.PositionTrackChangedEvent += PositionTrackChanged;
            PlayerSerivce.TrackChangedEvent += TrackChanged;
        }

        private void TrackChanged(object sender, EventArgs e)
        {
            Title = PlayerSerivce.CurrentTrack.Title;
            foreach(var artist in PlayerSerivce.CurrentTrack.Artists) Artist += $", {artist.Name}";
            Cover = PlayerSerivce.CurrentTrack.Album?.Cover;
            SecondsAll = PlayerSerivce.Duration.TotalSeconds;
            Changed("Title");
            Changed("Artist");
            Changed("Cover");
            Changed("SecondsAll");
        }

        private void PositionTrackChanged(object sender, TimeSpan e)
        {
            SecondsAll = PlayerSerivce.Duration.TotalSeconds;
            Seconds = PlayerSerivce.Position.TotalSeconds;
            Time = Seconds.ToString();
            AllTime = SecondsAll.ToString();
            Changed("SecondsAll");
            Changed("Seconds");
            Changed("Time");
            Changed("AllTime");
        }

        private void PlayStateChanged(object sender, EventArgs e)
        {
            Changed("IsPlay");
            Changed("VisibilityPause");
        }
    }
}
