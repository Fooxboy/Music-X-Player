using System;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using DryIoc;
using Fooxboy.MusicX.Core.New.Models;
using Fooxboy.MusicX.Core.New.Services;
using Fooxboy.MusicX.Uwp.Models;
using Fooxboy.MusicX.Uwp.Services;
using Fooxboy.MusicX.Uwp.Views;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using RxMvvm;
using RxMvvm.Navigation;
using Splat.DryIoc;
using VkNet;
using VkNet.Abstractions;
using VkNet.AudioBypassService.Extensions;

namespace Fooxboy.MusicX.Uwp.ViewModels
{
    public class AppBootstrapper : ReactiveObject, IScreen
    {
        public RoutingState Router { get; }

        private readonly IRxNavigationService _rxNavigationService;

        private readonly AuthorizationService _authorizationService;

        public AppBootstrapper()
        {
            Router = new RoutingState();

            Rx.IoCProvier.UseDryIocDependencyResolver();

            RegisterDependencies();

            _rxNavigationService = Rx.IoCProvier.Resolve<IRxNavigationService>();

            _authorizationService = Rx.IoCProvier.Resolve<AuthorizationService>();

            NavigateToFirstViewModel().Subscribe();
        }

        private void RegisterDependencies()
        {
            var services = new ServiceCollection();
            services.AddAudioBypass();
            var vkApi = new VkApi(services);

            Rx.IoCProvier.RegisterInstance<IVkApi>(vkApi);
            Rx.IoCProvier.Register<NotificationService>();
            Rx.IoCProvier.Register<TrackLoaderService>();
            Rx.IoCProvier.Register<AlbumLoaderService>();
            Rx.IoCProvier.Register<DiscordService>();
            Rx.IoCProvier.Register<LoadingService>();
            Rx.IoCProvier.Register<PlayerService>(Reuse.Singleton);
            Rx.IoCProvier.Register<IRxNavigationService, RxNavigationService>();
            Rx.IoCProvier.Register<ISettingsManager<Session>, SessionManager>();
            Rx.IoCProvier.Register<ISettingsManager<AppSettings>, SettingsManager>();
            Rx.IoCProvier.Register<AuthorizationService>();

            Rx.IoCProvier.RegisterInstance<IScreen>(this);
            Rx.IoCProvier.Register<WelcomeViewModel>();
            Rx.IoCProvier.Register<LoginViewModel>();
            Rx.IoCProvier.Register<RootViewModel>();
            Rx.IoCProvier.Register<PlayerViewModel>(Reuse.Singleton);
            Rx.IoCProvier.Register<UserInfoViewModel>();
            Rx.IoCProvier.Register<LoadingViewModel>();
            Rx.IoCProvier.Register<NotificationViewModel>();
            Rx.IoCProvier.Register<HomeViewModel>();
            Rx.IoCProvier.Register<RecommendationsViewModel>();
            Rx.IoCProvier.Register<FavoriteArtistsViewModel>();
            Rx.IoCProvier.Register<DownloadsViewModel>();

            Rx.IoCProvier.Register<IViewFor<AppBootstrapper>, BootsrapperView>();
            Rx.IoCProvier.Register<IViewFor<WelcomeViewModel>, WelcomeView>();
            Rx.IoCProvier.Register<IViewFor<LoginViewModel>, LoginView>();
            Rx.IoCProvier.Register<IViewFor<RootViewModel>, RootView>();
            Rx.IoCProvier.Register<IViewFor<HomeViewModel>, HomeView>();
            Rx.IoCProvier.Register<IViewFor<RecommendationsViewModel>, RecommendationsView>();
            Rx.IoCProvier.Register<IViewFor<FavoriteArtistsViewModel>, FavoriteArtistsView>();
            Rx.IoCProvier.Register<IViewFor<DownloadsViewModel>, DownloadsView>();
        }

        private IObservable<IRoutableViewModel> NavigateToFirstViewModel()
        {
            return _authorizationService.AuthorizeFromSession().ToObservable()
                .ObserveOn(RxApp.MainThreadScheduler)
                .SelectMany(x => x
                    ? _rxNavigationService.Navigate<RootViewModel>(Router)
                    : _rxNavigationService.Navigate<WelcomeViewModel>(Router));
        }
    }
}