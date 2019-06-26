using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Fooxboy.MusicX.Uwp.Resources.ContentDialogs;
using Fooxboy.MusicX.Uwp.ViewModels;
using GalaSoft.MvvmLight.Threading;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Fooxboy.MusicX.Uwp.Services.VKontakte
{
    public static class AuthService
    {

        public static bool TwoFactorAuthIsOpen = false;
        public static TwoFactorAuthContentDialog dialog = null;
        public static string TwoFactorAuth()
        {
            DispatcherHelper.CheckBeginInvokeOnUI( () =>
            {
                var dialogA = new TwoFactorAuthContentDialog();
                dialog = dialogA;
            });

            bool buffer = true;
            var task = Task.Run(async () =>
            {
                while(dialog == null)
                {
                    await Task.Delay(500);
                }

                DispatcherHelper.CheckBeginInvokeOnUI(async () =>
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

        public async static Task SetNameAccount()
        {
            var user = await Fooxboy.MusicX.Core.VKontakte.Users.Info.CurrentUser();
            var name = $"{user.FirstName} {user.LastName}";
            var viewmodel = PlayerMenuViewModel.Instanse;
            viewmodel.NameAccount = name;
        }

        public async static Task<string> FirstAuth(string login, string password)
        {
            var token = await Fooxboy.MusicX.Core.VKontakte.Auth.User(login, password, AuthService.TwoFactorAuth, new CaptchaSolver());
            await Fooxboy.MusicX.Core.VKontakte.Auth.Auto(token, new CaptchaSolver());
            await SetNameAccount();
            return token;
        }

        public async static Task AutoAuth()
        {
            var tokenObject = await TokenService.Load();
            await Fooxboy.MusicX.Core.VKontakte.Auth.Auto(tokenObject.Token, new CaptchaSolver());
            await SetNameAccount();
        }

        public async static Task LogOut()
        {
            StaticContent.IsAuth = false;
            PlayerMenuViewModel.Instanse.VkontaktePages = Visibility.Collapsed;
            await TokenService.Delete();
        }

        public async static Task<bool> IsAuth()
        {
            try
            {
                await StaticContent.LocalFolder.GetFileAsync("token.json");
                return true;
            }
            catch
            {
                return false;
            }
            
        }
    }
}
