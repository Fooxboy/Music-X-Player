using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Uwp.Models;
using Fooxboy.MusicX.Uwp.Services;
using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarSymbols;
using Newtonsoft.Json;
using Windows.Storage;

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

        private  HomeViewModel()
        {
            
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
            var pathPlaylists = await ApplicationData.Current.LocalFolder.GetFolderAsync("Playlists");
            var files = await pathPlaylists.GetFilesAsync();
            var playlists = new ObservableCollection<PlaylistFile>();
            foreach(var file in files)
            {
                var json = await FileIO.ReadTextAsync(file);
                var playlist = JsonConvert.DeserializeObject<PlaylistFile>(json);
                playlists.Add(playlist);
            }
            this.playlists = playlists;
            Changed("Playlists");
        }

        public async Task GetMusicLocal()
        {
            var files = (await KnownFolders.MusicLibrary.GetFilesAsync(Windows.Storage.Search.CommonFileQuery.OrderByName)).ToList();
            var music = new ObservableCollection<AudioFile>();
            foreach (var f in files)
            {
                if (f.FileType == ".mp3" || f.FileType == ".wav")
                {
                    var track = await FindMetadataService.ConvertToAudioFile(f);
                    music.Add(track);
                }
            }
            this.music = music;
            Changed("Music");
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
