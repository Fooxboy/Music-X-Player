using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Fooxboy.MusicX.Uwp.Models;
using Fooxboy.MusicX.Uwp.Services;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Диалоговое окно содержимого" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Fooxboy.MusicX.Uwp.Resources.ContentDialogs
{
    public sealed partial class EditPlaylistContentDialog : ContentDialog
    {
        public EditPlaylistContentDialog(PlaylistFile playlist)
        {
            this.InitializeComponent();
            Playlist = playlist;
        }

        public PlaylistFile Playlist { get; set; }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Playlist.Name = NamePlaylistBox.Text;
            await PlaylistsService.SavePlaylist(Playlist);
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private async void CoverPlaylist_Click(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".png");

            Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                StorageFile cover;
                try
                {
                    cover = await file.CopyAsync(StaticContent.CoversFolder);

                }
                catch
                {
                    cover = await StaticContent.CoversFolder.GetFileAsync(file.Name);
                    await file.CopyAndReplaceAsync(cover);
                }

                Playlist.Cover = cover.Path;
                CoverImage.UriSource = new Uri(Playlist.Cover);
            }
            else
            {

            }
        }

        private void Rectangle_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            CameraIcon.Visibility = Visibility.Visible;
            CameraIconBackground.Visibility = Visibility.Visible;
        }

        private void Rectangle_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            CameraIcon.Visibility = Visibility.Collapsed;
            CameraIconBackground.Visibility = Visibility.Collapsed;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
