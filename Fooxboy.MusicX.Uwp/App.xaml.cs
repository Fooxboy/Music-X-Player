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
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Push;
using Newtonsoft.Json;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.Storage;
using Windows.System;
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
using Microsoft.AppCenter.Crashes;

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

            try
            {
                var composite = (ApplicationDataCompositeValue)settings.Values["themeApp"];

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
            }
            catch
            {
                var theme = (int)settings.Values["themeApp"];
                if (theme == 0) this.RequestedTheme = ApplicationTheme.Light;
                else this.RequestedTheme = ApplicationTheme.Dark;
            }
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            
        }

        protected async override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null)
            {
                rootFrame = new Frame();
                rootFrame.NavigationFailed += OnNavigationFailed;
                if(e != null)
                {
                    if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                    {
                        //TODO: Загрузить состояние из ранее приостановленного приложения
                    }
                }
                AppCenter.Start("96c77488-34ce-43d0-b0d3-c4b1ce326c7f", typeof(Analytics), typeof(Push), typeof(Crashes));
                AppCenter.LogLevel = LogLevel.Verbose;
                CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
                
                var appView = ApplicationView.GetForCurrentView();
                appView.TitleBar.ButtonBackgroundColor = Colors.Transparent;
                appView.TitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
                var settings = ApplicationData.Current.LocalSettings;

                try
                {
                    var composite = (ApplicationDataCompositeValue)settings.Values["themeApp"];

                    if (composite == null) appView.TitleBar.ButtonForegroundColor = Colors.Black;
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
                    if (theme == 0) appView.TitleBar.ButtonForegroundColor = Colors.Black;
                    else appView.TitleBar.ButtonForegroundColor = Colors.White;
                }

                Window.Current.Content = rootFrame;
            }
            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    rootFrame.Navigate(typeof(Views.RootWindow), null);
                }
            }
            Window.Current.Activate();
            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
            Push.CheckLaunchedFromNotification(e);
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
            //StaticContent.NavigationContentService.Back();
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
            deferral.Complete();
        }

        protected override void OnActivated(IActivatedEventArgs args)
        {
            if (args.Kind == ActivationKind.Protocol)
            {
                ProtocolActivatedEventArgs eventArgs = args as ProtocolActivatedEventArgs;
                // TODO: Handle URI activation
                // The received URI is eventArgs.Uri.AbsoluteUri
            }
        }
        protected async override void OnFileActivated(FileActivatedEventArgs args)
        {

        }
    }
}
