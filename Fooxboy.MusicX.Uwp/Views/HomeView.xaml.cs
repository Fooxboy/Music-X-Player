using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Fooxboy.MusicX.Core;
using Fooxboy.MusicX.Uwp.Services;
using Fooxboy.MusicX.Uwp.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Microsoft.Toolkit.Uwp.UI.Animations;
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
            //Window.Current.SetTitleBar(TitleBarGrid);
            HomeViewModel = HomeViewModel.Instanse;
        }

        public HomeViewModel HomeViewModel { get; set; }


        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if(HomeViewModel.Playlists.Count == 0)
            {
                await PlaylistsService.SetPlaylistLocal();
            }

            if(HomeViewModel.Music.Count == 0)
            {
                await MusicFilesService.GetMusicLocal();
                await HomeViewModel.CountMusic();
            }
   
        }


        private async void CoverPlaylist_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            var image = (Image)sender;
            await image.Scale(
                        scaleX: 1.1f,
                        scaleY: 1.1f,
                        centerX: 50f,
                        centerY: 50f,
                        duration: 350, delay: 0).StartAsync();
        }

        private async void CoverPlaylist_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            var image = (Image)sender;
            await image.Scale(
                        scaleX: 1.0f,
                        scaleY: 1.0f,
                        centerX: 50f,
                        centerY: 50f,
                        duration: 350, delay: 0).StartAsync();
        }
    }
}
