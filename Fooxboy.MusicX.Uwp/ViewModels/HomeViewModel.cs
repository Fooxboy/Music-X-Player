using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Uwp.Models;
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
    }
}
