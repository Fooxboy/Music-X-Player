using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Fooxboy.MusicX.Core;
using Fooxboy.MusicX.Uwp.Models;
using Fooxboy.MusicX.Uwp.Services;
using Fooxboy.MusicX.Uwp.ViewModels;
using GalaSoft.MvvmLight.Threading;
using Newtonsoft.Json;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Fooxboy.MusicX.Uwp
{
    /// <summary>
    /// Обеспечивает зависящее от конкретного приложения поведение, дополняющее класс Application по умолчанию.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Инициализирует одноэлементный объект приложения. Это первая выполняемая строка разрабатываемого
        /// кода, поэтому она является логическим эквивалентом main() или WinMain().
        /// </summary>
        public App()
        {
            Log.Run();
            Log.Trace("Инициализация объекта приложения");
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }


        protected async override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Log.Trace("Приложение запускается пользователем.");
            Frame rootFrame = Window.Current.Content as Frame;


            if (rootFrame == null)
            {
                Log.Trace("Создание фрейма, который является контектом навигации.");
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if(e != null)
                {
                    if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                    {
                        //TODO: Загрузить состояние из ранее приостановленного приложения
                    }
                }
                

                CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
                if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.ApplicationView"))
                {
                    var appView = ApplicationView.GetForCurrentView();
                    appView.TitleBar.ButtonBackgroundColor = Colors.Transparent;
                    appView.TitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
                }

                var localpath = ApplicationData.Current.LocalFolder;
                StaticContent.LocalFolder = localpath;
               
                if (await localpath.TryGetItemAsync("Playlists") == null)
                {
                    var pathPlaylists = await localpath.CreateFolderAsync("Playlists");
                    StaticContent.LocalFolder = localpath;
                    StaticContent.PlaylistsFolder = pathPlaylists;
                    StaticContent.CoversFolder = await localpath.CreateFolderAsync("Covers");
                    var file = await pathPlaylists.CreateFileAsync("Id1.json");
                    var file2 = await pathPlaylists.CreateFileAsync("Id2.json");
                    var playlist = new Models.PlaylistFile()
                    {
                        Artist = "Music X",
                        Cover = "ms-appx:///Assets/Images/latest.png",
                        Id = 1,
                        Name = "Слушали недавно",
                        Tracks = new List<Models.AudioFile>()
                    };

                    var playlist2 = new Models.PlaylistFile()
                    {
                        Artist = "Music X",
                        Cover = "ms-appx:///Assets/Images/favorites.png",
                        Id = 2,
                        Name = "Избранное",
                        Tracks = new List<AudioFile>()
                    };

                    var a = JsonConvert.SerializeObject(playlist);
                    var b = JsonConvert.SerializeObject(playlist2);
                    await FileIO.WriteTextAsync(file, a);
                    await FileIO.WriteTextAsync(file2, b);
                    var musicFile = await localpath.CreateFileAsync("MusicCollection.json");
                    var musicString = JsonConvert.SerializeObject(new MusicCollection() { Music = new List<AudioFile>(),
                        DateLastUpdate = "none" });
                    await FileIO.WriteTextAsync(musicFile, musicString);
                }else
                {
                    StaticContent.PlaylistsFolder = await localpath.GetFolderAsync("Playlists");
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

                StaticContent.CoversFolder = await localpath.GetFolderAsync("Covers");


                Log.Trace("Размещение фрейма в текущем окне.");
                DispatcherHelper.Initialize();
                //StaticContent.AudioService = AudioService.Instance;
                // Размещение фрейма в текущем окне
                Window.Current.Content = rootFrame;
            }

            if(e != null)
            {
                if (e.PrelaunchActivated == false)
                {
                    if (rootFrame.Content == null)
                    {
                        // Если стек навигации не восстанавливается для перехода к первой странице,
                        // настройка новой страницы путем передачи необходимой информации в качестве параметра
                        // навигации
                        rootFrame.Navigate(typeof(Views.MainFrameView), null);
                    }
                    // Обеспечение активности текущего окна
                    Window.Current.Activate();
                }
            }else
            {
                if (rootFrame.Content == null)
                {
                    // Если стек навигации не восстанавливается для перехода к первой странице,
                    // настройка новой страницы путем передачи необходимой информации в качестве параметра
                    // навигации
                    rootFrame.Navigate(typeof(Views.MainFrameView), null);
                }
                // Обеспечение активности текущего окна
                Window.Current.Activate();
            }

            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;


        }

        /// <summary>
        /// Вызывается в случае сбоя навигации на определенную страницу
        /// </summary>
        /// <param name="sender">Фрейм, для которого произошел сбой навигации</param>
        /// <param name="e">Сведения о сбое навигации</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            StaticContent.NavigationContentService.Back();
        }

        /// <summary>
        /// Вызывается при приостановке выполнения приложения.  Состояние приложения сохраняется
        /// без учета информации о том, будет ли оно завершено или возобновлено с неизменным
        /// содержимым памяти.
        /// </summary>
        /// <param name="sender">Источник запроса приостановки.</param>
        /// <param name="e">Сведения о запросе приостановки.</param>
        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Сохранить состояние приложения и остановить все фоновые операции
            var audio = StaticContent.NowPlay;
            var file = new LastPlay()
            {
                Playlist = StaticContent.NowPlayPlaylist,
                Track = audio,
                Volume = StaticContent.Volume
            };
            var json = JsonConvert.SerializeObject(file);
            var lastFile = await StaticContent.LocalFolder.GetFileAsync("LastPlay.json");
            await FileIO.WriteTextAsync(lastFile, json);
            deferral.Complete();
        }

        protected async override void OnFileActivated(FileActivatedEventArgs args)
        {
            var files = args.Files;
            if(files.Count > 1)
            {
                var playlist = new PlaylistFile()
                {
                    Artist = "Music X",
                    Cover = "/Assets/Images/now.png",
                    Id = 1000,
                    Name = "Сейчас играет",
                    Tracks = new List<AudioFile>()
                };
                foreach(var file in files)
                {
                    var audio = await FindMetadataService.ConvertToAudioFile((StorageFile)file);
                    playlist.Tracks.Add(audio);
                }

                StaticContent.NowPlayPlaylist = playlist;
                StaticContent.OpenFiles = true;
            }
            else
            {
                var file = files[0];
                var audio = await FindMetadataService.ConvertToAudioFile((StorageFile)file);
                StaticContent.NowPlay = audio;
                StaticContent.OpenFiles = true;

            }
            if (Window.Current.Visible)
            {
                HomeLocalViewModel.Instanse.Page_Loaded(null, null);
            }else
            {
                OnLaunched(null);
            }
        }
    }
}
