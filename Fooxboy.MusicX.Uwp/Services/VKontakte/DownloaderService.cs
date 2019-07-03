using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Uwp.Models;
using Fooxboy.MusicX.Uwp.Resources.ContentDialogs;
using GalaSoft.MvvmLight.Threading;
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
            Timer.Start();
            DownloadComplete += DonwloadFileComplete;
        }


        private async void CheckProgress(object sender, object o)
        {
            if(CurrentDownloadOperation != null && CurrentDownloadTrack != null)
            {
                var a = CurrentDownloadOperation.Progress.BytesReceived;
                DownloadProgressChanged?.Invoke(this, a);
                if(CurrentDownloadOperation.Progress.Status == BackgroundTransferStatus.Completed&& DownloadAccess)
                {
                    DownloadComplete?.Invoke(this, null);
                }else if(CurrentDownloadOperation.Progress.Status == BackgroundTransferStatus.Idle && DownloadAccess)
                {
                    try
                    {
                        await CurrentDownloadOperation.StartAsync();
                    }
                    catch
                    {
                        //ниче не делаем, операция уже запущена
                    }
                    
                }
            }
        }

        public event EventHandler CurrentDownloadFileChanged;
        public event EventHandler DownloadComplete;
        public event EventHandler DownloadQueueComplete;
        public event EventHandler<ulong> DownloadProgressChanged;
        public bool DownloadAccess = false;


        public ulong Maximum { get; set; }
        public List<DownloadAudioFile> QueueTracks { get; set; }
        public DownloadAudioFile CurrentDownloadTrack { get; set; }
        public DownloadOperation CurrentDownloadOperation { get; set; }


        private static string GetFileSize(Uri uriPath)
        {
            var webRequest = HttpWebRequest.Create(uriPath);
            webRequest.Method = "HEAD";

            using (var webResponse = webRequest.GetResponse())
            {
                var fileSize = webResponse.Headers.Get("Content-Length");
                return fileSize;
            }
        }

        public async Task StartDownloadAudio(AudioFile audio)
        {
            try
            {
                var track = new DownloadAudioFile()
                {
                    Title = audio.Title,
                    AlbumName = audio.Title,
                    AlbumYear = "2019",
                    Artist = audio.Artist,
                    Cover = audio.Cover,
                    Url = audio.SourceString,
                    FromAlbum = false
                };
                AddToQueue(track);

                StorageFolder folder = await KnownFolders.MusicLibrary.TryGetItemAsync("Music X") == null ?
                    await KnownFolders.MusicLibrary.CreateFolderAsync("Music X")
                    : await KnownFolders.MusicLibrary.GetFolderAsync("Music X");


                if ((await folder.TryGetItemAsync($"{track.Artist} - {track.Title} (Music X).mp3")) != null) return;

                if (CurrentDownloadTrack == null)
                {
                    var task = Task.Run(async () =>
                    {
                        await DownloadAudio(track);
                    });
                }
            }catch(Exception e)
            {
                await ContentDialogService.Show(new ExceptionDialog("Невозможно начать загрузку трека", "Попробуйте чуть-чуть позже", e));
            }
            
        }

        public async Task StartDownloadPlaylist(PlaylistFile playlist)
        {
            foreach (var audio in playlist.Tracks)
            {
                var track = new DownloadAudioFile()
                {
                    Title = audio.Title,
                    AlbumName = playlist.Name,
                    AlbumYear = playlist.Year,
                    Artist = audio.Artist,
                    Cover = audio.Cover,
                    Url = audio.SourceString,
                    FromAlbum = true
                };
                AddToQueue(track);
            }

            StorageFolder folder = await KnownFolders.MusicLibrary.TryGetItemAsync("Music X") == null ?
                await KnownFolders.MusicLibrary.CreateFolderAsync("Music X")
                : await KnownFolders.MusicLibrary.GetFolderAsync("Music X");

            StorageFolder playlistFolder = await folder.TryGetItemAsync(playlist.Name) == null ?
                await folder.CreateFolderAsync(playlist.Name)
                : await folder.GetFolderAsync(playlist.Name);

            var task = Task.Run(async () =>
            {
                await DownloadAudio(QueueTracks.First());
            });

        }

        private async Task DownloadAudio(DownloadAudioFile track)
        {
            CurrentDownloadTrack = track;
            DownloadAccess = false;
            StorageFile trackFile = null;
            if (!track.FromAlbum)
            {
                var libraryTracks = await KnownFolders.MusicLibrary.GetFolderAsync("Music X");
                trackFile = await libraryTracks.CreateFileAsync($"{track.Artist} - {track.Title} (Music X).mp3");
            }else
            {
                var libraryTracks = await KnownFolders.MusicLibrary.GetFolderAsync("Music X");
                var libraryPlaylist = await libraryTracks.GetFolderAsync(track.AlbumName);
                if (await libraryPlaylist.TryGetItemAsync($"{track.Artist} - {track.Title} (Music X).mp3") != null)
                {
                    DownloadComplete?.Invoke(this, null);
                }else
                {
                    trackFile = await libraryPlaylist.CreateFileAsync($"{track.Artist} - {track.Title} (Music X).mp3");
                }
            }
            
            BackgroundDownloader downloader = new BackgroundDownloader();
            DownloadOperation download = downloader.CreateDownload(new Uri(track.Url), trackFile);
            CurrentDownloadOperation = download;
            Maximum = ulong.Parse(GetFileSize(new Uri(track.Url)));
            var task = Task.Run(() =>
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    CurrentDownloadFileChanged?.Invoke(this, null);
                });

            });
            
            DownloadAccess = true;
            await download.StartAsync();
        }


        private void DonwloadFileComplete(object a, EventArgs e)
        {
            if(CurrentDownloadTrack != null)
            {
                QueueTracks.Remove(CurrentDownloadTrack);
                CurrentDownloadTrack = null;
                if (QueueTracks.Count == 0) DownloadQueueComplete?.Invoke(this, null);
                else
                {
                    var task = Task.Run(async () =>
                    {
                        await DownloadAudio(QueueTracks.First());
                    });
                    
                }
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
