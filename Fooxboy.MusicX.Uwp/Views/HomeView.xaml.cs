using DryIoc;
using Fooxboy.MusicX.Core;
using Fooxboy.MusicX.Uwp.Models;
using Fooxboy.MusicX.Uwp.ViewModels;
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

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Fooxboy.MusicX.Uwp.Views
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class HomeView : Page
    {
        public HomeView()
        {

            this.InitializeComponent();
        }
        public HomeViewModel ViewModel { get; set; }
        private IContainer _container;

        private void BlockPlaylists_SizeChanged(object sender, SizeChangedEventArgs e)
        {
           BorderShadow.Width = e.NewSize.Width;
        }

        private async void scroll_ViewChanging(object sender, ScrollViewerViewChangingEventArgs e)
        {
            var current = scroll.VerticalOffset;
            var max = scroll.ScrollableHeight;

            //Долистали до конца, загружаем еще треков.
            if(max - current < 80)
            {
                await ViewModel.StartLoadingTracks();
            }

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            if (ViewModel != null) return;
            _container = (IContainer) e.Parameter;
            ViewModel = new HomeViewModel(_container);

            
            base.OnNavigatedTo(e);
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.GetMaxTracks();
            await ViewModel.StartLoadingAlbums();
            await ViewModel.StartLoadingTracks();


            if(ViewModel.Tracks.Count > 0)
            {
                NoTracksGrid.Visibility = Visibility.Collapsed;
                scroll.Visibility = Visibility.Visible;
            }else
            {
                if(ViewModel.Albums.Count == 0)
                {
                    NoTracksGrid.Visibility = Visibility.Visible;
                    scroll.Visibility = Visibility.Collapsed;
                }else
                {
                    NoTracksGrid.VerticalAlignment = VerticalAlignment.Top;
                    NoTracksGrid.Margin = new Thickness(0, 500, 0, 0);
                    NoTracksGrid.Visibility = Visibility.Visible;
                }
            }

        }

        private async void TracksListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var track = (Track)e.ClickedItem;
             ViewModel.PlayTrack(track);
        }
    }
}
