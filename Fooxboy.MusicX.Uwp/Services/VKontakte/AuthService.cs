using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Uwp.Resources.ContentDialogs;
using Fooxboy.MusicX.Uwp.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Fooxboy.MusicX.Uwp.Services.VKontakte
{
    public static class AuthService
    {
        public static string TwoFactorAuth()
        {
            var dialog = new TwoFactorAuthContentDialog();
            dialog.PrimaryButtonClick += (d, e) => { };
            string result = null;

            Task<string> a = Task.Run(async () =>
            {
                return await dialog.ShowResult();
            });

            result = a.Result;
            return result;
        }

        public async static Task AutoAuth()
        {

        }

        public async static Task LogOut()
        {
            StaticContent.IsAuth = false;
            PlayerMenuViewModel.Instanse.VkontaktePages = Visibility.Collapsed;
            await TokenService.Delete();
        }

        public async static Task<bool> IsAuth() => await StaticContent.LocalFolder.TryGetItemAsync("token.json") != null;
    }
}
