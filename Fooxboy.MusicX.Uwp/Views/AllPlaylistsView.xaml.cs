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
using DryIoc;
using Fooxboy.MusicX.Uwp.Models;
using Fooxboy.MusicX.Uwp.ViewModels;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Fooxboy.MusicX.Uwp.Views
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class AllPlaylistsView : Page
    {

        public AllPlaylistsViewModel ViewModel { get; set; }
        private IContainer _container;

        public AllPlaylistsView()
        {
            this.InitializeComponent();
            
        }

        private void AllPlaylistsView_OnLoaded(object sender, RoutedEventArgs e)
        {
            
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            var param = (AllPlaylistsModel) e.Parameter;

            _container = param.Container;

            ViewModel = new AllPlaylistsViewModel(_container);
            await ViewModel.StartLoading(param);

            base.OnNavigatedTo(e);
        }

        private async void ScrollViewer_OnViewChanging(object sender, ScrollViewerViewChangingEventArgs e)
        {
            var current = scroll.VerticalOffset;
            var max = scroll.ScrollableHeight;

            //Долистали до конца, загружаем еще плейлистов.
            if (max - current < 80)
            {
                await ViewModel.LoadMore();
            }
        }
    }
}
