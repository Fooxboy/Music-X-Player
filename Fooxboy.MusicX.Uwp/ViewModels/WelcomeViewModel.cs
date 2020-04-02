using System;
using Fooxboy.MusicX.Uwp.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Reactive;
using System.Threading.Tasks;
using Windows.Storage;
using Fooxboy.MusicX.Core.New.Services;
using RxMvvm.Navigation;

namespace Fooxboy.MusicX.Uwp.ViewModels
{
    public class WelcomeViewModel : ReactiveObject, IRoutableViewModel
    {
        private readonly ISettingsManager<AppSettings> _appSettingsManager;
        private readonly IRxNavigationService _rxNavigationService;

        public WelcomeViewModel(IScreen screen, ISettingsManager<AppSettings> appSettingsManager,
            IRxNavigationService rxNavigationService)
        {
            HostScreen = screen;
            _appSettingsManager = appSettingsManager;
            _rxNavigationService = rxNavigationService;

            StartCommand = ReactiveCommand.CreateFromTask(Start);
        }

        public string UrlPathSegment => "welcome";

        public IScreen HostScreen { get; }

        [Reactive]
        public bool Loading { get; set; }

        public ReactiveCommand<Unit, Unit> StartCommand { get; }

        private async Task Start()
        {
            Loading = true;

            var appSettings = await _appSettingsManager.LoadAsync();

            if (appSettings == null)
            {
                appSettings = new AppSettings
                {
                    IsRateMe = false,
                    SaveImageToCache = true,
                    SaveTracksToCache = false,
                    StreamMusic = false,
                    ThemeApp = 0
                };
                await _appSettingsManager.SaveAsync(appSettings);
            }

            Loading = false;

            var settings = ApplicationData.Current.LocalSettings;
            settings.Values["themeApp"] = appSettings.ThemeApp;

            _rxNavigationService.Navigate<LoginViewModel>(HostScreen.Router).Subscribe();
        }
    }
}