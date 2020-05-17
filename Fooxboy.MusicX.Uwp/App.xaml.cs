using System;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Push;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Services.Maps;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using DryIoc;
using Windows.UI.Xaml.Navigation;
using Flurl.Http;
using Microsoft.AppCenter.Crashes;
using Fooxboy.MusicX.Core;
using Fooxboy.MusicX.Uwp.Services;
using Fooxboy.MusicX.Uwp.Views;
using VkNet.Exception;

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
            var settings =new AppPrivateSettingsService();

            this.RequestedTheme = settings.GetTheme();

            this.InitializeComponent();
            this.Suspending += OnSuspending;
            
        }

        private IContainer _container;

        protected async override void OnLaunched(LaunchActivatedEventArgs e)
        {
            //инициализация контейера
            var c = new DryIoc.Container();
            c.RegisterInstance<Api>(Core.Api.GetApi());
            c.Register<ConfigService>(Reuse.Singleton);
            c.Register<NotificationService>(Reuse.Singleton);
            c.Register<TokenService>(Reuse.Singleton);
            c.Register<TrackLoaderService>(Reuse.Singleton);
            c.Register<AlbumLoaderService>(Reuse.Singleton);
            c.Register<DiscordService>(Reuse.Singleton);
            c.Register<LoadingService>(Reuse.Singleton);
            c.Register<PlayerService>(Reuse.Singleton);
            c.Register<CurrentUserService>(Reuse.Singleton);
            c.Register<LoggerService>(Reuse.Singleton);

            this._container = c;

            Container.SetContainer(this._container);

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


                var theme = new AppPrivateSettingsService().GetTheme();

                if(theme == ApplicationTheme.Light) appView.TitleBar.ButtonForegroundColor = Colors.Black;
                else appView.TitleBar.ButtonForegroundColor = Colors.White;


                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    var configService = _container.Resolve<ConfigService>();
                    try
                    {
                        var config = await configService.GetConfig();
                        if (config.AccessTokenVkontakte is null) rootFrame.Navigate(typeof(Views.LoginView), _container);
                        else
                        {
                            rootFrame.Navigate(typeof(Views.RootWindow), this._container);
                            try
                            {
                                await _container.Resolve<Api>().VKontakte.Auth
                                    .AutoAsync(config.AccessTokenVkontakte, null);

                            }
                            catch (FlurlHttpException)
                            {
                                rootFrame.Navigate(typeof(ErrorPage), "Нет доступа к интернету.");
                            }
                            catch (VkApiException)
                            {
                                var tokenService = _container.Resolve<TokenService>();
                                await tokenService.Delete();
                                rootFrame.Navigate(typeof(LoginView));
                            }
                            catch (Exception ee)
                            {
                                rootFrame.Navigate(typeof(ErrorPage), $"Ошибка: {ee.Message}");
                            }
                            //await _container.Resolve<Api>().Discord.InitAsync();

                        }
                    }catch
                    {
                        rootFrame.Navigate(typeof(Views.WelcomeView), _container);
                    }
                }
            }
            Window.Current.Activate();
            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
            Push.CheckLaunchedFromNotification(e);
        }

      
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            //throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            var navigationService = _container.Resolve<NavigationService>();
            navigationService.Back();
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
