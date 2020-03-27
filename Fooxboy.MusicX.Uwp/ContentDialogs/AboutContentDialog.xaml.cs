using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Диалоговое окно содержимого" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Fooxboy.MusicX.Uwp.ContentDialogs
{
    public sealed partial class AboutContentDialog : ContentDialog
    {
        public AboutContentDialog()
        {
            this.InitializeComponent();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private async void Telegram_OnClick(object sender, RoutedEventArgs e)
        {
            var uri = new Uri(@"tg://resolve?domain=MusicXPlayer");
             await Windows.System.Launcher.LaunchUriAsync(uri);
        }

        private async void Vk_OnClick(object sender, RoutedEventArgs e)
        {
            var uri = new Uri(@"https://vk.com/musicxplayer");
            await Windows.System.Launcher.LaunchUriAsync(uri);
        }
    }
}
