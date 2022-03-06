using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fooxboy.MusicX.Uwp.Models;
using Windows.UI.Xaml;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Fooxboy.MusicX.Uwp.Services;
using DryIoc;
using Fooxboy.MusicX.Uwp.Views;
using Fooxboy.MusicX.Core.Models;
using Fooxboy.MusicX.Core.Services;

// Документацию по шаблону элемента "Пользовательский элемент управления" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234236

namespace Fooxboy.MusicX.Uwp.Resources.Controls
{
    public sealed partial class PlaylistControl : UserControl
    {

        public static readonly DependencyProperty PlaylistProperty = DependencyProperty.Register("Album",
            typeof(Playlist), typeof(PlaylistControl), new PropertyMetadata(new Playlist
            {
               Followed = new Followed(),
               Followers = 0,
               IsFollowing = false,
               AccessKey = "",
               AlbumType = "",
               Audios = new List<Audio>(),
               Count = 0,
               CreateTime = 0,
               Description = "",
               Genres = new List<Genre>(),
               Id = 0,
               IsExplicit = false,
               MainArtists = new List<MainArtist>(),
               Original = new Original(),
               OwnerId = 0,
               Permissions = new Permissions(),
               Photo = new Photo(),
               PlayButton = true,
               Plays = 0,
               Title = "Альбом",
               SubtitleBadge = true,
               Type = 1,
               UpdateTime = 0,
               Year = 2022
            }));


        public string Artists { get; set; }

        private IContainer _container;

        private VkService vkService;
        private PlayerService _player;
        private NotificationService _notification;
        private CurrentUserService _currentUserService;

        public PlaylistControl()
        {
            _container = Container.Get;
            IsDeleted = false;

            _player = _container.Resolve<PlayerService>();
            _notification = _container.Resolve<NotificationService>();
            _currentUserService = _container.Resolve<CurrentUserService>();

            this.InitializeComponent();
            PlayCommand = new RelayCommand( async () => { await PlayAlbum(); });

            DeleteCommand = new RelayCommand(async () => { await DeleteAlbum(); });
            AddToLibCommand = new RelayCommand(async () => { await AddToLibAlbum(); });
        }


        public Playlist Album
        {
            get => (Playlist)GetValue(PlaylistProperty);
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
            try
            {
                await vkService.AddPlaylistAsync(Album.Id, Album.OwnerId, Album.AccessKey); 
             
                _notification.CreateNotification("Альбом добавлен", $"{Album.Title} был добавлен в Вашу библиотеку.");
                AddToLib.IsEnabled = false;
            }
            catch (Exception e)
            {
                _notification.CreateNotification("Невозможно добавить альбом", $"Ошибка: {e.Message}");

            }

        }

        public async Task DeleteAlbum()
        {
            try
            {
                await vkService.DeletePlaylistAsync(Album.Id, Album.OwnerId);
                _notification.CreateNotification("Альбом удален", $"{Album.Title} был удален из Вашей библиотеки.");
                DeletedAlbum.Visibility = Visibility.Visible;
                IsDeleted = true;
                Delete.IsEnabled = false;
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
               // var tracks = await _api.VKontakte.Music.Tracks.GetAsync(100, 0, Album.AccessKey, Album.Id, Album.OwnerId);
               // var tracksNew = await tracks.ToListTrack();
               // await _player.Play(0, tracksNew);
            }
            catch (Exception e)
            {
                _notification.CreateNotification("Невозможно воспроизвести альбом", $"Ошибка: {e.Message}");
            }
            
        }

        private async void Grid_PointerEntered(object sender, PointerRoutedEventArgs e)
        {


           

            /*   await PlaylistControlGrid.Scale(centerX: 50.0f,
                           centerY: 50.0f,
                           scaleX: 1.1f,
                           scaleY: 1.1f,
                           duration: 200, delay: 0, easingType: EasingType.Back).StartAsync();*/
        }

        private async void Grid_PointerExited(object sender, PointerRoutedEventArgs e)
        {

           
            /* await PlaylistControlGrid.Scale(centerX: 50.0f,
                         centerY: 50.0f,
                         scaleX: 1.0f,
                         scaleY: 1.0f,
                         duration: 200, delay: 0, easingType: EasingType.Back).StartAsync();*/


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

            if (Album.MainArtists.Count > 0)
            {
                string s = string.Empty;
                foreach (var trackArtist in Album.MainArtists)
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
                    var owner = await vkService.OwnerAsync(Album.OwnerId);
                    this.ArtistsText.Text = owner.Name;
                }
                
            }

            this.coverPlaylist.Source = Album.Cover;
            this.TitilePlaylist.Text = Album.Title;

            var theme = Application.Current.RequestedTheme;

            if (theme == ApplicationTheme.Light)
            {
                playblack.Visibility = Visibility.Visible;
                playwhite.Visibility = Visibility.Collapsed;
            }
            else
            {
                playblack.Visibility = Visibility.Collapsed;
                playwhite.Visibility = Visibility.Visible;
            }

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

            if (IsPlay) return;
            navigateService.Go(typeof(PlaylistView), new PlaylistViewNavigationData() {Album= this.Album, Container = _container}, 1);
        }

        private async void playlistC_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            PlaylistPlay.Visibility = Visibility.Visible;
            await AnimationBuilder.Create().Scale(to: 1.07f, duration: TimeSpan.FromMilliseconds(200),
               delay: TimeSpan.Zero, easingType: EasingType.Default).StartAsync(playlistC);
        }

        private async void playlistC_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            PlaylistPlay.Visibility = Visibility.Collapsed;

            await AnimationBuilder.Create().Scale(to: 1.0f, duration: TimeSpan.FromMilliseconds(200),
               delay: TimeSpan.Zero, easingType: EasingType.Default).StartAsync(playlistC);
        }

        private bool IsPlay = false;

        private async void PlayPlaylistButton_Click(object sender, RoutedEventArgs e)
        {
            IsPlay = true;
            await PlayAlbum();

        }
    }
}
