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

        public PlayerViewModel()
        {
            PlayerSerivce = Container.Get.Resolve<PlayerService>();
            PlayCommand = new RelayCommand(() => PlayerSerivce.Play());
            PauseCommand = new RelayCommand(() => PlayerSerivce.Pause());
            NextCommand = new RelayCommand(() => PlayerSerivce.NextTrack());
            PreviousCommand = new RelayCommand(() => PlayerSerivce.PreviousTrack());
        }
    }
}
