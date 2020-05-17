using DryIoc;
using Fooxboy.MusicX.Uwp.Services;
using Fooxboy.MusicX.Uwp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Fooxboy.MusicX.Core;
using Fooxboy.MusicX.Uwp.ContentDialogs;
using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Uwp.Helpers;

namespace Fooxboy.MusicX.Uwp.ViewModels
{
    public class NavigationRootViewModel:BaseViewModel
    {
        public bool VisibilitySelectorHome { get; set; }
        public bool VisibilitySelectorRecommendations { get; set; }
        public bool VisibilitySelectorFavoriteArtists { get; set; }
        public bool VisibilitySelectorDownloads { get; set; }
        public RelayCommand GoToHome { get; set; }
        public RelayCommand GoToRecommendations { get; set; }
        public RelayCommand GoToFavoriteArtists { get; set; }
        public RelayCommand GoToDownloads { get; set; }
        private NavigationService _navigationService;
        private IContainer _container;
        private ILoggerService _logger;

        public RelayCommand LogOutCommand { get; set; }

        public RelayCommand OpenAboutCommand { get; set; }
        public RelayCommand OpenSettingsCommand { get; set; }

        public NavigationRootViewModel(IContainer container)
        {
            this._container = container;
            _logger = _container.Resolve<LoggerService>();
            GoToHome = new RelayCommand(ToHome);
            GoToRecommendations = new RelayCommand(ToRecommendations);
            GoToFavoriteArtists = new RelayCommand(ToFavoriteArtists);
            GoToDownloads = new RelayCommand(ToDownloads);
            _navigationService = _container.Resolve<NavigationService>();
            OpenAboutCommand = new RelayCommand(OpenAbout);
            LogOutCommand = new RelayCommand(LogOut);
            OpenSettingsCommand = new RelayCommand(OpenSettings);
        }

        public async void LogOut()
        {
            _logger.Trace("Выход из аккаунта...");
            var tokenSerice = _container.Resolve<TokenService>();
            await tokenSerice.Delete();

            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                Frame rootFrame = Window.Current.Content as Frame;
                rootFrame?.Navigate(typeof(LoginView), null, new DrillInNavigationTransitionInfo());
            });

        }

        public void OpenSettings()
        {
            DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
            {
                var settings = new SettingsContentDialog();
                await settings.ShowAsync();
            });
        }

        public void OpenAbout()
        {
            DispatcherHelper.ExecuteOnUIThreadAsync(async() =>
            {
                var about = new AboutContentDialog();
                await about.ShowAsync();
            });
        }

        public void ChangedSelector()
        {
            if (VisibilitySelectorHome == true)
            {
                VisibilitySelectorRecommendations = false;
                VisibilitySelectorFavoriteArtists = false;
                VisibilitySelectorDownloads = false;
            } else if (VisibilitySelectorRecommendations == true)
            {
                VisibilitySelectorHome = false;
                VisibilitySelectorFavoriteArtists = false;
                VisibilitySelectorDownloads = false;
            } else if (VisibilitySelectorFavoriteArtists == true)
            {
                VisibilitySelectorRecommendations = false;
                VisibilitySelectorDownloads = false;
                VisibilitySelectorHome = false;
            } else if (VisibilitySelectorDownloads == true)
            {
                VisibilitySelectorRecommendations = false;
                VisibilitySelectorHome = false;
                VisibilitySelectorFavoriteArtists = false;
            }

            Changed("VisibilitySelectorHome");
            Changed("VisibilitySelectorRecommendations");
            Changed("VisibilitySelectorFavoriteArtists");
            Changed("VisibilitySelectorDownloads");
        }

        public void ToHome()
        {
            //if (VisibilitySelectorHome == true) return;
            VisibilitySelectorHome = true;
            ChangedSelector();
            _navigationService.Go(typeof(HomeView), _container, 1);
        }

        public void ToRecommendations()
        {
            if (VisibilitySelectorRecommendations == true) return;
            VisibilitySelectorRecommendations = true;
            VisibilitySelectorHome = false;
            this.ChangedSelector();
            var api = _container.Resolve<Api>();
            var player = _container.Resolve<PlayerService>();
            var notification = _container.Resolve<NotificationService>();
            var logger = _container.Resolve<LoggerService>();
            _navigationService.Go(typeof(RecommendationsView), new  object[]{api, player, notification, logger }, 1);

        }

        public void ToFavoriteArtists()
        {
            if (VisibilitySelectorFavoriteArtists == true) return;
            VisibilitySelectorFavoriteArtists = true;
            ChangedSelector();
            _navigationService.Go(typeof(FavoriteArtistsView), null, 1);

        }

        public void ToDownloads()
        {
            if (VisibilitySelectorDownloads == true) return;
            VisibilitySelectorDownloads = true;
            ChangedSelector();
            _navigationService.Go(typeof(DownloadsView), null, 1);

        }
    }
}
