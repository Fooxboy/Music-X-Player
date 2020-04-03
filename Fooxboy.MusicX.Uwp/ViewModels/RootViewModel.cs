using System.Reactive;
using ReactiveUI;
using RxMvvm.Navigation;

namespace Fooxboy.MusicX.Uwp.ViewModels
{
    public class RootViewModel : ReactiveObject, IRoutableViewModel, IScreen
    {
        private readonly IRxNavigationService _navigationService;

        public RootViewModel(IScreen screen, 
            PlayerViewModel playerViewModel,
            UserInfoViewModel userInfoViewModel,
            NotificationViewModel notificationViewModel, 
            IRxNavigationService navigationService)
        {
            HostScreen = screen;
            PlayerViewModel = playerViewModel;
            UserInfoViewModel = userInfoViewModel;
            NotificationViewModel = notificationViewModel;
            _navigationService = navigationService;

            Router = new RoutingState();

            GoToHomeCommand =
                ReactiveCommand.CreateFromObservable(() => _navigationService.Navigate<HomeViewModel>(Router));
            GoToRecommendationsCommand =
	            ReactiveCommand.CreateFromObservable(() => _navigationService.Navigate<RecommendationsViewModel>(Router));
            GoToFavoriteArtistsCommand =
	            ReactiveCommand.CreateFromObservable(() => _navigationService.Navigate<FavoriteArtistsViewModel>(Router));

            GoToHomeCommand.Execute();
        }

        public PlayerViewModel PlayerViewModel { get; set; }

        public UserInfoViewModel UserInfoViewModel { get; set; }

        public NotificationViewModel NotificationViewModel { get; set; }

        public string UrlPathSegment => "root";

        public IScreen HostScreen { get; }

        public RoutingState Router { get; }

        public ReactiveCommand<Unit, IRoutableViewModel> GoToHomeCommand { get; }

        public ReactiveCommand<Unit, IRoutableViewModel> GoToRecommendationsCommand { get; }

        public ReactiveCommand<Unit, IRoutableViewModel> GoToFavoriteArtistsCommand { get; }

        public ReactiveCommand<Unit, Unit> GoToDownloadsCommand { get; }
    }
}
