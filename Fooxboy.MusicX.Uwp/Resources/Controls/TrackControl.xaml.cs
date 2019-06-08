using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Fooxboy.MusicX.Core;
using Fooxboy.MusicX.Uwp.Models;
using Fooxboy.MusicX.Uwp.Services;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
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

            PlayCommand = new RelayCommand( async () =>
            {
                await PlayMusicService.PlayMusicForLibrary(Track, 1);
            });

            AddToFavoriteCommand = new RelayCommand(async () =>
            {
                var playlist = await PlaylistsService.GetById(2);
                if (playlist.Tracks.Any(t => t.SourceString == Track.SourceString))
                {
                    var dialog = new MessageDialog("Данный трек уже добавлен в избранное", "Ошибка при добавлении в избранное");
                    await dialog.ShowAsync();
                } else
                {
                    playlist.Tracks.Add(Track);
                    await PlaylistsService.SavePlaylist(playlist);
                }
            });

            foreach(var playlist in StaticContent.Playlists)
            {
                AddTo.Items.Add(new MenuFlyoutItem {
                    Text = playlist.Name,
                    Icon = new FontIcon() { FontFamily =new FontFamily("Segoe MDL2 Assets"),
                        Glyph = "&#xE93C;" },
                    Command = new RelayCommand<PlaylistFile>(AddToPlaylist),
                    CommandParameter = playlist });
            }
            
        }

        public AudioFile Track
        {
            get { return (AudioFile)GetValue(TrackProperty); }
            set { SetValue(TrackProperty, value); }
        }


        async void AddToPlaylist(PlaylistFile playlist)
        {
            if (playlist.Tracks.Any(t => t == Track)) return;
            playlist.Tracks.Add(Track);
            await PlaylistsService.SavePlaylist(playlist);
        }
        private RelayCommand PlayCommand { get; set; }
        private RelayCommand AddToPlaylistCommand { get; set; }
        public RelayCommand AddToFavoriteCommand { get; set; }

        private void Grid_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            Like.Visibility = Visibility.Visible;
        }

        private void Grid_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            Like.Visibility = Visibility.Collapsed;
        }
    }
}
