using DryIoc;
using Fooxboy.MusicX.Uwp.Resources.ContentDialogs;
using Fooxboy.MusicX.Uwp.Services;
using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fooxboy.MusicX.Uwp.ViewModels
{
    public class LoginViewModel :BaseViewModel
    {
        public LoginViewModel()
        {
            AuthCommand = new RelayCommand(Auth);
        }
        public string Login { get; set; }
        public string Password { get; set; }
        public bool IsMusicXAccount { get; set; }
        public bool IsLoading { get; set; }
        public string Image { get; set; }
        public bool VisibilityPersonImage { get; set; }
        public bool VisibilityTextBox { get; set; }
        public bool VisibilityLogoImage { get; set; }


        public RelayCommand AuthCommand { get; set; }

        public async void Auth()
        {
            IsLoading = true;
            VisibilityTextBox = false;
            Changed("IsLoading");
            Changed("VisibilityTextBox");

            try
            {
                var api = Container.Get.Resolve<Core.Api>();
                var token = await api.VKontakte.Auth.UserAsync(Login, Password, TwoFactorAuth, null);
                var tokenService = Container.Get.Resolve<TokenService>();
                await tokenService.Save(token);
                var user = await api.VKontakte.Users.Info.CurrentUserAsync();
                Image = user.PhotoUser;
                VisibilityLogoImage = false;
                VisibilityPersonImage = true;
                Changed("VisibilityLogoImage");
                Changed("VisibilityPersonImage");
                Changed("Image");
            }
            catch(VkNet.Exception.VkApiAuthorizationException)
            {
                IsLoading = false;
                VisibilityTextBox = true;
                Changed("IsLoading");
                Changed("VisibilityTextBox");
                //Неверный логин или пароль.
            }
            catch(VkNet.Exception.VkAuthorizationException)
            {
                IsLoading = false;
                VisibilityTextBox = true;
                Changed("IsLoading");
                Changed("VisibilityTextBox");
                //Неверный логин или пароль.
            }
            catch(Exception e)
            {
                IsLoading = false;
                VisibilityTextBox = true;
                Changed("IsLoading");
                Changed("VisibilityTextBox");
                //неизвестная ошибка при логине.
            }
        }

        public static string TwoFactorAuth()
        {
            TwoFactorAuthContentDialog dialog = null;
            DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                var dialogA = new TwoFactorAuthContentDialog();
                dialog = dialogA;
            });

            bool buffer = true;
            var task = Task.Run(async () =>
            {
                while (dialog == null)
                {
                    await Task.Delay(500);
                }

                _ = DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
                  {
                      await dialog.ShowAsync();
                      buffer = false;
                  });

                while (buffer)
                {
                    await Task.Delay(1000);
                }

            });
            task.ConfigureAwait(false);
            task.Wait();
            return dialog.Result;
        }
    }
}
