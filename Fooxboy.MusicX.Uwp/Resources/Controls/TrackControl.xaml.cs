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
using Fooxboy.MusicX.Uwp.Services.VKontakte;
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
using Fooxboy.MusicX.Uwp.Views.VKontakte;

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
                if (Track.IsLocal)
                {
                    await PlayMusicService.PlayMusicForLibrary(Track, 1);
                }else
                {
                    await MusicService.PlayMusic(Track, 1);
                }
                
            });

            

            DeleteCommand = new RelayCommand(async () =>
            {
                try
                {
                    if(Track.IsLocal)
                    {
                        StaticContent.Music.Remove(Track);
                        AudioFile trackByPlaylist = null;
                        if (Track.PlaylistId != 0)
                        {
                            var playlist = await Services.PlaylistsService.GetById(Track.PlaylistId);
                            trackByPlaylist = playlist.TracksFiles.Single(t => t.SourceString == Track.SourceString);
                            playlist.TracksFiles.Remove(trackByPlaylist);
                            await Services.PlaylistsService.SavePlaylist(playlist);
                        }
                        if (trackByPlaylist != null)
                        {
                            if (trackByPlaylist.Source == null)
                                trackByPlaylist.Source = await StorageFile.GetFileFromPathAsync(Track.SourceString);
                            await trackByPlaylist.Source.DeleteAsync();
                        }
                        else
                        {
                            if (Track.Source == null)
                                Track.Source = await StorageFile.GetFileFromPathAsync(Track.SourceString);
                            await Track.Source.DeleteAsync();
                        }

                        await MusicFilesService.UpdateMusicCollection();
                    }else
                    {
                        //TODO: удаление трека ебаный врот
                    }
                    
                }catch(Exception e)
                {
                    await ContentDialogService.Show(new ExceptionDialog("Невозможно удалить этот трек", "Возможно, этот трек был уже удален.", e));
                }

            });

            AddToFavoriteCommand = new RelayCommand(async () =>
            {
                try
                {
                    if(Track.IsLocal)
                    {
                        var playlist = await Services.PlaylistsService.GetById(2);
                        if (playlist.TracksFiles.Any(t => t.SourceString == Track.SourceString))
                        {
                            var dialog = new MessageDialog("Данный трек уже добавлен в избранное", "Ошибка при добавлении в избранное");
                            await dialog.ShowAsync();
                        }
                        else
                        {
                            Like.Visibility = Visibility.Collapsed;
                            LikeAdd.Visibility = Visibility.Visible;
                            Track.IsFavorite = true;
                            playlist.TracksFiles.Add(Track);
                            await Services.PlaylistsService.SavePlaylist(playlist);
                        }
                    }else
                    {

                    }
                   
                }catch(Exception e)
                {
                    await ContentDialogService.Show(new ExceptionDialog("Невозможно добавить трек в избранное", "Возможно, этот трек поврежден или не существует плейлиста, если ошибка будет повторяться, переустановите приложение.", e));
                }

            });

            RemoveFavoriteCommand = new RelayCommand(() =>
            {

            });
            
            AddOnLibraryCommand = new RelayCommand(async () =>
            {
                try
                {
                    await Fooxboy.MusicX.Core.VKontakte.Music.Add.ToLibrary(Track.Id, Track.AccessKey);
                    await new MessageDialog("Трек добавлен в Вашу библиотеку").ShowAsync();
                }catch(Flurl.Http.FlurlHttpException)
                {
                    InternetService.GoToOfflineMode();
                }
                catch (Exception e)
                {
                    await ContentDialogService.Show(new ExceptionDialog("Ошибка при добавлении трека", "Возникла ошибка при добавлении трека в Вашу библиотеку", e));
                }
            });

            GetPropertyCommand = new RelayCommand(async () =>
            {
                await ContentDialogService.Show(new PropertiesTrackContentDialog(Track));
            });

            GoToArtistCommand = new RelayCommand(() =>
            {
                StaticContent.NavigationContentService.Go(typeof(ArtistView), new ArtistParameter()
                {
                    Id = Track.ArtistId,
                    Name = Track.Artist
                });
            });

        }

        public AudioFile Track
        {
            get { return (AudioFile)GetValue(TrackProperty); }
            set { SetValue(TrackProperty, value); }
        }

        async void AddToPlaylist(PlaylistFile playlist)
        {
            try
            {
                if (playlist.TracksFiles.Any(t => t == Track)) return;
                Track.PlaylistId = playlist.Id;
                playlist.TracksFiles.Add(Track);
                await Services.PlaylistsService.SavePlaylist(playlist);
                var index = StaticContent.Music.IndexOf(Track);
                var track = StaticContent.Music[index];
                track.PlaylistId = playlist.Id;
                await MusicFilesService.UpdateMusicCollection();
            }catch(Exception e)
            {
                await ContentDialogService.Show(new ExceptionDialog("Невозможно добавить трек в плейлист", "Возможно, плейлиста не существует или трек был удален", e));
            }
            
        }
        private RelayCommand PlayCommand { get; set; }
        private RelayCommand DeleteCommand { get; set; }
        private RelayCommand AddToPlaylistCommand { get; set; }
        public RelayCommand AddToFavoriteCommand { get; set; }
        public RelayCommand RemoveFavoriteCommand { get; set; }
        public RelayCommand GetPropertyCommand { get; set; }
        public RelayCommand AddOnLibraryCommand { get; set; }
        public RelayCommand GoToArtistCommand { get; set; }


        private void Grid_PointerEntered(object sender, PointerRoutedEventArgs e)
        {

            RectanglePlay.Visibility = Visibility.Visible;
            IconPlay.Visibility = Visibility.Visible;

            if (Track.IsLocal)
            {
                if(Track.IsFavorite)
                {
                    Like.Visibility = Visibility.Collapsed;
                    LikeAdd.Visibility = Visibility.Visible;
                }else
                {
                    Like.Visibility = Visibility.Visible;
                    LikeAdd.Visibility = Visibility.Collapsed;
                }
            }else
            {
            }

        }



        private void Grid_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            RectanglePlay.Visibility = Visibility.Collapsed;
            IconPlay.Visibility = Visibility.Collapsed;
            if (Track.IsLocal)
            {
                LikeAdd.Visibility = Visibility.Collapsed;
                Like.Visibility = Visibility.Collapsed;
            }else
            {
            }
        }

        private void Grid_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            if (Track.IsLocal)
            {
                GoToArtist.Visibility = Visibility.Collapsed;
                PropertyButton.Visibility = Visibility.Visible;
                AddOnLibrary.Visibility = Visibility.Collapsed;

                foreach (var playlist in StaticContent.Playlists)
                {
                    if (playlist.Id != 1 & playlist.Id != 2 & playlist.Id != 1000)
                    {
                        AddTo.Items.Add(new MenuFlyoutItem
                        {
                            Text = playlist.Name,
                            Icon = new FontIcon()
                            {
                                FontFamily = new FontFamily("Segoe MDL2 Assets"),
                                Glyph = "&#xE93C;"
                            },
                            Command = new RelayCommand<PlaylistFile>(AddToPlaylist),
                            CommandParameter = playlist
                        });
                    }
                }
            }else
            {
                GoToArtist.Visibility = Visibility.Collapsed;
                PropertyButton.Visibility = Visibility.Collapsed;
                AddTo.Visibility = Visibility.Collapsed;

                if(Track.IsInLibrary)
                {
                    AddOnLibrary.Visibility = Visibility.Collapsed;
                }else
                {
                    AddOnLibrary.Visibility = Visibility.Visible;
                }

                if (Track.IsLicensed)
                {
                    if (Track.ArtistId != 0)
                    {
                        GoToArtist.Visibility = Visibility.Visible;
                    }
                }

            }
        }
    }
}
