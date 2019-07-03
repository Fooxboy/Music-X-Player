using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Uwp.Services.VKontakte;
using Windows.UI.Xaml;

namespace Fooxboy.MusicX.Uwp.ViewModels.VKontakte
{
    public class DownloadsViewModel:BaseViewModel
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
            Service.DownloadComplete += Service_DownloadComplete;

            if(Service.CurrentDownloadTrack != null)
            {
                Title = Service.CurrentDownloadTrack.Title;
                Artist = Service.CurrentDownloadTrack.Artist;
                Album = Service.CurrentDownloadTrack.AlbumName;
                YearAlbum = Service.CurrentDownloadTrack.AlbumYear;
                VisibilityNoNowDownload = Visibility.Collapsed;
                VisibilityNowDownload = Visibility.Visible;
                Changed("VisibilityNoNowDownload");
                Changed("VisibilityNowDownload");
                Changed("Title");
                Changed("Artist");
                Changed("Album");
                Changed("YearAlbum");
            }else
            {
                VisibilityNoNowDownload = Visibility.Visible;
                VisibilityNowDownload = Visibility.Collapsed;
                Changed("VisibilityNoNowDownload");
                Changed("VisibilityNowDownload");
            }
        }


        public string Title { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string YearAlbum { get; set; }
        public string Cover { get; set; }
        public ulong Maximum { get; set; }
        public ulong CurrentValue { get; set; }

        private void Service_DownloadComplete(object sender, EventArgs e)
        {
            if (Service.CurrentDownloadTrack != null)
            {
                Title = Service.CurrentDownloadTrack.Title;
                Artist = Service.CurrentDownloadTrack.Artist;
                Album = Service.CurrentDownloadTrack.AlbumName;
                YearAlbum = Service.CurrentDownloadTrack.AlbumYear;
                Cover = Service.CurrentDownloadTrack.Cover;
                VisibilityNoNowDownload = Visibility.Collapsed;
                VisibilityNowDownload = Visibility.Visible;
                Maximum = Service.Maximum;
                Changed("VisibilityNoNowDownload");
                Changed("VisibilityNowDownload");
                Changed("Title");
                Changed("Artist");
                Changed("Album");
                Changed("YearAlbum");
                Changed("Maximum");
                Changed("Cover");
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
            if (Service.CurrentDownloadTrack != null)
            {
                Title = Service.CurrentDownloadTrack.Title;
                Artist = Service.CurrentDownloadTrack.Artist;
                Album = Service.CurrentDownloadTrack.AlbumName;
                YearAlbum = Service.CurrentDownloadTrack.AlbumYear;
                Cover = Service.CurrentDownloadTrack.Cover;
                VisibilityNoNowDownload = Visibility.Collapsed;
                VisibilityNowDownload = Visibility.Visible;
                Maximum = Service.Maximum;
                Changed("VisibilityNoNowDownload");
                Changed("VisibilityNowDownload");
                Changed("Title");
                Changed("Artist");
                Changed("Album");
                Changed("YearAlbum");
                Changed("Maximum");
                Changed("Cover");

            }
            else
            {
                VisibilityNoNowDownload = Visibility.Visible;
                VisibilityNowDownload = Visibility.Collapsed;
                Changed("VisibilityNoNowDownload");
                Changed("VisibilityNowDownload");
            }
        }

        private void Service_DownloadProgressChanged(object sender, ulong e)
        {
            CurrentValue = e;
            Changed("CurrentValue");
            throw new NotImplementedException();
        }

        private void Service_CurrentDownloadFileChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public Visibility VisibilityNoNowDownload
        {
            get => Visibility.Visible;
            set
            {
                //supprort x:bind
            }
        }

        public Visibility VisibilityNowDownload
        {
            get => Visibility.Collapsed;
            set
            {
                //supprort x:bind
            }
        }
    }
}
