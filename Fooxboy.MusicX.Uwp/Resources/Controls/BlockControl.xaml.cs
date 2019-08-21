using System;
using System.Collections.Generic;
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
using Fooxboy.MusicX.Uwp.Models;
using Fooxboy.MusicX.Uwp.Services.VKontakte;

// Документацию по шаблону элемента "Пользовательский элемент управления" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234236

namespace Fooxboy.MusicX.Uwp.Resources.Controls
{
    public sealed partial class BlockControl : UserControl
    {

        public static readonly DependencyProperty BlockProperty = DependencyProperty.Register("Block", typeof(Block), typeof(BlockControl), new PropertyMetadata(new Block()
        {
            BlockId = "", CountElements = 0, Description = "", PlaylistsFiles = null, TrackFiles =  null, Playlists = null, Tracks = null, Title = "", TypeBlock = "default"
        }, (d, e) =>
        {
            var control = (BlockControl)d;
            var value = (Block)e.NewValue;
            if (value.PlaylistsFiles != null) control.ListViewPlaylists.Visibility = Visibility.Visible;
            if (value.TrackFiles != null) control.GridViewTracks.Visibility = Visibility.Visible;
        }));

        public Block Block
        {
            get => (Block)GetValue(BlockProperty);
            set
            {
                SetValue(BlockProperty, value);
               
            }
        }

        public BlockControl()
        {
            this.InitializeComponent();
            
        }

        public AudioFile SelectAudioFile { get; set; }
        public PlaylistFile SelectPlaylist { get; set; }

        private async void GridViewTracks_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            if (SelectAudioFile != null)
            {
                var playlistCurrent = new PlaylistFile()
                {
                    Artist = "Неизвестный исполнитель",
                    Cover = "ms-appx:///Assets/Images/playlist-placeholder.png",
                    Id = 666,
                    IsLocal = false,
                    Name = Block.Title
                };

                playlistCurrent.TracksFiles = Block.TrackFiles;

                await MusicService.PlayMusic(SelectAudioFile, 2, playlistCurrent);
            }
        }

        private void ListViewPlaylists_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            if (SelectPlaylist == null) return;
            StaticContent.NavigationContentService.Go(typeof(Views.PlaylistView), SelectPlaylist);
        }
    }
}
