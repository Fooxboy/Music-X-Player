using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Uwp.Enums;
using Fooxboy.MusicX.Uwp.Models;
using Fooxboy.MusicX.Uwp.Services;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace Fooxboy.MusicX.Uwp
{
    public static class StaticContent
    {
        public static NavigationService NavigationContentService { get; set; }
        public static Frame PlayerMenuFrame { get; set; }
        public static double Volume { get; set; }
        public static RepeatMode Repeat { get; set; }
        public static bool Shuffle { get; set; }
        public static AudioFile NowPlay { get; set; }
        public static StorageFolder CoversFolder { get; set; }
        public static StorageFolder PlaylistsFolder { get; set; }
        public static StorageFolder LocalFolder { get; set; }
        public static PlaylistFile NowPlayPlaylist { get; set; }
        public static AudioService AudioService => AudioService.Instance;
        private static ObservableCollection<PlaylistFile> playlists;
        public static ObservableCollection<PlaylistFile> Playlists
        {
            get
            {
                if (playlists == null)
                {
                    playlists = new ObservableCollection<PlaylistFile>();
                    return playlists;
                }
                else return playlists;
            }
        }
        private static ObservableCollection<AudioFile> music;
        public static ObservableCollection<AudioFile> Music
        {
            get
            {
                if (music == null)
                {
                    music = new ObservableCollection<AudioFile>();
                    return music;
                }
                else return music;
            }
        }
        public static bool OpenFiles { get; set; }
    }
}
