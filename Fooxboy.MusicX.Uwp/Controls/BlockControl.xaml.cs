using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using DryIoc;
using Fooxboy.MusicX.Core;
using Fooxboy.MusicX.Core.Models.Music.BlockInfo;
using Fooxboy.MusicX.Core.VKontakte.Music;
using Fooxboy.MusicX.Uwp.Converters;
using Fooxboy.MusicX.Uwp.Models;
using Fooxboy.MusicX.Uwp.Services;
using Fooxboy.MusicX.Uwp.Views;
using Album = Fooxboy.MusicX.Uwp.Models.Album;
using Block = Fooxboy.MusicX.Core.Models.Block;
using Track = Fooxboy.MusicX.Uwp.Models.Track;

// Документацию по шаблону элемента "Пользовательский элемент управления" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234236

namespace Fooxboy.MusicX.Uwp.Controls
{
    public sealed partial class BlockControl : UserControl
    {

        private PlayerService _player;
        private NavigationService _navigation;
        private Api _api;
        private NotificationService _notification;

        public BlockControl()
        {
            this.InitializeComponent();
            _player = Container.Get.Resolve<PlayerService>();
            _navigation = Container.Get.Resolve<NavigationService>();
            _api = Container.Get.Resolve<Api>();
            _notification = Container.Get.Resolve<NotificationService>();
        }

        public static readonly DependencyProperty BlockProperty = DependencyProperty.Register("Block", typeof(Block),
            typeof(BlockControl), new PropertyMetadata(new Block()));

        private List<Track> TracksLastRelease;

        public Block Block
        {
            get => (Block)GetValue(BlockProperty);
            set
            {
                SetValue(BlockProperty, value);
            }
        }

        private void BlockControl_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.BorderShadow.Width = e.NewSize.Width - 50;
            this.BackgroundRect.Width = e.NewSize.Width - 50;

        }

        public Album LastRelease { get; set; }
      

        private async void BlockControl_OnLoaded(object sender, RoutedEventArgs e)
        {

            if (Block.Type == "playlists")
            {
                this.ListAlbums.ItemsSource = await Block.Albums.ToAlbumsList();
                this.TracksGrid.Visibility = Visibility.Collapsed;
                this.PlaylistsGrid.Visibility = Visibility.Visible;
                this.ArtistsGrid.Visibility = Visibility.Collapsed;
            }

            if(Block.Type == "alghoritm")
            {
                this.ListAlgAlbums.ItemsSource = await Block.Albums.ToAlbumsList();
                this.TracksGrid.Visibility = Visibility.Collapsed;
                this.PlaylistsGrid.Visibility = Visibility.Collapsed;
                this.ArtistsGrid.Visibility = Visibility.Collapsed;
                this.AlghoritmGrid.Visibility = Visibility.Visible;
                ShowAllButton.Visibility = Visibility.Collapsed;
                BackgroundRect.Height = 330;
                BorderShadow.Height = 330;

            }

            if (Block.Type == "audios_list" || Block.Type == "audios" || Block.Type == "top_audios")
            {
                this.ListTracks.ItemsSource = await Block.Tracks.ToListTrack();
                this.TracksGrid.Visibility = Visibility.Visible;
                this.PlaylistsGrid.Visibility = Visibility.Collapsed;
                this.ArtistsGrid.Visibility = Visibility.Collapsed;

            }

            if (Block.Type == "custom_image_small")
            {
                this.ArtistsList.ItemsSource = Block.Artists.ToList();
                ArtistsGrid.Visibility = Visibility.Visible;
                this.TracksGrid.Visibility = Visibility.Collapsed;
                this.PlaylistsGrid.Visibility = Visibility.Collapsed;
                ShowAllButton.Visibility = Visibility.Collapsed;
            }

            if (Block.Type == "videos" || Block.Type == "artist_videos")
            {
                ArtistsGrid.Visibility = Visibility.Collapsed;
                this.TracksGrid.Visibility = Visibility.Collapsed;
                this.PlaylistsGrid.Visibility = Visibility.Collapsed;
                VideosGrid.Visibility = Visibility.Visible;
                ShowAllButton.Visibility = Visibility.Collapsed;
            }

            if (Block.Type == "single_playlist")
            {
                if (Block.Albums == null)
                {
                    NoData.Visibility = Visibility.Visible;
                    Data.Visibility = Visibility.Collapsed;
                }
                else
                {
                    var album = Block.Albums[0];
                    var tracks = _api.VKontakte.Music.Tracks.Get(100, 0, album.AccessKey, album.Id, album.OwnerId);
                    TracksLastRelease = await tracks.ToListTrack();
                    LastRelease = await Block.Albums.FirstOrDefault().ToAlbum();
                    CoverLastRelease.Source = LastRelease.Cover;
                    TitleLastRelease.Text = LastRelease.Title;
                    ArtistLastRelease.Text = LastRelease.Artists[0].Name;
                    NoData.Visibility = Visibility.Collapsed;
                    Data.Visibility = Visibility.Visible;
                }

                LastReleaseGrid.Visibility = Visibility.Visible;
                ArtistsGrid.Visibility = Visibility.Collapsed;
                this.TracksGrid.Visibility = Visibility.Collapsed;
                this.PlaylistsGrid.Visibility = Visibility.Collapsed;
                VideosGrid.Visibility = Visibility.Collapsed;
                ShowAllButton.Visibility = Visibility.Collapsed;
            }

        }

        private async void ListTracks_OnItemClick(object sender, ItemClickEventArgs e)
        {
            var tracks = await Block.Tracks.ToListTrack();

            var track = (Track)e.ClickedItem;

            var position = tracks.IndexOf(tracks.Single(t=> t.Id == track.Id && t.Url == track.Url));

            await _player.Play(position, tracks);
        }


        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {

            var c = Container.Get;


            if (Block.Type == "audios_list" || Block.Type == "audios" || Block.Type == "top_audios")
            {
                _navigation.Go(typeof(AllTracksView), new object[] { _player, c.Resolve<Api>(), "block", Block.Id, _notification, c.Resolve<LoggerService>() }, 1);
            }
            if (Block.Type == "playlists")
            {
                _navigation.Go(typeof(AllPlaylistsView), new AllPlaylistsModel()
                {
                    AlbumLoader = Container.Get.Resolve<AlbumLoaderService>(),
                    Container = Container.Get,
                    Id = 0,
                    BlockId = Block.Id,
                    TitlePage = Block.Title,
                    TypeViewPlaylist = AllPlaylistsModel.TypeView.RecomsAlbums
                }, 1);
            }
        }

        private async void ArtistsList_OnItemClick(object sender, ItemClickEventArgs e)
        {
            var c = Container.Get;

            var elem = (SearchArtistBlockInfo)e.ClickedItem;

            if (elem.Meta.ContentType != "artist")
            {
                await Launcher.LaunchUriAsync(new Uri(elem.Url));
            }
            else
            {
                var tracks = await _api.VKontakte.Music.Search.TracksAsync(elem.Title, 1);
                var track = tracks.FirstOrDefault();
                if (track != null)
                {
                    var artist = track.Artists?.SingleOrDefault(a=> a.Name == elem.Title);
                    if (artist != null)
                    {
                        _navigation.Go(typeof(ArtistView), new object[]{_api, _notification, artist.Id, _player, c.Resolve<LoggerService>() }, 1);
                    }
                }
            }


            //throw new NotImplementedException();
        }

        private async void PlayLastRelease_OnClick(object sender, RoutedEventArgs e)
        {
            await _player.Play(0, TracksLastRelease);
        }

        private async void OpenPlaylistLastRelease_OnClick(object sender, RoutedEventArgs e)
        {
            _navigation.Go(typeof(PlaylistView), new PlaylistViewNavigationData() { Album =await Block.Albums[0].ToAlbum(), Container = Container.Get }, 1);
        }
    }
}
