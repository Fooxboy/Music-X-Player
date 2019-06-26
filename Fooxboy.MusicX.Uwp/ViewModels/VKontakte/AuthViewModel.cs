using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Uwp.Resources.ContentDialogs;
using Fooxboy.MusicX.Uwp.Services;
using Fooxboy.MusicX.Uwp.Services.VKontakte;
using Fooxboy.MusicX.Uwp.Views.VKontakte;
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

                if (Login == null || Password == null)
                {
                    await new MessageDialog("Вы не указали логин или пароль").ShowAsync();
                    IsActiveProgressRing = false;
                    VisibilityButton = Visibility.Visible;
                    Changed("IsActiveProgressRing");
                    Changed("VisibilityButton");
                    return;
                }

                string token = null;

                try
                {
                    token = await AuthService.FirstAuth(Login, Password);                
                }
                catch (VkNet.Exception.UserAuthorizationFailException)
                {
                    await ContentDialogService.Show(new IncorrectLoginOrPasswordContentDialog());
                }
                catch (VkNet.Exception.VkAuthorizationException)
                {
                    await ContentDialogService.Show(new IncorrectLoginOrPasswordContentDialog());
                }
                catch (VkNet.Exception.VkApiAuthorizationException)
                {
                    await ContentDialogService.Show(new IncorrectLoginOrPasswordContentDialog());
                }
                catch (VkNet.Exception.UserDeletedOrBannedException)
                {
                    await ContentDialogService.Show(new IncorrectLoginOrPasswordContentDialog());
                }
                catch (Flurl.Http.FlurlHttpException)
                {
                    await ContentDialogService.Show(new ErrorConnectContentDialog());
                }catch(Exception e)
                {
                    await ContentDialogService.Show(new ExceptionDialog("Неизвестная ошибка авторизации", "", e));
                }

                if(token != null)
                {
                    try
                    {
                        PlayerMenuViewModel.Instanse.VkontaktePages = Visibility.Visible;
                        await TokenService.Save(token);
                        StaticContent.IsAuth = true;
                        StaticContent.CurrentSessionIsAuth = true;
                        StaticContent.NavigationContentService.Go(typeof(HomeView));
                    }catch(Exception e)
                    {
                        await ContentDialogService.Show(new ExceptionDialog("Неизвестная ошибка после авторизации", "Возможно, ошибка навигации или сохранения", e));
                        PlayerMenuViewModel.Instanse.VkontaktePages = Visibility.Collapsed;
                        StaticContent.IsAuth = false;
                        StaticContent.CurrentSessionIsAuth = false;
                    }
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
