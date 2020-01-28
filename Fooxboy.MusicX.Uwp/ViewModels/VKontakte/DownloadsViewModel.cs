//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Fooxboy.MusicX.Uwp.Models;
//using Fooxboy.MusicX.Uwp.Resources.ContentDialogs;
//using Fooxboy.MusicX.Uwp.Services;
//using Fooxboy.MusicX.Uwp.Services.VKontakte;
//using Windows.Storage;
//using Windows.UI.Xaml;
//using Windows.UI.Xaml.Controls;
//using Windows.UI.Xaml.Input;

//namespace Fooxboy.MusicX.Uwp.ViewModels.VKontakte
//{
//    public class DownloadsViewModel : BaseViewModel
//    {

//        private static DownloadsViewModel instanse;

//        public static DownloadsViewModel Instanse
//        {
//            get
//            {
//                if (instanse == null) instanse = new DownloadsViewModel();

//                return instanse;
//            }
//        }

//        public RelayCommand ShowQueueCommand { get; set; }
//        public ObservableCollection<AudioFile> Music { get; set; }

//        public AudioFile SelectedAudio { get; set; }

//        public DownloaderService Service { get; set; }
//        private PlaylistFile playlistCurrent;
//        private DownloadsViewModel()
//        {
//            Service = DownloaderService.GetService;
//            Service.CurrentDownloadFileChanged += Service_CurrentDownloadFileChanged;
//            Service.DownloadProgressChanged += Service_DownloadProgressChanged;
//            Service.DownloadQueueComplete += Service_DownloadQueueComplete;
//            Service.DownloadComplete += Service_DownloadComplete;

//            if (Service.CurrentDownloadTrack != null)
//            {
//                CurrentDownloadFile = Service.CurrentDownloadTrack;
//                Changed("CurrentDownloadFile");
//                MaximumValue = Service.Maximum;
//                VisibilityNoNowDownload = Visibility.Collapsed;
//                VisibilityNowDownload = Visibility.Visible;
//                Changed("VisibilityNoNowDownload");
//                Changed("VisibilityNowDownload");
//                Changed("TitleString");
//                Changed("MaximumValue");
//                Changed("ArtistString");
//                Changed("AlbumString");
//                Changed("YearAlbumString");
//            } else
//            {
//                VisibilityNoNowDownload = Visibility.Visible;
//                VisibilityNowDownload = Visibility.Collapsed;
//                Changed("VisibilityNoNowDownload");
//                Changed("VisibilityNowDownload");
//            }

//            playlistCurrent = new PlaylistFile()
//            {
//                Artist = "",
//                Cover = "ms-appx:///Assets/Images/playlist-placeholder.png",
//                Id = 778,
//                IsLocal = true,
//                Year = "2019",
//                IsAlbum = false,
//                Genre = "без жанра",
//                Name = "Загруженное"
//            };

//            ShowQueueCommand = new RelayCommand(async () =>
//            {
//                await ContentDialogService.Show(new QueueDownloadContentDialog(Service.QueueTracks));
//            });

//        }

//        public double MaximumValue { get; set; }
//        public double CurrentValue { get; set; }
//        public DownloadAudioFile CurrentDownloadFile { get; set; }

//        public async Task StartLoadingTracks()
//        {
//            if ((await KnownFolders.MusicLibrary.TryGetItemAsync("Music X")) != null)
//            {
//                var tracks = await DownloadLibraryService.GetTracks();
//                Music = new ObservableCollection<AudioFile>(tracks);
//                if(tracks.Count == 0)
//                {
//                    VisibilityNoDownloadsTracks = Visibility.Visible;
//                    Changed("VisibilityNoDownloadsTracks");
//                }else
//                {
//                    playlistCurrent.TracksFiles = Music.ToList();
//                    VisibilityNoDownloadsTracks = Visibility.Collapsed;
//                    Changed("VisibilityNoDownloadsTracks");
//                }
//                Changed("Music");
//            }
//        }


//        private void Service_DownloadComplete(object sender, DownloadAudioFile e)
//        {

//            if (e == null) return;

//            if (e.AudioFile.Source == null) return;
//            VisibilityNoDownloadsTracks = Visibility.Collapsed;
//            Changed("VisibilityNoDownloadsTracks");

//            if (Music.Any(a => a == e.AudioFile)) return;

//            Music.Add(e.AudioFile);
//            Changed("Music");
//        }

//        private void Service_DownloadQueueComplete(object sender, EventArgs e)
//        {
//            VisibilityNoNowDownload = Visibility.Visible;
//            VisibilityNowDownload = Visibility.Collapsed;
//            Changed("VisibilityNoNowDownload");
//            Changed("VisibilityNowDownload");
//        }

//        private void Service_DownloadProgressChanged(object sender, ulong e)
//        {
//            CurrentValue = e;
//            Changed("CurrentValue");
//        }

//        private void Service_CurrentDownloadFileChanged(object sender, EventArgs e)
//        {
//            if (Service.CurrentDownloadTrack != null)
//            {
//                CurrentDownloadFile = Service.CurrentDownloadTrack;
//                Changed("CurrentDownloadFile");

//                MaximumValue = Service.Maximum;
//                Changed("MaximumValue");

//                VisibilityNoNowDownload = Visibility.Collapsed;
//                Changed("VisibilityNoNowDownload");

//                VisibilityNowDownload = Visibility.Visible;
//                Changed("VisibilityNowDownload");
//            }
//            else
//            {
//                VisibilityNoNowDownload = Visibility.Visible;
//                VisibilityNowDownload = Visibility.Collapsed;
//                Changed("VisibilityNoNowDownload");
//                Changed("VisibilityNowDownload");
//            }
//        }

//        public async void MusicListView_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            playlistCurrent.TracksFiles = Music.ToList();
//            await PlayMusicService.PlayMusicForLibrary(SelectedAudio, 3, playlistCurrent);
//        }

//        public async void MusicListView_Tapped(object sender, TappedRoutedEventArgs e)
//        {
//            playlistCurrent.TracksFiles = Music.ToList();
//            await PlayMusicService.PlayMusicForLibrary(SelectedAudio, 3, playlistCurrent);
//        }

//        public Visibility VisibilityNoNowDownload { get; set; }

//        public Visibility VisibilityNowDownload { get; set; }
//        public Visibility VisibilityNoDownloadsTracks { get; set; }

//    }
//}
