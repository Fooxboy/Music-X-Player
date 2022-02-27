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
using Fooxboy.MusicX.Core;
using Fooxboy.MusicX.Uwp.Services;
using Fooxboy.MusicX.Uwp.ViewModels;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Fooxboy.MusicX.Uwp.Views
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class ArtistView : Page
    {
        public ArtistViewModel ViewModel { get; set; }
        public ArtistView()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            var data = (object[])e.Parameter;
            var api = (Api) data[0];
            var player = (PlayerService) data[3];
            BackgroundImage.ImageExOpened += BackgroundImage_ImageExOpened;

            var theme = Application.Current.RequestedTheme;

            if (theme == ApplicationTheme.Light)
            {
                playblack.Visibility = Visibility.Visible;
                playwhite.Visibility = Visibility.Collapsed;
            }
            else
            {
                playblack.Visibility = Visibility.Collapsed;
                playwhite.Visibility = Visibility.Visible;
            }

            var notification = (NotificationService) data[1];
            var artistId = Int64.Parse( (string)data[2]);
            var logger = (LoggerService) data[4];

            ViewModel = new ArtistViewModel(api, notification, player, logger);
            await ViewModel.StartLoading(artistId);
            base.OnNavigatedTo(e);
        }

        private void BackgroundImage_ImageExOpened(object sender, Microsoft.Toolkit.Uwp.UI.Controls.ImageExOpenedEventArgs e)
        {
            BackgroundName.Width = (ButtonsPanel.ActualWidth + 25);
            var heigh = BackgroundImage.ActualHeight;
            NameGrid.Margin = new Thickness(0, heigh - 250, 0, 60);
            //throw new NotImplementedException();
        }

        private void ArtistView_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var heigh = BackgroundImage.ActualHeight;
            NameGrid.Margin = new Thickness(0, heigh - 150, 0, 60);
        }

        private double maxOffset = 0;
        private bool isLoading = false;
        private void ForegroundGrid_OnViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (isLoading) return;
            isLoading = true;
            var current = ForegroundScroll.VerticalOffset;
            
            if (current > maxOffset)
            {
                if (Blur.Opacity > 1)
                {
                    isLoading = false;
                    return;
                }
                Blur.Opacity += 0.05;
            }
            else
            {
                if (Blur.Opacity < 0)
                {
                    isLoading = false;
                    return;
                    
                }
                Blur.Opacity -= 0.05;
            }

            maxOffset = current;

            isLoading = false;
            //throw new NotImplementedException();
        }
    }
}
