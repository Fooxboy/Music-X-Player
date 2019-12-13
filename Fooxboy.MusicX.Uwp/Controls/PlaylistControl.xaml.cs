using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
using Fooxboy.MusicX.Uwp.Services.VKontakte;
using Fooxboy.MusicX.Uwp.Resources.ContentDialogs;
using Windows.Storage;

// Документацию по шаблону элемента "Пользовательский элемент управления" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234236

namespace Fooxboy.MusicX.Uwp.Resources.Controls
{
    public sealed partial class PlaylistControl : UserControl
    {

        public static readonly DependencyProperty PlaylistProperty = DependencyProperty.Register("Playlist",
            typeof(PlaylistFile), typeof(PlaylistControl), new PropertyMetadata(new PlaylistFile
            {
                Artist = "MusicX",
                Cover = "ms-appx:///Assets/Images/placeholder.png",
                Id = -1,
                Name = "MusicX",
                TracksFiles = new List<AudioFile>(),
                IsLocal = true
            }));

        public PlaylistControl()
        {
            this.InitializeComponent();
            PlayCommand = new RelayCommand( async () =>
            {
                if(Playlist.IsLocal) await Services.PlaylistsService.PlayPlaylist(Playlist);
                else
                {
                    if (Playlist.TracksFiles.Count == 0)
                    {
                        var tracks = await Fooxboy.MusicX.Core.VKontakte.Music.Playlist.GetTracks(Playlist.Id, Playlist.OwnerId, Playlist.AccessKey);
                        Playlist.TracksFiles = await MusicService.ConvertToAudioFile(tracks, Playlist.Cover);
                    }

                    if (Playlist.TracksFiles.Count == 0) return;

                    await MusicService.PlayMusic(Playlist.TracksFiles.First(), 2, Playlist);
                }

            });

            DeleteCommand = new RelayCommand(async () =>
            {
                if(Playlist.Id != 1 & Playlist.Id != 2 & Playlist.Id != 1000)
                {
                    if(Playlist.IsLocal) await Services.PlaylistsService.DeletePlaylist(Playlist);
                    else
                    {
                        await new MessageDialog("Вы не можете удалить этот плейлист.").ShowAsync();
                    }
                }
                else
                {
                    await new MessageDialog("Вы не можете удалить этот плейлист", "Невозможно удалить плейлист").ShowAsync();
                }
            }); 
        }


        public PlaylistFile Playlist
        {
            get => (PlaylistFile)GetValue(PlaylistProperty);
            set
            {
                SetValue(PlaylistProperty, value);
            }
        }

        public RelayCommand PlayCommand { get; set; }
        public RelayCommand DeleteCommand { get; set; }

        private async void Grid_PointerEntered(object sender, PointerRoutedEventArgs e)
        {

            await CoverPlaylist.Scale(centerX: 50.0f,
                        centerY: 50.0f,
                        scaleX: 1.1f,
                        scaleY: 1.1f,
                        duration: 200, delay: 0).StartAsync();
        }

        private async void Grid_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            await CoverPlaylist.Scale(centerX: 50.0f,
                        centerY: 50.0f,
                        scaleX: 1.0f,
                        scaleY: 1.0f,
                        duration: 200, delay: 0).StartAsync();

            
        }
    }
}
