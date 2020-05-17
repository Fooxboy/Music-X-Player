﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using DryIoc;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Fooxboy.MusicX.Uwp.Services;

// Документацию по шаблону элемента "Диалоговое окно содержимого" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Fooxboy.MusicX.Uwp.ContentDialogs
{
    public sealed partial class SettingsContentDialog : ContentDialog
    {
        public SettingsContentDialog()
        {
            this.InitializeComponent();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private async void LightTheme(object sender, RoutedEventArgs e)
        {
            var c = Container.Get;
            var settigns = c.Resolve<AppPrivateSettingsService>();
            settigns.Set("ThemeApp", 0);
            var d = new MessageDialog("Перезапустите приложение, чтобы применить тему.");
            await d.ShowAsync();
        }

        private async void DarkTheme(object sender, RoutedEventArgs e)
        {
            var c = Container.Get;
            var settigns = c.Resolve<AppPrivateSettingsService>();
            settigns.Set("ThemeApp", 1);
            var d = new MessageDialog("Перезапустите приложение, чтобы применить тему.");
            await d.ShowAsync();
        }

        private void SettingsContentDialog_OnLoaded(object sender, RoutedEventArgs e)
        {
            var c = Container.Get;
            var settigns = c.Resolve<AppPrivateSettingsService>();
            var theme = settigns.GetTheme();
            if (theme == ApplicationTheme.Light)
            {
                ButtonDark.IsChecked = false;
                ButtonLight.IsChecked = true;
            }
            else
            {
                ButtonDark.IsChecked = true;
                ButtonLight.IsChecked = false;
            }
            //throw new NotImplementedException();
        }
    }
}
