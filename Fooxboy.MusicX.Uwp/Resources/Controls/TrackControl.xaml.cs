using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Fooxboy.MusicX.Core;
using Fooxboy.MusicX.Uwp.Models;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пользовательский элемент управления" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234236

namespace Fooxboy.MusicX.Uwp.Resources.Controls
{
    public sealed partial class TrackControl : UserControl
    {
        public static readonly DependencyProperty TrackProperty = DependencyProperty.Register("Track",
            typeof(AudioFile), typeof(TrackControl), new PropertyMetadata(new AudioFile()
            {
                Artist = "MusicX",
                Cover = "ms-appx:///Assets/Images/placeholder.png",
                Duration = TimeSpan.FromSeconds(1),
                DurationMinutes = "00:00",
                DurationSeconds = 0,
                Id = -2,
                InternalId = 0,
                OwnerId = 0,
                PlaylistId = 0,
                Source = null,
                SourceString = "ms-appx:///Assets/Audio/song.mp3",
                Title = "MusicX"
            }));

        public TrackControl()
        {
            this.InitializeComponent();

            PlayCommand = new RelayCommand(() =>
            {
                Log.Info("aaaa");
            });
        }

        public AudioFile Track
        {
            get { return (AudioFile)GetValue(TrackProperty); }
            set { SetValue(TrackProperty, value); }
        }

        private RelayCommand PlayCommand { get; set; }
    }
}
