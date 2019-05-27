using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Fooxboy.MusicX.Uwp.Models;
using Fooxboy.MusicX.Uwp.Services;
using Fooxboy.MusicX.Uwp.Utils.Extensions;
using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarSymbols;
using Newtonsoft.Json;
using TagLib.Matroska;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Fooxboy.MusicX.Uwp.ViewModels
{
    public class HomeViewModel : BaseViewModel 
    {
        private static HomeViewModel instanse;
        public static HomeViewModel Instanse
        {
            get
            {
                if (instanse != null) return instanse;
                instanse = new HomeViewModel();
                return instanse;
            }
        }

        private HomeViewModel()
        {
            
        }


        public async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if(StaticContent.NowPlay != null)
            {
                selectedAudioFile = StaticContent.NowPlay;
                await PlayMusicForLibrary();
            }

            if(StaticContent.NowPlayPlaylist != null)
            {
                selectedAudioFile = StaticContent.NowPlayPlaylist.Tracks[0];
                await PlayMusicForLibrary();
            }
        }

        private ObservableCollection<PlaylistFile> playlists;
        public ObservableCollection<PlaylistFile> Playlists
        {
            get
            {
                return playlists;
            }set
            {
                if (value != playlists)
                {
                    playlists = value;
                    Changed("Playlists");
                }
            }
        }

        private ObservableCollection<AudioFile> music;
        public ObservableCollection<AudioFile> Music
        {
            get
            {
                return music;
            }
            set
            {
                if (value != music)
                {
                    music = value;
                    Changed("Music");
                }
            }
        }


        public async Task GetPlaylistLocal()
        {

            try
            {
                var pathPlaylists = await ApplicationData.Current.LocalFolder.GetFolderAsync("Playlists");
                var files = await pathPlaylists.GetFilesAsync();
                var playlists = new ObservableCollection<PlaylistFile>();
                foreach (var file in files)
                {
                    var json = await FileIO.ReadTextAsync(file);
                    var playlist = JsonConvert.DeserializeObject<PlaylistFile>(json);
                    playlists.Add(playlist);
                }
                this.playlists = playlists;
                Changed("Playlists");
            }
            catch(Exception e)
            {
                ContentDialog deleteFileDialog = new ContentDialog()
                {
                    Title = "АШИБКА",
                    Content = $"{e}",
                    PrimaryButtonText = "ОК",
                    SecondaryButtonText = "Отмена"
                };

                await deleteFileDialog.ShowAsync();
            }
            
        }

        public async Task GetMusicLocal()
        {
            try
            {
                var files = (await KnownFolders.MusicLibrary.GetFilesAsync(Windows.Storage.Search.CommonFileQuery.OrderByName)).ToList();
                var music = new ObservableCollection<AudioFile>();
                foreach (var f in files)
                {
                    if (f.FileType == ".mp3" || f.FileType == ".wav")
                    {
                        try
                        {
                            var track = await FindMetadataService.ConvertToAudioFile(f);
                            music.Add(track);
                        }catch(Exception e)
                        {

                        }
                        
                    }
                }
                this.music = music;
                Changed("Music");
            }
            catch(Exception e)
            {
                ContentDialog deleteFileDialog = new ContentDialog()
                {
                    Title = "АШИБКА",
                    Content = $"{e}",
                    PrimaryButtonText = "ОК",
                    SecondaryButtonText = "Отмена"
                };

                await deleteFileDialog.ShowAsync();
            }
            
        }


        public async void MusicListView_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await PlayMusicForLibrary();
        }


        private async Task PlayMusicForLibrary()
        {

            try
            {
                var track = selectedAudioFile;
                var lastPlayPlaylist = await PlaylistsService.GetById(1);
                if (!(lastPlayPlaylist.Tracks.Any(t => t.Source == track.Source))) lastPlayPlaylist.Tracks.Add(track);

                if (StaticContent.NowPlayPlaylist == null)
                {
                    var playlistNowPlayA = new PlaylistFile()
                    {
                        Artist = "Music X",
                        Cover = "ms-appx:///Assets/Images/now.png",
                        Id = 1000,
                        Name = "Сейчас играет",
                        Tracks = new List<AudioFile>()
                    };
                    if (music != null)
                    {
                        foreach (var trackMusic in music) playlistNowPlayA.Tracks.Add(trackMusic);
                        StaticContent.NowPlayPlaylist = playlistNowPlayA;
                    }
                    else
                    {
                        StaticContent.NowPlayPlaylist = playlistNowPlayA;
                        StaticContent.NowPlayPlaylist.Tracks.Add(track);
                    }


                    if(playlists != null)
                    { 
                        playlists.Add(StaticContent.NowPlayPlaylist);
                    }
                    
                    var playlistNowPlayIsAudioPlaylist = StaticContent.NowPlayPlaylist.ToAudioPlaylist(track);
                    StaticContent.AudioService.SetCurrentPlaylist(playlistNowPlayIsAudioPlaylist);
                    Changed("Playlists");
                }
                else
                {
                    if (!(StaticContent.NowPlayPlaylist.Tracks.Any(t => t.Source == track.Source))) StaticContent.NowPlayPlaylist.Tracks.Add(track);
                    if (StaticContent.AudioService.IsPlaying) StaticContent.AudioService.Pause();
                    StaticContent.AudioService.CurrentPlaylist.CurrentItem = track.ToIAudio();
                    StaticContent.NowPlay = track;
                }

                StaticContent.AudioService.Play();
            }catch(Exception e)
            {
                ContentDialog deleteFileDialog = new ContentDialog()
                {
                    Title = "АШИБКА",
                    Content = $"{e}",
                    PrimaryButtonText = "ОК",
                    SecondaryButtonText = "Отмена"
                };

                await deleteFileDialog.ShowAsync();

            }


        }
        public async void ListViewMusic_Click(object sender, ItemClickEventArgs e)
        {
            await PlayMusicForLibrary();
        }

        private AudioFile selectedAudioFile;
        public AudioFile SelectedAudioFile
        {
            get
            {
                return selectedAudioFile;
            }set
            {
                if (selectedAudioFile == value) return;
                selectedAudioFile = value;
                Changed("SelectedAudioFile");
            }
        }

        private PlaylistFile selectedPlayListFile;
        public PlaylistFile SelectedPlaylistFile
        {
            get
            {
                return selectedPlayListFile;
            }
            set
            {
                if (selectedPlayListFile == value) return;
                selectedPlayListFile = value;
                Changed("SelectedPlayListFile");
            }
        }

        public void PlaylistListView_Click(object sender, ItemClickEventArgs e)
        {
            StaticContent.NavigationContentService.Go(typeof(Views.PlaylistView), SelectedPlaylistFile);
        }

        public void PlaylistListView_Tapped(object sender, TappedRoutedEventArgs e)
        {
            StaticContent.NavigationContentService.Go(typeof(Views.PlaylistView), SelectedPlaylistFile);
        }

        public async Task<int> CountMusic()
        {
            return (await KnownFolders.MusicLibrary.GetFilesAsync(Windows.Storage.Search.CommonFileQuery.OrderByName)).ToList().Count;
        }

        public async static Task<ObservableCollection<AudioFile>> GetFromSubfolder(StorageFolder folder)
        {
            var re = new ObservableCollection<AudioFile>();
            var files = (await folder.GetFilesAsync()).ToList();
            foreach(var f in files)
            {
               if(f.FileType == ".mp3" || f.FileType == ".wav"){
                    var track = await FindMetadataService.ConvertToAudioFile(f);
                    re.Add(track);
               }
            }
            var subs = (await folder.GetFoldersAsync()).ToList();
            if (subs.Any()) foreach (var s in subs) foreach(var t in (await GetFromSubfolder(s)).ToList()) re.Add(t);
            return re;
        }

       
    }
}
