using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Fooxboy.MusicX.Core;
using Fooxboy.MusicX.Core.Interfaces;
using Fooxboy.MusicX.Uwp.Models;
using Fooxboy.MusicX.Uwp.Resources.ContentDialogs;
using Fooxboy.MusicX.Uwp.Services;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Networking.Sockets;
using Windows.UI;

// Документацию по шаблону элемента "Пользовательский элемент управления" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234236

namespace Fooxboy.MusicX.Uwp.Resources.Controls
{
    public sealed partial class TrackControl : UserControl
    {
        public static readonly DependencyProperty TrackProperty = DependencyProperty.Register("Track",
            typeof(Track), typeof(TrackControl), new PropertyMetadata(new Track()
            {
                Album = new Album(),
                Artist = "Music X",
                Artists = new List<IArtist>(),
                Duration = TimeSpan.Zero,
                GenreId = 0,
                Id = 0,
                IsAvailable = false,
                IsLicensed = false,
                OwnerId = -2,
                Subtitle = "",
                Title = ""
            }));

        public TrackControl()
        {
            this.InitializeComponent();

            PlayCommand = new RelayCommand( async () =>
            {
               
            });

            

            DeleteCommand = new RelayCommand(async () =>
            {
                
            });

           
            
            AddOnLibraryCommand = new RelayCommand(async () =>
            {
                
            });

            
            GoToArtistCommand = new RelayCommand(() =>
            {
               
            });

        }


        public string Artists { get; set; }

        public Track Track
        {
            get { return (Track)GetValue(TrackProperty); }
            set
            {
                //foreach (var artist in value.Artists) Artists += artist.Name + ", ";
                SetValue(TrackProperty, value);
            }
        }

      
        private RelayCommand PlayCommand { get; set; }
        private RelayCommand DeleteCommand { get; set; }
        public RelayCommand AddOnLibraryCommand { get; set; }
        public RelayCommand GoToArtistCommand { get; set; }


        private void Grid_PointerEntered(object sender, PointerRoutedEventArgs e)
        {

            RectanglePlay.Visibility = Visibility.Visible;
            IconPlay.Visibility = Visibility.Visible;
        }



        private void Grid_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            RectanglePlay.Visibility = Visibility.Collapsed;
            IconPlay.Visibility = Visibility.Collapsed;
        }

        private void Grid_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            GoToArtist.Visibility = Visibility.Collapsed;
            AddOnLibrary.Visibility = Visibility.Visible;
           
            if (Track.IsLicensed)
            {
                if (Track.Artist != null)
                {
                    if (Track.Artist.Length != 0)
                    {
                        GoToArtist.Visibility = Visibility.Visible;
                    }
                    
                }
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if(Track.AccessKey == "loading")
            {
                LoadingGrid.Visibility = Visibility.Visible;
                TrackGrid.Visibility = Visibility.Collapsed;
            }else if(Track.AccessKey == "space")
            {
                LoadingGrid.Visibility = Visibility.Visible;
                TrackGrid.Visibility = Visibility.Collapsed;
                progressRing.Visibility = Visibility.Collapsed;
            }

            if(!Track.IsAvailable)
            {
                TitleText.Foreground = new SolidColorBrush(Colors.Gray);
                ArtistText.Foreground = new SolidColorBrush(Colors.Gray);

            }
        }
    }
}
