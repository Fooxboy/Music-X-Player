using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Uwp.Resources.ContentDialogs;
using Windows.UI.Xaml.Controls;

namespace Fooxboy.MusicX.Uwp.Services.VKontakte
{
    public static class AuthService
    {
        public static string TwoFactorAuth()
        {
            var dialog =  new TwoFactorAuthContentDialog();
            dialog.PrimaryButtonClick += Click;
            string result = null;

            Task<string> a = Task.Run(async () =>
            {
                return await dialog.ShowResult();
            });

            result = a.Result;
            return result;
        }

        public static void Click(ContentDialog dialog, ContentDialogButtonClickEventArgs e) 
        {

        }
    }
}
