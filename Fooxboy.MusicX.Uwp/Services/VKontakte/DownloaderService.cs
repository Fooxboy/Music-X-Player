using System;
using System.Collections.Generic;
using System.IO;
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

        bool TimerStart;
        private DownloaderService()
        {
            Timer = new DispatcherTimer();
            Timer.Interval = TimeSpan.FromSeconds(2);
            Timer.Tick += CheckProgress;
            Timer.Start();
            TimerStart = true;
            DownloadComplete += DonwloadFileComplete;

            App.Current.Resuming += Current_Resuming;
            App.Current.Suspending += Current_Suspending;
        }

        private void Current_Suspending(object sender, Windows.ApplicationModel.SuspendingEventArgs e)
        {
            this.Timer.Stop();
            TimerStart = false;
        }

        private void Current_Resuming(object sender, object e)
        {
            if (CurrentDownloadedTrack != null) this.Timer.Start();
            TimerStart = true;
        }

        private async void CheckProgress(object sender, object o)
        {
            if(CurrentDownloadOperation != null && CurrentDownloadTrack != null)
            {
                var a = CurrentDownloadOperation.Progress.BytesReceived;
                DownloadProgressChanged?.Invoke(this, a);
                if (CurrentDownloadOperation.Progress.Status == BackgroundTransferStatus.Completed && DownloadAccess)
                {
                    DownloadAccess = false;
                    var trackFile = currentFileAudio;
                    var track = CurrentDownloadTrack;


                    var mp3FileAbs = new FileMp3Abstraction()
                    {
                        Name = trackFile.Name,
                        ReadStream = await trackFile.OpenStreamForReadAsync(),
                        WriteStream = await trackFile.OpenStreamForWriteAsync(),
                    };

                    var task = Task.Run(() =>
                    {
                        using (var mp3File = TagLib.File.Create(mp3FileAbs))
                        {
                            mp3File.Tag.AlbumArtists = new string[] { track.Artist };
                            mp3File.Tag.Title = track.Title;
                            mp3File.Tag.Album = track.AlbumName;
                            mp3File.Tag.Year = uint.Parse(track.AlbumYear);
                            mp3File.Tag.Lyrics = "Загружено с ВКонтакте с помощью Music X Player (UWP)";
                            mp3File.Tag.Copyright = "Music X Player (UWP)";
                            mp3File.Tag.Conductor = "Music X Player";
                            mp3File.Tag.Comment = "Загружено с ВКонтакте с помощью Music X Player (UWP)";
                            mp3File.Save();
                        }
                    });

                    var task2 = task.ContinueWith((b) =>
                    {
                        var currentDownloadedTrack = new DownloadAudioFile()
                        {
                            AlbumName = CurrentDownloadTrack.AlbumName,
                            FromAlbum = CurrentDownloadTrack.FromAlbum,
                            AudioFile = CurrentDownloadTrack.AudioFile,
                            AlbumYear = CurrentDownloadTrack.AlbumYear,
                            Artist = CurrentDownloadTrack.Artist,
                            Cover = CurrentDownloadTrack.Cover,
                            Title = CurrentDownloadTrack.Title,
                            Url = CurrentDownloadTrack.Url
                        };

                        currentDownloadedTrack.AudioFile.SourceString = currentFileAudio.Path;
                        currentDownloadedTrack.AudioFile.Source = currentFileAudio;
                        currentDownloadedTrack.AudioFile.IsLocal = true;

                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            DownloadComplete?.Invoke(this, currentDownloadedTrack);
                        });
                    });
  
                } else if (CurrentDownloadOperation.Progress.Status == BackgroundTransferStatus.Idle && DownloadAccess)
                {
                    try
                    {
                        await CurrentDownloadOperation.StartAsync();
                    }
                    catch
                    {
                        //ниче не делаем, операция уже запущена
                    }

                } else if (CurrentDownloadOperation.Progress.Status == BackgroundTransferStatus.Error && DownloadAccess)
                {
                    DownloadAccess = false;
                    await ContentDialogService.Show(new ExceptionDialog("Возникла ошибка при загрузке трека", "Возможно, ссылка недоступна", new Exception("BackgroundTransferStatus.Error")));
                    DownloadComplete?.Invoke(this, CurrentDownloadTrack);
                }
            }
        }

        public event EventHandler CurrentDownloadFileChanged;
        public event EventHandler<DownloadAudioFile> DownloadComplete;
        public event EventHandler DownloadQueueComplete;
        public event EventHandler<ulong> DownloadProgressChanged;
        public bool DownloadAccess = false;


        public ulong Maximum { get; set; }
        public List<DownloadAudioFile> QueueTracks { get; set; }
        public DownloadAudioFile CurrentDownloadTrack { get; set; }
        public DownloadAudioFile CurrentDownloadedTrack { get; set; }
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
                    Title = audio.Title.Replace("*", "").Replace(".", "").Replace("\"", "").Replace("\\", "").Replace("/", "").Replace("[", "").Replace("]", "").Replace(":", "").Replace(";", "").Replace("|", "").Replace("=", "").Replace(",", ""),
                    AlbumName = audio.Title.Replace("*", "").Replace(".", "").Replace("\"", "").Replace("\\", "").Replace("/", "").Replace("[", "").Replace("]", "").Replace(":", "").Replace(";", "").Replace("|", "").Replace("=", "").Replace(",", ""),
                    AlbumYear = "2019",
                    Artist = audio.Artist.Replace("*", "").Replace(".", "").Replace("\"", "").Replace("\\", "").Replace("/", "").Replace("[", "").Replace("]", "").Replace(":", "").Replace(";", "").Replace("|", "").Replace("=", "").Replace(",", ""),
                    Cover = audio.Cover,
                    Url = audio.SourceString,
                    FromAlbum = false,
                    AudioFile = audio
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
            foreach (var audio in playlist.TracksFiles)
            {
                var track = new DownloadAudioFile()
                {
                    Title = audio.Title.Replace("*", "").Replace(".", "").Replace("\"", "").Replace("\\", "").Replace("/", "").Replace("[", "").Replace("]", "").Replace(":", "").Replace(";", "").Replace("|", "").Replace("=", "").Replace(",", ""),
                    AlbumName = playlist.Name.Replace("*", "").Replace(".", "").Replace("\"", "").Replace("\\", "").Replace("/", "").Replace("[", "").Replace("]", "").Replace(":", "").Replace(";", "").Replace("|", "").Replace("=", "").Replace(",", ""),
                    AlbumYear = playlist.Year,
                    Artist = audio.Artist.Replace("*", "").Replace(".", "").Replace("\"", "").Replace("\\", "").Replace("/", "").Replace("[", "").Replace("]", "").Replace(":", "").Replace(";", "").Replace("|", "").Replace("=", "").Replace(",", ""),
                    Cover = audio.Cover,
                    Url = audio.SourceString,
                    FromAlbum = true,
                    AudioFile = audio
                };
                AddToQueue(track);
            }

            StorageFolder folder = await KnownFolders.MusicLibrary.TryGetItemAsync("Music X") == null ?
                await KnownFolders.MusicLibrary.CreateFolderAsync("Music X")
                : await KnownFolders.MusicLibrary.GetFolderAsync("Music X");

            var playlistName = playlist.Name.Replace("*", "").Replace(".", "").Replace("\"", "").Replace("\\", "").Replace("/", "").Replace("[", "").Replace("]", "").Replace(":", "").Replace(";", "").Replace("|", "").Replace("=", "").Replace(",", "");
            StorageFolder playlistFolder = await folder.TryGetItemAsync(playlistName) == null ?
                await folder.CreateFolderAsync(playlistName)
                : await folder.GetFolderAsync(playlistName);

            var task = Task.Run(async () =>
            {
                await DownloadAudio(QueueTracks.First());
            });

        }

        public StorageFile currentFileAudio;

        private async Task DownloadAudio(DownloadAudioFile track)
        {

            if (!TimerStart) this.Timer.Start();

           
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
                var albumName = track.AlbumName.Replace("*", "").Replace(".", "").Replace("\"", "").Replace("\\", "").Replace("/", "").Replace("[", "").Replace("]", "").Replace(":", "").Replace(";", "").Replace("|", "").Replace("=", "").Replace(",", "");
                var libraryPlaylist = await libraryTracks.GetFolderAsync(track.AlbumName);
                if (await libraryPlaylist.TryGetItemAsync($"{track.Artist} - {track.Title} (Music X).mp3") != null)
                {
                    DownloadComplete?.Invoke(this, CurrentDownloadTrack);
                }else
                {
                    trackFile = await libraryPlaylist.CreateFileAsync($"{track.Artist} - {track.Title} (Music X).mp3");
                }
            }
            currentFileAudio = trackFile;
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


        private void DonwloadFileComplete(object a, DownloadAudioFile e)
        {
            
            var settings = ApplicationData.Current.LocalSettings;

            settings.Values["CountDownloads"] = (int)settings.Values["CountDownloads"] + 1;

            QueueTracks.Remove(e);
            if (QueueTracks.Count == 0)
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    this.Timer.Stop();
                    DownloadQueueComplete?.Invoke(this, null);
                });
            }
            else
            {
                var task = Task.Run(async () =>
                {
                    await DownloadAudio(QueueTracks.First());
                });
            }

            CurrentDownloadTrack = null;
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
