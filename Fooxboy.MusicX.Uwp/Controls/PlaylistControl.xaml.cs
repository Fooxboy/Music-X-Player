using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Fooxboy.MusicX.Uwp.Models;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Fooxboy.MusicX.Uwp.Services;
using Windows.UI.Popups;
using Fooxboy.MusicX.Core.Interfaces;
using Fooxboy.MusicX.Uwp.Resources.ContentDialogs;
using Windows.Storage;
using DryIoc;
using Fooxboy.MusicX.Core;
using Fooxboy.MusicX.Uwp.Converters;
using Fooxboy.MusicX.Uwp.Views;

// Документацию по шаблону элемента "Пользовательский элемент управления" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234236

namespace Fooxboy.MusicX.Uwp.Resources.Controls
{
    public sealed partial class PlaylistControl : UserControl
    {

        public static readonly DependencyProperty PlaylistProperty = DependencyProperty.Register("Album",
            typeof(Album), typeof(PlaylistControl), new PropertyMetadata(new Album
            {
                Year = 2020,
                Artists = new List<IArtist>(),
                Followers = 0,
                Description = "",
                Cover = "ms-appx:///Assets/Images/placeholder-album.png",
                Genres = new List<string>(),
                Id = -2,
                IsAvailable = false,
                IsFollowing = false,
                OwnerId = -2,
                Plays = 0,
                TimeCreate = DateTime.Now,
                TimeUpdate = DateTime.Now,
                Title = "",
                Tracks = new List<ITrack>(),
                Type = 0
            }));


        public string Artists { get; set; }

        private IContainer _container;

        private Api _api;
        private PlayerService _player;
        private NotificationService _notification;
        private CurrentUserService _currentUserService;

        public PlaylistControl()
        {
            _container = Container.Get;
            IsDeleted = false;

            _api = _container.Resolve<Api>();
            _player = _container.Resolve<PlayerService>();
            _notification = _container.Resolve<NotificationService>();
            _currentUserService = _container.Resolve<CurrentUserService>();

            this.InitializeComponent();
            PlayCommand = new RelayCommand( async () => { await PlayAlbum(); });

            DeleteCommand = new RelayCommand(async () => { await DeleteAlbum(); });
            AddToLibCommand = new RelayCommand(async () => { await AddToLibAlbum(); });
        }


        public Album Album
        {
            get => (Album)GetValue(PlaylistProperty);
            set
            {
                
                
                SetValue(PlaylistProperty, value);
            }
        }

        public RelayCommand PlayCommand { get; set; }
        public RelayCommand DeleteCommand { get; set; }
        public RelayCommand AddToLibCommand { get; set; }
        private bool IsDeleted;

        public async Task AddToLibAlbum()
        {
            _notification.CreateNotification("Невозможно добавить альбом", $"Эта возможность пока что недоступна.");

        }

        public async Task DeleteAlbum()
        {
            try
            {
                await _api.VKontakte.Music.Albums.Delete(Album.Id, Album.OwnerId);
                _notification.CreateNotification("Альбом удален", $"{Album.Title} был удален из Вашей библиотеки.");
                DeletedAlbum.Visibility = Visibility.Visible;
                IsDeleted = true;
            }
            catch (Exception e)
            {
                _notification.CreateNotification("Не удалось удалить альбом", $"Ошибка: {e.Message}");
            }
        }
        public async Task PlayAlbum()
        {
            try
            {
                _notification.CreateNotification("Воспроизведение альбома",
                    "Подождите, получаем информацию о списке треков.");
                var tracks =
                    await _api.VKontakte.Music.Tracks.GetAsync(100, 0, Album.AccessKey, Album.Id, Album.OwnerId);
                var tracksNew = tracks.ToListTrack();
                _player.Play(Album, 0, tracksNew);
            }
            catch (Exception e)
            {
                _notification.CreateNotification("Невозможно воспроизвести альбом", $"Ошибка: {e.Message}");
            }
            
        }

        private async void Grid_PointerEntered(object sender, PointerRoutedEventArgs e)
        {

            await PlaylistControlGrid.Scale(centerX: 50.0f,
                        centerY: 50.0f,
                        scaleX: 1.1f,
                        scaleY: 1.1f,
                        duration: 200, delay: 0, easingType: EasingType.Back).StartAsync();
        }

        private async void Grid_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            await PlaylistControlGrid.Scale(centerX: 50.0f,
                        centerY: 50.0f,
                        scaleX: 1.0f,
                        scaleY: 1.0f,
                        duration: 200, delay: 0, easingType: EasingType.Back).StartAsync();


        }

        private void ImageEx_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            BorderShadow.Height = e.NewSize.Height;
            BorderShadow.Width = e.NewSize.Width;
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            if (Album.OwnerId == _currentUserService.UserId)
            {
                AddToLib.IsEnabled = false;
                Delete.IsEnabled = true;
            }
            else
            {
                AddToLib.IsEnabled = true;
                Delete.IsEnabled = false;
            }

            if (Album.Artists.Count > 0)
            {
                string s = string.Empty;
                foreach (var trackArtist in Album.Artists)
                {
                    s += trackArtist.Name + ", ";
                }

                var artists = s.Remove(s.Length - 2);

                ArtistsText.Text = artists;
            }
            else
            {
                if (Album.OwnerId > 0)
                {
                    var owner = await _container.Resolve<Api>().VKontakte.Users.Info.OwnerAsync(Album.OwnerId);
                    this.ArtistsText.Text = owner.FirstName + " " + owner.LastName;
                }
                
            }

            this.coverPlaylist.Source = Album.Cover;
            this.TitilePlaylist.Text = Album.Title;

        }

        private void PlaylistControlGrid_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            if (IsDeleted)
            {
                _notification.CreateNotification("Невозможно открыть альбом", "Альбом был удален.");
                return;
            }
            var a = this;

            _container = Container.Get;

            var navigateService = _container.Resolve<NavigationService>();
            navigateService.Go(typeof(PlaylistView), new PlaylistViewNavigationData() {Album= this.Album, Container = _container}, 1);
        }
    }
}
