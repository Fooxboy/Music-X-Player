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
using Fooxboy.MusicX.Uwp.Resources.ContentDialogs;
using Windows.UI.Xaml;
using Windows.UI.Popups;
using Fooxboy.MusicX.Uwp.Services.VKontakte;
using Windows.Storage;

namespace Fooxboy.MusicX.Uwp.ViewModels
{
    public class PlaylistViewModel : BaseViewModel
    {
        private PlaylistViewModel()
        {
            DownloadIsActive = true;

            EditPlaylist = new RelayCommand(async () =>
            {
                if(Playlist.Id != 1 & Playlist.Id != 2 & Playlist.Id != 1000 && Playlist.IsLocal)
                {
                    await ContentDialogService.Show(new EditPlaylistContentDialog(Playlist));
                    Changed("Playlist");
                }else
                {
                    await new MessageDialog("Этот плейлист невозможно изменить", "Ошибка редактирования плейлиста").ShowAsync();
                }
                
            });

            DownloadPlaylist = new RelayCommand(async () =>
            {
                DownloadIsActive = false;
                Changed("DownloadIsActive");

                try
                {
                    var settings = ApplicationData.Current.LocalSettings;
                    int countTracks = (int)settings.Values["CountDownloads"];
                    int countTracksWithAlbum = countTracks + Playlist.Tracks.Count;

                    if (!StaticContent.IsPro)
                    {
                        if (countTracksWithAlbum > 19) await new MessageDialog("Извините, но загрузка более 20 треков доступна только  в Pro версии.").ShowAsync();
                        else
                        {
                            var service = DownloaderService.GetService;
                            await service.StartDownloadPlaylist(Playlist);
                        }
                    }
                    else
                    {
                        var service = DownloaderService.GetService;
                        await service.StartDownloadPlaylist(Playlist);
                    }


                }
                catch (Exception e)
                {
                    await ContentDialogService.Show(new ExceptionDialog("Невозможно начать загрузку плейлиста", "Попробуйте ещё раз", e));
                }

            });
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

        public bool DownloadIsActive { get; set; }

        public RelayCommand EditPlaylist { get; set; }
        public RelayCommand DownloadPlaylist { get; set; }

        public string Description { get; set; }

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

        public Visibility VisibilityDescription
        {
            get
            {
                if (Playlist != null) return Playlist.IsAlbum ? Visibility.Collapsed : Visibility.Visible;
                else return Visibility.Collapsed;
            }
            set
            {
                //support x:bind
            }
        }

        public Visibility VisibilityInfo
        {
            get
            {
                if (Playlist != null) return Playlist.IsAlbum ? Visibility.Visible : Visibility.Collapsed;
                else return Visibility.Collapsed;

            }
            set
            {
                //support x:bind;
            }
        }

        private string pltrackcount;
        public string PLTrackCount
        {
            get
            {
                if (pltrackcount == null)
                    return "0 треков";
                else return pltrackcount;
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

        private Visibility visibilityNoTrack;
        public Visibility VisibilotyNoTrack
        {
            get
            {
                return visibilityNoTrack;
            }
            set
            {
                if(visibilityNoTrack != value)
                {
                    visibilityNoTrack = value;
                    Changed("VisibilotyNoTrack");
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
                    if (value.TracksFiles.Count == 0) VisibilotyNoTrack = Visibility.Visible;
                    else VisibilotyNoTrack = Visibility.Collapsed;
                    playlist = value;
                    Changed("Playlist");
                    PLTrackCount = $"{playlist.TracksFiles.Count} трек(а/ов)";
                    Changed("PLTrackCount");
                }
            }
        }

        public async void MusicListView_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if(SelectedAudioFile != null)
            {
                if (SelectedAudioFile.IsLocal) await PlayMusicService.PlayMusicForLibrary(SelectedAudioFile, 3, Playlist);
                else await MusicService.PlayMusic(SelectedAudioFile, 2, Playlist);
            }
        }

       
        public async void ListViewMusic_Click(object sender, ItemClickEventArgs e)
        {
            if (SelectedAudioFile.IsLocal) await PlayMusicService.PlayMusicForLibrary(SelectedAudioFile, 3, Playlist);
            else await MusicService.PlayMusic(SelectedAudioFile, 2, Playlist);
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
