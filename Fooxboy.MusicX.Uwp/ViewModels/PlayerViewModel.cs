using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Uwp.Enums;
using Fooxboy.MusicX.Uwp.Interfaces;
using Fooxboy.MusicX.Uwp.Services;
using Fooxboy.MusicX.Uwp.Utils.Extensions;
using Windows.UI.Xaml;

namespace Fooxboy.MusicX.Uwp.ViewModels
{
    public class PlayerViewModel : BaseViewModel
    {
        //Синглтон 
        private static PlayerViewModel instanse;
        public static PlayerViewModel Instanse
        {
            get
            {
                if (instanse != null) return instanse;
                instanse = new PlayerViewModel();
                return instanse;
            }
        }

        /// <summary>
        /// Приватный конструкор
        /// </summary>
        private PlayerViewModel()
        {
            StaticContent.AudioService.PlayStateChanged += AudioServicePlayStateChanged;
            StaticContent.AudioService.PositionChanged += AudioServicePositionChanged;
            StaticContent.AudioService.CurrentAudioChanged += AudioServiceCurrentAudioChanged;

            PlayPauseCommand = new RelayCommand(
                () =>
                {
                    if (!IsPlaying)
                        StaticContent.AudioService.Play();
                    else
                        StaticContent.AudioService.Pause();

                    Changed("VisibilityTextPlay");
                    Changed("VisibilityTextPause");
                }
                );

            SwitchNextCommand = new RelayCommand(() =>
            {
                StaticContent.AudioService.SwitchNext(skip: true);
            });

            SwitchPrevCommand = new RelayCommand(() =>
            {
                StaticContent.AudioService.SwitchPrev();
            });

            
        }
        

        public RelayCommand PlayPauseCommand { get; private set; } 

        public RelayCommand SwitchNextCommand { get; private set; }

        public RelayCommand SwitchPrevCommand { get; private set; }

        public bool IsPlaying
        {
            get { return StaticContent.AudioService.IsPlaying; }
            set
            {
                //аоаоаомммм
            }
        }

       // public string AudioCover {


        public IAudio CurrentAudio => StaticContent.AudioService.CurrentPlaylist.CurrentItem;
        public TimeSpan Position => StaticContent.AudioService.Position;
        public double PositionSeconds
        {
            get
            {
                return StaticContent.AudioService.Position.TotalSeconds; 
            }
            set
            {
                if (StaticContent.AudioService.Position.TotalSeconds == value)
                    return;

                StaticContent.AudioService.Seek(TimeSpan.FromSeconds(value));
            }
        }

        public TimeSpan Duration => StaticContent.AudioService.Duration;

        public double DurationSeconds
        {
            get
            {
                return StaticContent.AudioService.Duration.TotalSeconds;
            }
            set
            {

            }
        }

        public double Volume
        {
            get { return StaticContent.Volume * 100f; }
            set
            {
                var v = value / 100f;
                if (StaticContent.AudioService.Volume == v)
                    return;

                StaticContent.AudioService.Volume = v;
                StaticContent.Volume = v;
                Changed("Volume");
            }
        }

        public bool Repeat
        {
            get { return StaticContent.AudioService.Repeat == RepeatMode.Always; }
            set
            {
                StaticContent.AudioService.Repeat = value ? RepeatMode.Always : RepeatMode.None;
                StaticContent.Repeat = StaticContent.AudioService.Repeat;
            }
        }

        public Visibility VisibilityTextPlay
        {
            get
            {
                return !IsPlaying ? Visibility.Visible : Visibility.Collapsed;
            }
            set
            {

            }
        }

        public Visibility VisibilityTextPause
        {
            get
            {
                return IsPlaying ? Visibility.Visible : Visibility.Collapsed;
            }
            set
            {

            }
        }

        private void AudioServicePlayStateChanged(object sender, EventArgs e)
        {

            Changed("IsPlaying");
            Changed("TextPlayButton");
            Changed("VisibilityTextPlay");
            Changed("VisibilityTextPause");
            //Обновление плиточки
            //TileHelper.UpdateIsPlaying(IsPlaying);
        }

        private async  void AudioServiceCurrentAudioChanged(object sender, EventArgs e)
        {
            Changed("CurrentAudio");
            Changed("Duration");
            Changed("DurationSeconds");
            Changed("VisibilityTextPlay");
            Changed("VisibilityTextPause");
            await MusicFilesService.SetLastPlayAudio(CurrentAudio.ToAudioFile());
            StaticContent.NowPlay = CurrentAudio.ToAudioFile();
        }

        private void AudioServicePositionChanged(object sender, TimeSpan position)
        {
            Changed(nameof(Position));
            Changed(nameof(PositionSeconds));
            Changed("Duration");
            Changed("DurationSeconds");
        }

    }
}
