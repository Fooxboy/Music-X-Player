using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Uwp.Models;
using Fooxboy.MusicX.Uwp.Services.VKontakte;
using Windows.UI.Xaml;

namespace Fooxboy.MusicX.Uwp.ViewModels.VKontakte
{
    public class DownloadsViewModel : BaseViewModel
    {

        private static DownloadsViewModel instanse;

        public static DownloadsViewModel Instanse
        {
            get
            {
                if (instanse == null) instanse = new DownloadsViewModel();

                return instanse;
            }
        }

        public DownloaderService Service { get; set; }

        private DownloadsViewModel()
        {
            Service = DownloaderService.GetService;
            Service.CurrentDownloadFileChanged += Service_CurrentDownloadFileChanged;
            Service.DownloadProgressChanged += Service_DownloadProgressChanged;
            Service.DownloadQueueComplete += Service_DownloadQueueComplete;
            //Service.DownloadComplete += Service_DownloadComplete;

            if (Service.CurrentDownloadTrack != null)
            {
                CurrentDownloadFile = Service.CurrentDownloadTrack;
                Changed("CurrentDownloadFile");
                MaximumValue = Service.Maximum;
                VisibilityNoNowDownload = Visibility.Collapsed;
                VisibilityNowDownload = Visibility.Visible;
                Changed("VisibilityNoNowDownload");
                Changed("VisibilityNowDownload");
                Changed("TitleString");
                Changed("MaximumValue");
                Changed("ArtistString");
                Changed("AlbumString");
                Changed("YearAlbumString");
            } else
            {
                VisibilityNoNowDownload = Visibility.Visible;
                VisibilityNowDownload = Visibility.Collapsed;
                Changed("VisibilityNoNowDownload");
                Changed("VisibilityNowDownload");
            }
        }

        public double MaximumValue { get; set; }
        public double CurrentValue { get; set; }
        public DownloadAudioFile CurrentDownloadFile {get;set;}

        private void Service_DownloadComplete(object sender, EventArgs e)
        {
            if (Service.CurrentDownloadTrack != null)
            {
               
                VisibilityNoNowDownload = Visibility.Collapsed;
                VisibilityNowDownload = Visibility.Visible;
                MaximumValue = Service.Maximum;
                Changed("VisibilityNoNowDownload");
                Changed("VisibilityNowDownload");
                Changed("TitleString");
                Changed("ArtistString");
                Changed("AlbumString");
                Changed("YearAlbumString");
                Changed("MaximumValue");
                Changed("CoverString");
            }
            else
            {
                VisibilityNoNowDownload = Visibility.Visible;
                VisibilityNowDownload = Visibility.Collapsed;
                Changed("VisibilityNoNowDownload");
                Changed("VisibilityNowDownload");
            }
        }

        private void Service_DownloadQueueComplete(object sender, EventArgs e)
        {
            VisibilityNoNowDownload = Visibility.Visible;
            VisibilityNowDownload = Visibility.Collapsed;
            Changed("VisibilityNoNowDownload");
            Changed("VisibilityNowDownload");
        }

        private void Service_DownloadProgressChanged(object sender, ulong e)
        {
            CurrentValue = e;
            Changed("CurrentValue");
        }

        private void Service_CurrentDownloadFileChanged(object sender, EventArgs e)
        {
            if (Service.CurrentDownloadTrack != null)
            {
                CurrentDownloadFile = Service.CurrentDownloadTrack;
                Changed("CurrentDownloadFile");

                MaximumValue = Service.Maximum;
                Changed("MaximumValue");

                VisibilityNoNowDownload = Visibility.Collapsed;
                Changed("VisibilityNoNowDownload");

                VisibilityNowDownload = Visibility.Visible;
                Changed("VisibilityNowDownload");
            }
            else
            {
                VisibilityNoNowDownload = Visibility.Visible;
                VisibilityNowDownload = Visibility.Collapsed;
                Changed("VisibilityNoNowDownload");
                Changed("VisibilityNowDownload");
            }
        }

        public Visibility VisibilityNoNowDownload { get; set; }

        public Visibility VisibilityNowDownload { get; set; }
    }
}
