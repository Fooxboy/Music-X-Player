using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
using DryIoc;
using Fooxboy.MusicX.Uwp.Views;

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


        private CurrentUserService currentUserService;
        private Api _api;
        private NotificationService _notificationService;
        private PlayerService _player;
        public TrackControl()
        {
            _api = Container.Get.Resolve<Api>();
            _notificationService = Container.Get.Resolve<NotificationService>();
            currentUserService = Container.Get.Resolve<CurrentUserService>();
            _player = Container.Get.Resolve<PlayerService>();
            var logger = Container.Get.Resolve<LoggerService>();
            var navigation = Container.Get.Resolve<NavigationService>();

            this.InitializeComponent();

            DeleteCommand = new RelayCommand(async () => { await DeleteTrack(); });

            AddOnLibraryCommand = new RelayCommand(async () => { await AddToLibrary(); });

            GoToArtistCommand = new RelayCommand(() =>
            {
                try
                {
                    var artistId = Track.Artists[0].Id;
                    navigation.Go(typeof(ArtistView), new object[] { _api, _notificationService, artistId, _player, logger }, 1);

                }catch (Exception ex)
                {
                    logger.Error(ex.Message, ex);

                    _notificationService.CreateNotification("Невозможно перейти к артисту", $"Ошибка: {ex.Message}");

                }
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

        private async Task DeleteTrack()
        {
            try
            {
                await _api.VKontakte.Music.Tracks.DeleteTrackAsync(Track.Id, Track.OwnerId.Value);
                _notificationService.CreateNotification("Аудиозапись удалена",
                    $"{Track.Artist} - {Track.Title} удален из Вашей музыки.");

                TitleText.Foreground = new SolidColorBrush(Colors.Gray);
                ArtistText.Foreground = new SolidColorBrush(Colors.Gray);
                Track.IsAvailable = false;

                Delete.IsEnabled = false;
            }
            catch (Exception e)
            {
                _notificationService.CreateNotification("Невозможно удалить аудиозапись", $"Ошибка: {e.Message}");

            }


        }

        private async Task AddToLibrary()
        {
            try
            {
                await _api.VKontakte.Music.Tracks.AddTrackAsync(Track.Id, Track.OwnerId.Value);
                _notificationService.CreateNotification("Аудиозапись добавлена", $"{Track.Artist} - {Track.Title} добавлена к Вам в бибилотеку.");

                AddOnLibrary.IsEnabled = false;
            }
            catch (Exception e)
            {
                _notificationService.CreateNotification("Невозможно добавить аудиозапись", $"Ошибка: {e.Message}");
            }

        }

        
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
           // GoToArtist.Visibility = Visibility.Collapsed;
            AddOnLibrary.Visibility = Visibility.Visible;

            _notificationService.CreateNotification($"{Track.Artist} - {Track.Title}", $"Is Licensed = {Track.IsLicensed}, ArtstsCount = {Track.Artists.Count()}");
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

            if (Track.Artists?.Count > 0)
            {
                GoToArtist.IsEnabled = true;
            }
            else
            {
                GoToArtist.IsEnabled = false;
            }

            if (Track.OwnerId == currentUserService.UserId) AddOnLibrary.IsEnabled = false;
            else
            {
                Delete.IsEnabled = false;
            }

            if (Track.AccessKey != "space" && Track.AccessKey != "loading")
            {
                if (Track.Artists?.Count > 0)
                {
                    string s = string.Empty;
                    foreach (var trackArtist in Track.Artists)
                    {
                        s += trackArtist.Name + ", ";
                    }

                    var artists = s.Remove(s.Length - 2);

                    ArtistText.Text = artists;
                }
                else
                {
                    ArtistText.Text = Track.Artist;
                }
            }


            if (Track.AccessKey == "loading")
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
