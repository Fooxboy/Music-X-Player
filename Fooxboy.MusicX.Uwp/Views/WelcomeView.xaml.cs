using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Fooxboy.MusicX.Uwp.Models;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Newtonsoft.Json;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Fooxboy.MusicX.Uwp.Services;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Fooxboy.MusicX.Uwp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WelcomeView : Page
    {
        public WelcomeView()
        {
            this.InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            ProgressRing.IsActive = true;
            Start.Text = "Music X выполняет первичную настройку. Пожалуйста, подождите...";
            ButtonStart.Visibility = Visibility.Collapsed;

            //создание папки с плейлистами.
            var localpath = ApplicationData.Current.LocalFolder;

            if (await localpath.TryGetItemAsync("Playlists") == null)
            {
                var pathPlaylists = await localpath.CreateFolderAsync("Playlists");
                StaticContent.PlaylistsFolder = pathPlaylists;
                var filePlaylistId1 = await pathPlaylists.CreateFileAsync("Id1.json");
                var filePlaylistId2 = await pathPlaylists.CreateFileAsync("Id2.json");

                var playlistLastPlay  = new Models.PlaylistFile()
                {
                    Artist = "Music X",
                    Cover = "ms-appx:///Assets/Images/latest.png",
                    Id = 1,
                    Name = "Слушали недавно",
                    Tracks = new List<Models.AudioFile>()
                };

                var playlistFavorite = new Models.PlaylistFile()
                {
                    Artist = "Music X",
                    Cover = "ms-appx:///Assets/Images/favorites.png",
                    Id = 2,
                    Name = "Избранное",
                    Tracks = new List<AudioFile>()
                };

                var jsonPlaylistId1 = JsonConvert.SerializeObject(playlistLastPlay);
                var jsonPlaylistId2 = JsonConvert.SerializeObject(playlistFavorite);
                await FileIO.WriteTextAsync(filePlaylistId1, jsonPlaylistId1);
                await FileIO.WriteTextAsync(filePlaylistId2, jsonPlaylistId2);
            }

            if(await localpath.TryGetItemAsync("Covers") == null)
            {
                StaticContent.CoversFolder = await localpath.CreateFolderAsync("Covers");
            }

            if (await localpath.TryGetItemAsync("MusicCollection.json") == null)
            {
                var musicFile = await localpath.CreateFileAsync("MusicCollection.json");
                var musicString = JsonConvert.SerializeObject(new MusicCollection()
                {
                    Music = new List<AudioFile>(),
                    DateLastUpdate = "none"
                });
                await FileIO.WriteTextAsync(musicFile, musicString);
            }

            if (await localpath.TryGetItemAsync("LastPlay.json") == null)
            {
                var lastFile = await localpath.CreateFileAsync("LastPlay.json");
                var audio = new AudioFile()
                {
                    Artist = "",
                    Cover = "ms-appx:///Assets/Images/placeholder.png",
                    DurationMinutes = "00:00",
                    DurationSeconds = 0,
                    Id = -2,
                    InternalId = -2,
                    OwnerId = -2,
                    PlaylistId = 1,
                    SourceString = "ms-appx:///Assets/Audio/song.mp3",
                    Source = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/Audio/song.mp3")),
                    Title = "Сейчас ничего не воспроизводится"
                };
                var lastplayModel = new LastPlay()
                {
                    Playlist = null,
                    Track = audio,
                    Volume = 1.0f,
                };
                var jsonLastFile = JsonConvert.SerializeObject(lastplayModel);
                await FileIO.WriteTextAsync(lastFile, jsonLastFile);
            }

            if (await localpath.TryGetItemAsync("ConfigApp.json") == null)
            {
                var configFile = await localpath.CreateFileAsync("ConfigApp.json");
                var config = new ConfigApp()
                {
                    FindInDocumentsLibrary = false,
                    FindInMusicLibrary = true,
                    ThemeApp = 0
                };
                var configString = JsonConvert.SerializeObject(config);
                await FileIO.WriteTextAsync(configFile, configString);

                StaticContent.Config = config;
            }

            await MusicFilesService.GetMusicLocal(true);

            var rootFrame = (Frame)Window.Current.Content;
            rootFrame.Navigate(typeof(Views.MainFrameView), null);
        }
    }
}
