using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Fooxboy.MusicX.Uwp.Models;
using Fooxboy.MusicX.Uwp.Resources.ContentDialogs;
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


        private HomeLocalViewModel()
        {
            var musicCollection = new LoadingCollection<AudioFile>();
            musicCollection.OnMoreItemsRequested = GetMoreAudio;
            MusicCollection = musicCollection;

            RefreshCommand = new RelayCommand(async () =>
            {
                await MusicFilesService.GetMusicLocal(true);

            });

        }

        public void OnNavigate()
        {
            Changed("Playlists");
        }


        public async Task<List<AudioFile>> GetMoreAudio(CancellationToken token, uint countAudio)
        {
            var count = Convert.ToInt32(countAudio);
            var collection = new List<AudioFile>();
            for(var  i = count; i<= count+20; i++)
            {
                collection.Add(Music[i]);
            }

            return collection;
        }

        public async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
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
                        await PlayMusicService.PlayMusicForLibrary(StaticContent.NowPlayPlaylist.TracksFiles[0], 3, StaticContent.NowPlayPlaylist);
                        Changed("Playlists");
                    }
                }
            }catch(Exception ee)
            {
                await ContentDialogService.Show(new ExceptionDialog("Ошибка при загрузке домашней страницы", "Music X не смог запустить последний файл, который играл у Вас на компьютере.", ee));
            }
           
            Changed("Playlists");
            Changed("Music");
        }


        private Visibility visibilityNoTrack = Visibility.Collapsed;

        public Visibility VisibilityNoTrack
        {
            get
            {
                return visibilityNoTrack;
            }
            set
            {
                if(value != visibilityNoTrack)
                {
                    visibilityNoTrack = value;
                    Changed("VisibilityNoTrack");
                }
            }
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

        public LoadingCollection<AudioFile> MusicCollection { get; set; }

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
            try
            {
                if (SelectedAudioFile != StaticContent.NowPlay)
                {
                    await PlayMusicService.PlayMusicForLibrary(SelectedAudioFile, 1);
                    Changed("Playlists");
                }
            }catch(Exception ee)
            {
                await ContentDialogService.Show(new ExceptionDialog("Ошибка при клике на трек", "Произошла неизвестная ошибка", ee));
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
            if (SelectedPlaylistFile == null) return;
            StaticContent.NavigationContentService.Go(typeof(Views.PlaylistView), SelectedPlaylistFile);
        }

        public void PlaylistListView_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (SelectedPlaylistFile == null) return;
            StaticContent.NavigationContentService.Go(typeof(Views.PlaylistView), SelectedPlaylistFile);
        }

        public RelayCommand RefreshCommand { get; set; }

        public void CountMusic()
        {
            if (StaticContent.Music.Count == 0) VisibilityNoTrack = Visibility.Visible;
            else VisibilityNoTrack = Visibility.Collapsed;

            tracksCount = Music.Count + " трек(а/ов)";
            if (Music.Count == 0) anymusic = Visibility.Visible;
            anymusic = Visibility.Collapsed;
            Changed("anyMusic");
        }
    }
}
