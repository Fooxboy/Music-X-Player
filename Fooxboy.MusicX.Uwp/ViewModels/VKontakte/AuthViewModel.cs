using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Uwp.Resources.ContentDialogs;
using Fooxboy.MusicX.Uwp.Services.VKontakte;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace Fooxboy.MusicX.Uwp.ViewModels.VKontakte
{
    public class AuthViewModel:BaseViewModel
    {
        private static AuthViewModel instanse;

        public static AuthViewModel Instanse
        {
            get
            {
                if (instanse is null) instanse = new AuthViewModel();

                return instanse;
            }
        }

        private AuthViewModel()
        {
            IsActiveProgressRing = false;
            VisibilityButton = Visibility.Visible;
            LoginCommand = new RelayCommand(async() =>
            {
                IsActiveProgressRing = true;
                VisibilityButton = Visibility.Collapsed;
                Changed("IsActiveProgressRing");
                Changed("VisibilityButton");

                if (Login == null || Password == null) await new MessageDialog("Вы не указали логин или пароль").ShowAsync();

                string token = null;

                try
                {
                    token = await Fooxboy.MusicX.Core.VKontakte.Auth.User(Login, Password,  AuthService.TwoFactorAuth);
                } catch (VkNet.Exception.UserAuthorizationFailException e)
                {
                    await new ExceptionDialog("Невозможно войти в аккаунт", "Возможно, логин или пароль не верный", e).ShowAsync();
                } catch (VkNet.Exception.VkAuthorizationException e)
                {
                    await new ExceptionDialog("Невозможно войти в аккаунт", "Возможно, логин или пароль не верный", e).ShowAsync();
                }
                catch (VkNet.Exception.VkApiAuthorizationException e)
                {
                    await new ExceptionDialog("Невозможно войти в аккаунт", "Возможно, логин или пароль не верный", e).ShowAsync();
                }
                catch (VkNet.Exception.UserDeletedOrBannedException e)
                {
                    await new ExceptionDialog("Невозможно войти в аккаунт", "Аккаунт удалён или заблокирован", e).ShowAsync();
                }
                catch (Exception e)
                {
                    await new ExceptionDialog("Невозможно войти в аккаунт", "Неизвестная ошибка входа", e).ShowAsync();
                }

                if(token != null)
                {
                    await TokenService.Save(token);
                    //TODO: навигация на домашний экран.
                }else
                {
                    IsActiveProgressRing = false;
                    VisibilityButton = Visibility.Visible;
                    Changed("IsActiveProgressRing");
                    Changed("VisibilityButton");
                }
            });
        }

        public string Login { get; set; }
        public string Password { get; set; }

        public RelayCommand LoginCommand { get; set; }
        public bool IsActiveProgressRing { get; set; }
        public Visibility VisibilityButton { get; set; }
    }
}
