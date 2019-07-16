using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Fooxboy.MusicX.Uwp.Models;
using Fooxboy.MusicX.Uwp.Services.VKontakte;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class QueueDownloadContentDialog : ContentDialog
    {
        public QueueDownloadContentDialog(List<DownloadAudioFile> tracks)
        {

            Tracks = new ObservableCollection<DownloadAudioFile>(tracks);
            this.InitializeComponent();

        }

        public ObservableCollection<DownloadAudioFile> Tracks { get; set; }


        public DownloadAudioFile SelectAudioFile { get; set; }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(SelectAudioFile != null)
            {
                var service = DownloaderService.GetService;
                service.QueueTracks.Remove(SelectAudioFile);
            }
        }
    }
}
