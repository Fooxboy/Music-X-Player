using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Push;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.AppCenter.Crashes;
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
            var settings = new AppPrivateSettingsService();

            RequestedTheme = settings.GetTheme();

            InitializeComponent();
            //Suspending += OnSuspending;
            
        }

        //private IContainer _container;

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            if (!(Window.Current.Content is Frame rootFrame))
            {
                rootFrame = new Frame();


                AppCenter.Start("96c77488-34ce-43d0-b0d3-c4b1ce326c7f", typeof(Analytics), typeof(Push), typeof(Crashes));
                AppCenter.LogLevel = LogLevel.Verbose;
                CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;

                var appView = ApplicationView.GetForCurrentView();
                appView.TitleBar.ButtonBackgroundColor = Colors.Transparent;
                appView.TitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;

                var theme = new AppPrivateSettingsService().GetTheme();

                appView.TitleBar.ButtonForegroundColor = theme == ApplicationTheme.Light ? Colors.Black : Colors.White;

                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                    rootFrame.Navigate(typeof(Views.BootsrapperView));
                Window.Current.Activate();
            }
        
            /*if (e.PrelaunchActivated == false)
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
                            rootFrame.Navigate(typeof(Views.RootView), this._container);
                            await _container.Resolve<Api>().VKontakte.Auth.AutoAsync(config.AccessTokenVkontakte, null);
                            await _container.Resolve<Api>().Discord.InitAsync();
                            
                        }
                    }catch
                    {
                        rootFrame.Navigate(typeof(Views.WelcomeView), _container);
                    }
                }
            }*/
            Push.CheckLaunchedFromNotification(e);
        }

      
       /* void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
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

        }*/
    }
}
