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
    public class HomeLocalViewModel :BaseViewModel
    {
        private static HomeLocalViewModel instanse;
        public static HomeLocalViewModel Instanse
        {
            get
            {
                if (instanse != null) return instanse;
                instanse = new HomeLocalViewModel();
                return instanse;
            }
        }

        public async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (StaticContent.OpenFiles)
            {
                StaticContent.OpenFiles = false;
                if (StaticContent.NowPlay != null)
                {

                    await PlayMusicService.PlayMusicForLibrary(StaticContent.NowPlay, 2);
                    Changed("Playlists");
                }

                if (StaticContent.NowPlayPlaylist != null)
                {
                    await PlayMusicService.PlayMusicForLibrary(StaticContent.NowPlayPlaylist.Tracks[0], 3, StaticContent.NowPlayPlaylist);
                    Changed("Playlists");
                }
            }
            Changed("Playlists");
            Changed("Music");
        }

        public ObservableCollection<PlaylistFile> Playlists
        {
            get
            {
                return StaticContent.Playlists;
            }
            set
            {
               
            }
        }

        public ObservableCollection<AudioFile> Music
        {
            get
            {
                return StaticContent.Music;
            }
            set
            {
               
            }
        }

        public async void MusicListView_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if(SelectedAudioFile != StaticContent.NowPlay)
            {
                await PlayMusicService.PlayMusicForLibrary(SelectedAudioFile, 1);
                Changed("Playlists");
            }
            
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

        private string trackscount;
        public string tracksCount
        {
            get
            {
                return trackscount;
            }
            set
            {
                if (trackscount == value) return;
                trackscount = value;
                Changed("tracksCount");
            }
        }

        private Visibility anymusic;
        public Visibility anyMusic
        {
            get
            {
                return anymusic;
            }
            set
            {
                if (anymusic == value) return;
                anymusic = value;
                Changed("anyMusic");
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

        public void CountMusic()
        {
            tracksCount = Music.Count + " трек(а/ов)";
            if (Music.Count == 0) anymusic = Visibility.Visible;
            anymusic = Visibility.Collapsed;
            Changed("anyMusic");
        }
    }
}
