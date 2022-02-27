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
using Fooxboy.MusicX.Uwp.Models;
using Fooxboy.MusicX.Uwp.ViewModels;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Fooxboy.MusicX.Uwp.Views
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class PlaylistView : Page
    {
        public PlaylistView()
        {
            this.InitializeComponent();
        }

        public PlaylistViewModel ViewModel { get; set; }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            var data = (PlaylistViewNavigationData) e.Parameter;
            ViewModel = new PlaylistViewModel(data?.Container);

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

            if (data?.Album.Genres.Count == 0)
            {
                Genres.Visibility = Visibility.Collapsed;
                Dot2.Visibility = Visibility.Collapsed;
                Dot1.Visibility = Visibility.Collapsed;
            }

            if (data?.Album.Year == 0)
            {
                Year.Visibility = Visibility.Collapsed;

            }

            await ViewModel.StartLoading(data?.Album);

            

            base.OnNavigatedTo(e);

        }

        private void Rectangle_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ShadowRectangle.Width = e.NewSize.Width;
        }

        private async void TracksListView_OnItemClick(object sender, ItemClickEventArgs e)
        {
            var track = (Track) e.ClickedItem;
            await ViewModel.PlayTrack(track);
        }
    }
}
