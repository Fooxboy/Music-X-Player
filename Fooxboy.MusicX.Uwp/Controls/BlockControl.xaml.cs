using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using DryIoc;
using Fooxboy.MusicX.Core;
using Fooxboy.MusicX.Core.Models;
using Fooxboy.MusicX.Core.VKontakte.Music;
using Fooxboy.MusicX.Uwp.Converters;
using Fooxboy.MusicX.Uwp.Models;
using Fooxboy.MusicX.Uwp.Services;
using Fooxboy.MusicX.Uwp.Views;
using Album = Fooxboy.MusicX.Uwp.Models.Album;
using Track = Fooxboy.MusicX.Uwp.Models.Track;

// Документацию по шаблону элемента "Пользовательский элемент управления" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234236

namespace Fooxboy.MusicX.Uwp.Controls
{
    public sealed partial class BlockControl : UserControl
    {

        private PlayerService _player;
        private NavigationService _navigation;

        public BlockControl()
        {
            this.InitializeComponent();
            _player = Container.Get.Resolve<PlayerService>();
            _navigation = Container.Get.Resolve<NavigationService>();
        }

        public static readonly DependencyProperty BlockProperty = DependencyProperty.Register("Block", typeof(Block),
            typeof(BlockControl), new PropertyMetadata(new Block()));



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
      

        private void BlockControl_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (Block.Albums != null)
            {
                this.ListAlbums.ItemsSource = Block.Albums.ToAlbumsList();
                this.TracksGrid.Visibility = Visibility.Collapsed;
                this.PlaylistsGrid.Visibility = Visibility.Visible;
            }

            if (Block.Tracks != null)
            {
                this.ListTracks.ItemsSource = Block.Tracks.ToListTrack();
                this.TracksGrid.Visibility = Visibility.Visible;
                this.PlaylistsGrid.Visibility = Visibility.Collapsed;
            } 
        }

        private void ListTracks_OnItemClick(object sender, ItemClickEventArgs e)
        {
            var tracks = Block.Tracks.ToListTrack();

            var track = (Track)e.ClickedItem;

            var position = tracks.IndexOf(tracks.Single(t=> t.Id == track.Id && t.Url == track.Url));

            _player.Play(new Album(),position, tracks);
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {

            var c = Container.Get;

            if (Block.Albums != null)
            {
                _navigation.Go(typeof(AllPlaylistsView), new AllPlaylistsModel()
                {
                    AlbumLoader = Container.Get.Resolve<AlbumLoaderService>(), Container = Container.Get, Id = 0, BlockId = Block.Id, TitlePage = Block.Title, TypeViewPlaylist = AllPlaylistsModel.TypeView.RecomsAlbums
                }, 1);
            }
            else
            {

                _navigation.Go(typeof(AllTracksView), new object[] {_player, c.Resolve<Api>(), "block", Block.Id}, 1);
            }
        }
    }
}
