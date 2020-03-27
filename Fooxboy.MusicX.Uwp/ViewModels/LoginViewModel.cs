using DryIoc;
using Fooxboy.MusicX.Uwp.Resources.ContentDialogs;
using Fooxboy.MusicX.Uwp.Services;
using Fooxboy.MusicX.Uwp.Views;
using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Fooxboy.MusicX.Uwp.ViewModels
{
    public class LoginViewModel :BaseViewModel
    {
        private IContainer _container;

        public LoginViewModel(IContainer container)
        {
            _container = container;
            AuthCommand = new RelayCommand(Auth);
            VisibilityLogoImage = true;
            VisibilityTextBox = true;
            Login = "";
            Password = "";
            Image = "ms-appx:///Assets/Images/now.png";


        }
        public string Login { get; set; }
        public string Password { get; set; }
        public bool IsMusicXAccount { get; set; }
        public bool IsLoading { get; set; }
        public string Image { get; set; }
        public bool VisibilityPersonImage { get; set; }
        public bool VisibilityTextBox { get; set; }
        public bool VisibilityLogoImage { get; set; }


        public RelayCommand AuthCommand { get; }

        public async void Auth()
        {
            IsLoading = true;
            VisibilityTextBox = false;
            Changed("IsLoading");
            Changed("VisibilityTextBox");

            try
            {
                var api = _container.Resolve<Core.Api>();
                var token = await api.VKontakte.Auth.UserAsync(Login, Password, TwoFactorAuth, null);
                var tokenService = _container.Resolve<TokenService>();
                await tokenService.Save(token);
                var user = await api.VKontakte.Users.Info.CurrentUserAsync();
                Image = user.PhotoUser;
                VisibilityLogoImage = false;
                VisibilityPersonImage = true;
                Image = user.PhotoUser;
                Changed("VisibilityLogoImage");
                Changed("VisibilityPersonImage");
                Changed("Image");


                await Task.Delay(3000);
                var currentFrame = Window.Current.Content as Frame;
                currentFrame?.Navigate(typeof(RootWindow));
            }
            catch(VkNet.AudioBypassService.Exceptions.VkAuthException)
            {
                IsLoading = false;
                VisibilityTextBox = true;
                Changed("IsLoading");
                Changed("VisibilityTextBox");

                await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
                {
                    var dialog = new IncorrectLoginOrPasswordContentDialog();
                    await dialog.ShowAsync();
                });
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
