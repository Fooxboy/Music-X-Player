using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Uwp.Resources.ContentDialogs;
using Fooxboy.MusicX.Uwp.Services.VKontakte;
using Windows.UI.Popups;

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
            LoginCommand = new RelayCommand(async() =>
            {
                if (Login == null || Password == null) await new MessageDialog("Вы не указали логин или пароль").ShowAsync();

                string token = null;

                try
                {
                    token = await Fooxboy.MusicX.Core.VKontakte.Auth.User(Login, Password);
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
                }
            });
        }

        public string Login { get; set; }
        public string Password { get; set; }

        public RelayCommand LoginCommand { get; set; }
    }
}
