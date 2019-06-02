using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fooxboy.MusicX.Uwp.Models;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
using Fooxboy.MusicX.Uwp.Utils.Extensions;
using Windows.UI.Xaml.Input;
using Fooxboy.MusicX.Uwp.Services;

namespace Fooxboy.MusicX.Uwp.ViewModels
{
    public class PlaylistViewModel : BaseViewModel
    {
        public PlaylistViewModel()
        {
        }

        private static PlaylistViewModel instanse;
        public static PlaylistViewModel Instanse
        {
            get
            {
                if (instanse != null) return instanse;
                instanse = new PlaylistViewModel();
                return instanse;
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


        private string pltrackcount;
        public string PLTrackCount
        {
            get
            {
                try
                {
                    return $"{playlist.Tracks.Count} трек(а/ов)";
                }catch(Exception)
                {
                    return "0 треков";
                }
            }
            set
            {
                if (value != pltrackcount)
                {
                    pltrackcount = value;
                    Changed("PLTrackCount");
                }
            }
        }

        private PlaylistFile playlist;
        public PlaylistFile Playlist
        {
            get
            {
                if(playlist == null)
                {
                    return StaticContent.NowPlayPlaylist;
                }else
                {
                    return playlist;
                }
            }
            set
            {
                if (value != playlist)
                {
                    playlist = value;
                    Changed("Playlist");
                }
            }
        }

        public async void MusicListView_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await PlayMusicService.PlayMusicForLibrary(SelectedAudioFile, 3, Playlist);
        }

       
        public async void ListViewMusic_Click(object sender, ItemClickEventArgs e)
        {
            await PlayMusicService.PlayMusicForLibrary(SelectedAudioFile, 3, Playlist);
        }

        private AudioFile selectedAudioFile;
        public AudioFile SelectedAudioFile
        {
            get
            {
                return selectedAudioFile;
            }
            set
            {
                if (selectedAudioFile == value) return;
                selectedAudioFile = value;
                Changed("SelectedAudioFile");
            }
        }

    }
}
