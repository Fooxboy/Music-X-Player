using System;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Push;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using DryIoc;
using Windows.UI.Xaml.Navigation;
using Microsoft.AppCenter.Crashes;
using Fooxboy.MusicX.Core;
using Fooxboy.MusicX.Uwp.Services;

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
            //инициализация контейера
            var c = new DryIoc.Container();
            c.RegisterInstance<Api>(Core.Api.GetApi());
            c.Register<ConfigService>();
            c.Register<TokenService>(made: Made.Of(() => new TokenService(Arg.Of<ConfigService>())));
            c.Register<PlayerService>();
            Container.SetContainer(c);

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
                }
                catch
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
                    var configService = Container.Get.Resolve<ConfigService>();
                    try
                    {
                        var config = await configService.GetConfig();
                        if (config.AccessTokenVkontakte is null) rootFrame.Navigate(typeof(Views.LoginView), null);
                        else rootFrame.Navigate(typeof(Views.RootWindow), null);
                    }catch
                    {
                        rootFrame.Navigate(typeof(Views.WelcomeView), null);
                    }
                   

                }
            }
            Window.Current.Activate();
            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
            Push.CheckLaunchedFromNotification(e);
        }

      
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            //StaticContent.NavigationContentService.Back();
        }

        
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
