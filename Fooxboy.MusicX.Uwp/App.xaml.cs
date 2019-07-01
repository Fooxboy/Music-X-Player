using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Fooxboy.MusicX.Core;
using Fooxboy.MusicX.Uwp.Models;
using Fooxboy.MusicX.Uwp.Resources.ContentDialogs;
using Fooxboy.MusicX.Uwp.Services;
using Fooxboy.MusicX.Uwp.Services.VKontakte;
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
            var settings = ApplicationData.Current.LocalSettings;
            InternetService.CheckConnection();
            try
            {
                Windows.Storage.ApplicationDataCompositeValue composite =
                    (Windows.Storage.ApplicationDataCompositeValue)settings.Values["themeApp"];

                Windows.Storage.ApplicationDataCompositeValue composite2 =
                    (Windows.Storage.ApplicationDataCompositeValue)settings.Values["IsPro"];

                if (composite == null)
                {
                    this.RequestedTheme = ApplicationTheme.Light;
                }
                else
                {
                    var theme = (int)settings.Values["themeApp"];
                    if (theme == 0)
                    {
                        this.RequestedTheme = ApplicationTheme.Light;

                    }
                    else
                    {
                        this.RequestedTheme = ApplicationTheme.Dark;
                    }
                }

                if (composite2 == null)
                {
                    StaticContent.IsPro = false;
                }
                else
                {
                    var IsPro = (bool)settings.Values["IsPro"];
                    StaticContent.IsPro = IsPro;
                }
            }
            catch
            {
                var theme = (int)settings.Values["themeApp"];
                if (theme == 0)
                {
                    this.RequestedTheme = ApplicationTheme.Light;

                }
                else
                {
                    this.RequestedTheme = ApplicationTheme.Dark;
                }


                var IsPro = (bool)settings.Values["IsPro"];
                StaticContent.IsPro = IsPro;
            }
            
            

            Log.Run();
            Log.Trace("Инициализация объекта приложения");
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            
        }

        protected async override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Log.Trace("OnLaunched");
            DispatcherHelper.Initialize();
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null)
            {
                Log.Trace("rootFrame != null");
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
                //try
                //{
                //    StaticContent.IsPro = await StoreService.IsBuyPro();
                //    //StaticContent.IsPro = true;
                //}catch(Exception eee)
                //{
                //    //await ContentDialogService.Show(new ExceptionDialog("Ошибка при получении лицензии", "АУЕ БЛЯ", eee));
                //    StaticContent.IsPro = false;
                //}
                
                var appView = ApplicationView.GetForCurrentView();
                appView.TitleBar.ButtonBackgroundColor = Colors.Transparent;
                appView.TitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;

                StaticContent.LocalFolder = ApplicationData.Current.LocalFolder;
                if (await StaticContent.LocalFolder.TryGetItemAsync("Playlists") != null)
                {
                    Log.Trace("Set playlistFolder");
                    StaticContent.PlaylistsFolder = await StaticContent.LocalFolder.GetFolderAsync("Playlists");
                }

                if(await StaticContent.LocalFolder.TryGetItemAsync("Covers") != null)
                {
                    Log.Trace("Set CoversFolder");

                    StaticContent.CoversFolder = await StaticContent.LocalFolder.GetFolderAsync("Covers");

                }

                if (await StaticContent.LocalFolder.TryGetItemAsync("ConfigApp.json") != null)
                {
                    var file = await StaticContent.LocalFolder.GetFileAsync("ConfigApp.json");
                    var fileString = await FileIO.ReadTextAsync(file);
                    var config = JsonConvert.DeserializeObject<ConfigApp>(fileString);
                    StaticContent.Config = config;
                }

                var settings = ApplicationData.Current.LocalSettings;




                try
                {
                    Windows.Storage.ApplicationDataCompositeValue composite =
                            (Windows.Storage.ApplicationDataCompositeValue)settings.Values["themeApp"];

                    if (composite == null)
                    {
                        appView.TitleBar.ButtonForegroundColor = Colors.Black;
                    }
                    else
                    {
                        var theme = (int)settings.Values["themeApp"];
                        if (theme == 0)
                        {
                            appView.TitleBar.ButtonForegroundColor = Colors.Black;

                        }
                        else
                        {
                            appView.TitleBar.ButtonForegroundColor = Colors.White;
                        }
                    }
                }catch
                {
                    var theme = (int)settings.Values["themeApp"];
                    if (theme == 0)
                    {
                        appView.TitleBar.ButtonForegroundColor = Colors.Black;

                    }
                    else
                    {
                        appView.TitleBar.ButtonForegroundColor = Colors.White;
                    }
                }



                if (InternetService.Connected)
                {
                    StaticContent.IsAuth = await AuthService.IsAuth();
                    if (StaticContent.IsAuth) await AuthService.AutoAuth();
                }else
                {
                    StaticContent.IsAuth = false;
                }


                Log.Trace("Размещение фрейма в текущем окне.");
                Window.Current.Content = rootFrame;
            }

            if(e != null)
            {
                if (e.PrelaunchActivated == false)
                {
                    if (rootFrame.Content == null)
                    {
                        if(await StaticContent.LocalFolder.TryGetItemAsync("RunApp.json") == null)
                        {
                            var runFile = await StaticContent.LocalFolder.CreateFileAsync("RunApp.json");
                            var model = new RunApp()
                            {
                                CodeName = "Test",
                                FirstStart = true,
                                RunUpdate = true
                            };

                            var json = JsonConvert.SerializeObject(model);
                            await FileIO.WriteTextAsync(runFile, json);

                            rootFrame.Navigate(typeof(Views.WelcomeView), null);
                        }else
                        {
                            rootFrame.Navigate(typeof(Views.MainFrameView), null);
                        }  
                    }

                }
            }else
            {
                if (rootFrame.Content == null)
                {

                    if (await StaticContent.LocalFolder.TryGetItemAsync("RunApp.json") == null)
                    {
                        var runFile = await StaticContent.LocalFolder.CreateFileAsync("RunApp.json");
                        var model = new RunApp()
                        {
                            CodeName = "Test",
                            FirstStart = true,
                            RunUpdate = true
                        };

                        var json = JsonConvert.SerializeObject(model);
                        await FileIO.WriteTextAsync(runFile, json);

                        rootFrame.Navigate(typeof(Views.WelcomeView), null);
                    }
                    else
                    {
                        rootFrame.Navigate(typeof(Views.MainFrameView), null);
                    }
                }

                
            }


            Window.Current.Activate();
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
            try
            {
                var lastFile = await StaticContent.LocalFolder.GetFileAsync("LastPlay.json");
                await FileIO.WriteTextAsync(lastFile, json);
            }
            catch
            {
                var lastFile = await StaticContent.LocalFolder.CreateFileAsync("LastPlay.json");
                await FileIO.WriteTextAsync(lastFile, json);
            }
            
            deferral.Complete();
        }

        protected async override void OnFileActivated(FileActivatedEventArgs args)
        {

            OnLaunched(null);
            //var files = args.Files;
            //if (files.Count > 1)
            //{
            //    var playlist = new PlaylistFile()
            //    {
            //        Artist = "Music X",
            //        Cover = "/Assets/Images/now.png",
            //        Id = 1000,
            //        Name = "Сейчас играет",
            //        Tracks = new List<AudioFile>()
            //    };
            //    foreach (var file in files)
            //    {
            //        var audio = await FindMetadataService.ConvertToAudioFile((StorageFile)file);
            //        playlist.Tracks.Add(audio);
            //    }

            //    StaticContent.NowPlayPlaylist = playlist;
            //    StaticContent.OpenFiles = true;
            //}
            //else
            //{
            //    var file = files[0];
            //    var audio = await FindMetadataService.ConvertToAudioFile((StorageFile)file);
            //    StaticContent.NowPlay = audio;
            //    StaticContent.OpenFiles = true;

            //}

            //if (Window.Current.Visible)
            //{
            //    HomeLocalViewModel.Instanse.Page_Loaded(null, null);
            //}
            //else
            //{
            //    DispatcherHelper.Initialize();
            //    var rootFrame = new Frame();
            //    rootFrame.NavigationFailed += OnNavigationFailed;
            //    CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            //    if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.ApplicationView"))
            //    {
            //        var appView = ApplicationView.GetForCurrentView();
            //        appView.TitleBar.ButtonBackgroundColor = Colors.Transparent;
            //        appView.TitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            //    }

            //    StaticContent.LocalFolder = ApplicationData.Current.LocalFolder;
            //    if (await StaticContent.LocalFolder.TryGetItemAsync("Playlists") != null)
            //    {
            //        StaticContent.PlaylistsFolder = await StaticContent.LocalFolder.GetFolderAsync("Playlists");
            //    }

            //    if (await StaticContent.LocalFolder.TryGetItemAsync("Covers") != null)
            //    {
            //        StaticContent.CoversFolder = await StaticContent.LocalFolder.GetFolderAsync("Covers");

            //    }

            //    if (await StaticContent.LocalFolder.TryGetItemAsync("ConfigApp.json") != null)
            //    {
            //        var file = await StaticContent.LocalFolder.GetFileAsync("ConfigApp.json");
            //        var fileString = await FileIO.ReadTextAsync(file);
            //        var config = JsonConvert.DeserializeObject<ConfigApp>(fileString);
            //        StaticContent.Config = config;
            //    }

            //    Window.Current.Content = rootFrame;

            //    if (await StaticContent.LocalFolder.TryGetItemAsync("RunApp.json") == null)
            //    {
            //        var runFile = await StaticContent.LocalFolder.CreateFileAsync("RunApp.json");
            //        var model = new RunApp()
            //        {
            //            CodeName = "Test",
            //            FirstStart = true,
            //            RunUpdate = true
            //        };

            //        var json = JsonConvert.SerializeObject(model);
            //        await FileIO.WriteTextAsync(runFile, json);

            //        rootFrame.Navigate(typeof(Views.WelcomeView), null);
            //    }
            //    else
            //    {
            //        rootFrame.Navigate(typeof(Views.ProVersionView), null);
            //    }

            //    Window.Current.Activate();
            //    SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
            //}

        }
    }
}
