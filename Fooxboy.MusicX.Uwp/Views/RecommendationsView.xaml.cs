using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
    public sealed partial class RecommendationsView : Page
    {
        public RecommendationsView()
        {
            this.InitializeComponent();
        }

        public RecommendationsViewModel ViewModel { get; set; }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var parameter = (object[]) e.Parameter;

            var api = (Api)parameter[0];
            var player = (PlayerService) parameter[1];
            var notification = (NotificationService) parameter[2];
            var logger = (LoggerService) parameter[3];
            ViewModel = new RecommendationsViewModel(api, player, notification, logger);

            base.OnNavigatedTo(e);
        }

        private void RecommendationsView_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.BorderShadow.Width = e.NewSize.Width;
        }

        private async void RecommendationsView_OnLoaded(object sender, RoutedEventArgs e)
        {
            if(ViewModel.Blocks.Count == 0) await ViewModel.StartLoading();


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
        }
    }
}
