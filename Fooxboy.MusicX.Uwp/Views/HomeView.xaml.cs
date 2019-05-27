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
            Window.Current.SetTitleBar(TitleBarGrid);
            HomeViewModel = HomeViewModel.Instanse;
        }

        public HomeViewModel HomeViewModel { get; set; }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".mp3");

            Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
            var audio = await FindMetadataService.Convert(file);
            StaticContent.AudioService.CurrentPlaylist.Add(audio);
            StaticContent.AudioService.CurrentPlaylist.CurrentItem = audio;
            StaticContent.AudioService.Volume = 1f;
            StaticContent.Volume = 1f;
            StaticContent.AudioService.Play();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            await HomeViewModel.GetPlaylistLocal();
            await HomeViewModel.GetMusicLocal();
            int mc = await HomeViewModel.CountMusic();
            this.MusicCount.Text = $"{mc} трека(ов)";
        }
    }
}
