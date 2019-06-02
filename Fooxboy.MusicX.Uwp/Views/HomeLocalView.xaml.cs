using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Fooxboy.MusicX.Uwp.Services;
using Fooxboy.MusicX.Uwp.ViewModels;
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
    public sealed partial class HomeLocalView : Page
    {
        public HomeLocalView()
        {
            this.InitializeComponent();
            HomeViewModel = HomeLocalViewModel.Instanse;
        }

        public HomeLocalViewModel HomeViewModel { get; set; }


        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (HomeViewModel.Playlists.Count == 0)
            {
                await PlaylistsService.SetPlaylistLocal();
            }

            if (HomeViewModel.Music.Count == 0)
            {
                await MusicFilesService.GetMusicLocal();
                HomeViewModel.CountMusic();
            }

        }
    }
}
