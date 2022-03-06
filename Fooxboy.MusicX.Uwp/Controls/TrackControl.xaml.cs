using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fooxboy.MusicX.Uwp.Services;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI;
using DryIoc;
using Fooxboy.MusicX.Uwp.Views;
using Fooxboy.MusicX.Core.Models;
using Fooxboy.MusicX.Core.Services;
using NLog;

// Документацию по шаблону элемента "Пользовательский элемент управления" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234236

namespace Fooxboy.MusicX.Uwp.Resources.Controls
{
    public sealed partial class TrackControl : UserControl
    {
        public static readonly DependencyProperty TrackProperty = DependencyProperty.Register("Track",
            typeof(Audio), typeof(TrackControl), new PropertyMetadata(new Audio()
            {
                IsFocusTrack = true,
                AccessKey = "",
                Album = new Album(),
                Artist = "Ошибка загрузки артиста",
                Date = 0,
                Duration = 100,
                GenreId = 0,
                Id = 0,
                IsExplicit = true,
                IsLicensed = false,
                LyricsId = 0,
                MainArtists = new List<MainArtist>(),
                OwnerId = 0,
                ShortVideosAllowed = false,
                StoriesAllowed = false,
                StoriesCoverAllowed = false,
                Subtitle = "",
                Title = "Неизвестный трек",
                TrackCode = "",
                IsAvailable = false,
                Url = ""
            }));


        private CurrentUserService currentUserService;
        private VkService vkService;
        private NotificationService _notificationService;
        private PlayerService _player;
        private Logger logger;

        public TrackControl()
        {
            _notificationService = Container.Get.Resolve<NotificationService>();
            currentUserService = Container.Get.Resolve<CurrentUserService>();
            _player = Container.Get.Resolve<PlayerService>();
            var navigation = Container.Get.Resolve<NavigationService>();

            this.InitializeComponent();

            DeleteCommand = new RelayCommand(async () => { await DeleteTrack(); });

            AddOnLibraryCommand = new RelayCommand(async () => { await AddToLibrary(); });

            GoToArtistCommand = new RelayCommand(() =>
            {
                try
                {
                    var artistId = Track.MainArtists[0].Id;
                    navigation.Go(typeof(ArtistView), new object[] { _api, _notificationService, artistId, _player, logger }, 1);

                }catch (Exception ex)
                {
                    logger.Error(ex, ex.Message);

                    _notificationService.CreateNotification("Невозможно перейти к артисту", $"Ошибка: {ex.Message}");

                }
            });

        }


        public string Artists { get; set; }

        public Audio Track
        {
            get { return (Audio)GetValue(TrackProperty); }
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

                await vkService.AudioDeleteAsync(Track.Id, Track.OwnerId);
                
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
                await vkService.AudioAddAsync(Track.Id, Track.OwnerId);
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

            _notificationService.CreateNotification($"{Track.Artist} - {Track.Title}", $"Is Licensed = {Track.IsLicensed}, ArtstsCount = {Track.MainArtists.Count()}");
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

            if (Track.MainArtists?.Count > 0)
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
                if (Track.MainArtists?.Count > 0)
                {
                    string s = string.Empty;
                    foreach (var trackArtist in Track.MainArtists)
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
