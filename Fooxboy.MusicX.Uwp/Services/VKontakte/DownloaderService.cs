using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Uwp.Models;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.UI.Xaml;

namespace Fooxboy.MusicX.Uwp.Services.VKontakte
{
    public class DownloaderService
    {

        private static DownloaderService service;

        public static DownloaderService GetService
        {
            get
            {
                if (service == null) service = new DownloaderService();

                return service;
            }
        }


        private DownloaderService()
        {
            Timer = new DispatcherTimer();
            Timer.Interval = TimeSpan.FromSeconds(2);
            Timer.Tick += CheckProgress;

            DownloadComplete += DonwloadFileComplete;
        }


        private void CheckProgress(object sender, object o)
        {
            if(CurrentDownloadOperation != null)
            {
                var a = CurrentDownloadOperation.Progress.BytesReceived;
                DownloadProgressChanged?.Invoke(this, a);
                if(CurrentDownloadOperation.Progress.Status == BackgroundTransferStatus.Completed)
                {
                    DownloadComplete?.Invoke(this, null);
                }
            }
        }


        public event EventHandler DownloadComplete;
        public event EventHandler DownloadQueueComplete;
        public event EventHandler<ulong> DownloadProgressChanged;


        public ulong Maximum { get; set; }
        public List<DownloadAudioFile> QueueTracks { get; set; }
        public DownloadAudioFile CurrentDownloadTrack { get; set; }
        public DownloadOperation CurrentDownloadOperation { get; set; }

        public async Task StartDownloadAudio(AudioFile audio)
        {
            var track = new DownloadAudioFile()
            {
                Title = audio.Title,
                AlbumName = audio.Title,
                AlbumYear = "2019",
                Artist = audio.Artist,
                Cover = audio.Cover,
                Url = audio.SourceString
            };
            AddToQueue(track);

            StorageFolder folder = await KnownFolders.MusicLibrary.TryGetItemAsync("Music X") == null ? 
                await KnownFolders.MusicLibrary.CreateFolderAsync("Music X") 
                : await KnownFolders.MusicLibrary.GetFolderAsync("Music X");


            if (folder.TryGetItemAsync($"{track.Artist} - {track.Title} (Music X).mp3") != null) return;

            if (CurrentDownloadTrack == null) await DownloadAudio(track);

        }

        private async Task DownloadAudio(DownloadAudioFile track)
        {
            CurrentDownloadTrack = track;
            var libraryTracks = await KnownFolders.MusicLibrary.GetFolderAsync("Music X");
            var trackFile = await libraryTracks.CreateFileAsync($"{track.Artist} - {track.Title} (Music X).mp3");
            BackgroundDownloader downloader = new BackgroundDownloader();
            DownloadOperation download = downloader.CreateDownload(new Uri(track.Url), trackFile);
            CurrentDownloadOperation = download;
            await download.StartAsync();
            Maximum = download.Progress.TotalBytesToReceive;
        }


        private async void DonwloadFileComplete(object a, EventArgs e)
        {
            QueueTracks.Remove(CurrentDownloadTrack);
            CurrentDownloadTrack = null;
            if (QueueTracks.Count == 0) DownloadQueueComplete?.Invoke(this, null);
            else
            {
                await DownloadAudio(QueueTracks.First());
            }

            
        }


        DispatcherTimer Timer; 


        private void AddToQueue(DownloadAudioFile track)
        {
            if(QueueTracks == null)
            {
                QueueTracks = new List<DownloadAudioFile>();
            }

            if (QueueTracks.Any(t => t == track)) return;
            QueueTracks.Add(track);

        }



    }
}
