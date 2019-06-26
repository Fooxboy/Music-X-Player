using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Uwp.Resources.ContentDialogs;
using GalaSoft.MvvmLight.Threading;
using VkNet.Utils.AntiCaptcha;
using Windows.UI.Popups;

namespace Fooxboy.MusicX.Uwp.Services.VKontakte
{
    public class CaptchaSolver : ICaptchaSolver
    {
        public void CaptchaIsFalse()
        {
            DispatcherHelper.CheckBeginInvokeOnUI(async () =>
            {
                await new MessageDialog("Вы ввели неверный код с картинки", "Попробуйте ещё раз").ShowAsync();
            });
        }

        public string Solve(string url)
        {
            CaptchaContentDialog dialog= null;
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                var dialogA = new CaptchaContentDialog();
                dialog = dialogA;
            });

            bool buffer = true;
            var task = Task.Run(async () =>
            {
                while (dialog == null)
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
    }
}
