using DryIoc;
using Fooxboy.MusicX.Core;
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
            ViewModel = new HomeViewModel();

            this.InitializeComponent();
        }
        public HomeViewModel ViewModel { get; set; }

        private void BlockPlaylists_SizeChanged(object sender, SizeChangedEventArgs e)
        {
           BorderShadow.Width = e.NewSize.Width;
        }

        private async void scroll_ViewChanging(object sender, ScrollViewerViewChangingEventArgs e)
        {
            var current = scroll.VerticalOffset;
            var max = scroll.ScrollableHeight;

            //Долистали до конца, загружаем еще треков.
            if(max - current < 30)
            {
                await ViewModel.StartLoadingTracks();
            }

        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.GetMaxTracks();
            await ViewModel.StartLoadingTracks();
        }
    }
}
