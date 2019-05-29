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
            RefreshMusicCommand = new RelayCommand(async () =>
            {
                await MusicFilesService.GetMusicLocal(true);
            });
        }


        public RelayCommand RefreshMusicCommand { get; private set; }

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
            }set
            {
                //if (value != StaticContent.Playlists)
                //{
                //    StaticContent.Playlists = value;
                //    Changed("Playlists");
                //}
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
                //if (value != StaticContent.Music)
                //{
                //    StaticContent.Music = value;
                //    Changed("Music");
                //}
            }
        }



        public async void MusicListView_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await PlayMusicService.PlayMusicForLibrary(SelectedAudioFile, 1);
            Changed("Playlists");
        }

        public async void ListViewMusic_Click(object sender, ItemClickEventArgs e)
        {
            //await PlayMusicService.PlayMusicForLibrary(SelectedAudioFile, 1);
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
    }
}
