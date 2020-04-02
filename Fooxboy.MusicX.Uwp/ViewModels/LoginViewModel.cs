using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Fooxboy.MusicX.Core.New.Services;
using RxMvvm.Navigation;
using VkNet.AudioBypassService.Exceptions;

namespace Fooxboy.MusicX.Uwp.ViewModels
{
    public class LoginViewModel : ReactiveObject, IRoutableViewModel
    {
        private readonly AuthorizationService _authorizationService;
        private readonly IRxNavigationService _rxNavigationService;

        public LoginViewModel(IScreen screen, AuthorizationService authorizationService,
            IRxNavigationService rxNavigationService)
        {
            HostScreen = screen;
            _authorizationService = authorizationService;
            _rxNavigationService = rxNavigationService;

            TwoFactorInteraction = new Interaction<Unit, string>();

            AuthCommand = ReactiveCommand.CreateFromTask(Auth);
        }

        [Reactive]
        public string Login { get; set; }

        [Reactive]
        public string Password { get; set; }

        [Reactive]
        public bool IsLoading { get; set; }

        public Interaction<Unit, string> TwoFactorInteraction;

        public IScreen HostScreen { get; }

        public string UrlPathSegment => "login";

        public ReactiveCommand<Unit, Unit> AuthCommand { get; }

        public async Task Auth()
        {
            IsLoading = true;

            try
            {
                await _authorizationService.AuthorizeAsync(Login, Password, null, TwoFactorAuthAsync);

                _rxNavigationService.Navigate<RootViewModel>(HostScreen.Router).Subscribe();
            }
            catch (VkAuthException)
            {
                // TODO: handle exception
                IsLoading = false;
            }
            catch (Exception e)
            {
                // TODO: handle exception
                IsLoading = false;
            }
        }

        public async Task<string> TwoFactorAuthAsync()
        {
            return await TwoFactorInteraction.Handle(Unit.Default);
        }
    }
}