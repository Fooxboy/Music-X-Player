using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Uwp.Enums;
using Fooxboy.MusicX.Uwp.Interfaces;

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

        public IAudio CurrentAudio => StaticContent.AudioService.CurrentPlaylist.CurrentItem;
        public TimeSpan Position => StaticContent.AudioService.Position;
        public double PositionSeconds
        {
            get { return StaticContent.AudioService.Position.TotalSeconds; }
            set
            {
                if (StaticContent.AudioService.Position.TotalSeconds == value)
                    return;

                StaticContent.AudioService.Seek(TimeSpan.FromSeconds(value));
            }
        }

        public TimeSpan Duration => StaticContent.AudioService.Duration;

        public double DurationSeconds => StaticContent.AudioService.Duration.TotalSeconds;

        public double Volume
        {
            get { return StaticContent.AudioService.Volume * 100f; }
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

        private void AudioServicePlayStateChanged(object sender, EventArgs e)
        {

            Changed("IsPlaying");

            //Обновление плиточки
            //TileHelper.UpdateIsPlaying(IsPlaying);
        }

        private  void AudioServiceCurrentAudioChanged(object sender, EventArgs e)
        {

            Changed("CurrentAudio");
            //TODO сделать обновление обложки
        }

        private void AudioServicePositionChanged(object sender, TimeSpan position)
        {
            Changed("Position");
            Changed("PositionSeconds");
            Changed("Duration");
            Changed("DurationSeconds");
        }


        //Поле в котором хранится имя исполнителя, так и все другие поля оформляются.
        private string artist;
        public string  Artist
        {
            get => artist;
            set
            {
                if (artist == value) return;
                artist = value;
                Changed("Artist");
            }
        }

    }
}
