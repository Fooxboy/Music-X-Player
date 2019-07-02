using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Fooxboy.MusicX.Core.Interfaces;
using Fooxboy.MusicX.Core.VKontakte.Music;
using Fooxboy.MusicX.Uwp.Models;
using Fooxboy.MusicX.Uwp.Resources.ContentDialogs;
using Fooxboy.MusicX.Uwp.Services;
using Fooxboy.MusicX.Uwp.Services.VKontakte;
using Fooxboy.MusicX.Uwp.Views;
using Fooxboy.MusicX.Uwp.Views.VKontakte;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Fooxboy.MusicX.Uwp.ViewModels.VKontakte
{
    public class HomeViewModel:BaseViewModel
    {
        private static HomeViewModel instanse;
        private long maxCountElements = -1;
        const int countTracksLoading = 20;
        private bool loadingPlaylists = true;
        private bool loadingMusic = true;
        public bool NoPlaylists = false;


        public static HomeViewModel Instanse
        {
            get
            {
                if (instanse == null) instanse = new HomeViewModel();

                return instanse;
            }
        }



        private HomeViewModel()
        {
            Music = new LoadingCollection<AudioFile>();
            Music.OnMoreItemsRequested = GetMoreAudio;
            Music.HasMoreItemsRequested = HasMoreGetAudio;

            Playlists = new LoadingCollection<PlaylistFile>();
            Playlists.OnMoreItemsRequested = GetMorePlaylist;
            Playlists.HasMoreItemsRequested = HasMoreGetPlaylists;
            VisibilityNoTracks = Visibility.Collapsed;
            IsLoading = true;
            visibilityPlaylists = Visibility.Visible;
            Changed("VisibilityNoTracks");
            Changed("IsLoading");
            Changed("Music");

            RefreshCommand = new RelayCommand(() =>
            {
                StaticContent.MusicVKontakte.Clear();
                StaticContent.PlaylistsVKontakte.Clear();
                Changed("Music");
                Changed("Playlists");
                ClearReady();
            });

            NavigateToSearchCommand = new RelayCommand(() =>
            {
                StaticContent.NavigationContentService.Go(typeof(SearchView), TextSearch);
            });

        }

        public void ClearReady()
        {
            loadingMusic = true;
            loadingPlaylists = true;
        }

        public string TextSearch { get; set; }


        public LoadingCollection<AudioFile> Music
        {
            get => StaticContent.MusicVKontakte;
            set
            {
                if (StaticContent.MusicVKontakte == value) return;
                StaticContent.MusicVKontakte = value;
                Changed("Music");
            }
        }
        public LoadingCollection<PlaylistFile> Playlists
        {
            get => StaticContent.PlaylistsVKontakte;
            set
            {
                if (StaticContent.PlaylistsVKontakte == value) return;
                StaticContent.PlaylistsVKontakte = value;
                Changed("Playlists");
            }
        }

        public async Task<List<AudioFile>> GetMoreAudio(CancellationToken token, uint offset)
        {
            if(InternetService.Connected)
            {
                try
                {
                    if (Music.Count > 0)
                    {
                        IsLoading = true;
                        Changed("IsLoading");
                    }

                    IList<IAudioFile> tracks = new List<IAudioFile>();
                    List<AudioFile> music = new List<AudioFile>();
                    try
                    {
                        IsLoading = true;
                        Changed("IsLoading");
                        if (maxCountElements == -1) maxCountElements = await Library.CountTracks();
                        tracks = await Library.Tracks(countTracksLoading, Music.Count);
                        music = await MusicService.ConvertToAudioFile(tracks);
                    }
                    catch (Flurl.Http.FlurlHttpException)
                    {
                        music = new List<AudioFile>();
                        IsLoading = false;
                        Changed("IsLoading");
                        loadingMusic = false;
                        InternetService.GoToOfflineMode();
                        await ContentDialogService.Show(new ErrorConnectContentDialog());
                    }

                    IsLoading = false;
                    Changed("IsLoading");
                    if (music.Count < countTracksLoading)
                    {
                        loadingMusic = false;
                    }
                    else
                    {
                        loadingMusic = true;
                    }

                    if (music.Count == 0)
                    {
                        VisibilityNoTracks = Visibility.Collapsed;
                        Changed("VisibilityNoTracks");
                    }
                    return music;
                }catch(Exception e)
                {
                    await ContentDialogService.Show(new ExceptionDialog("Неизвестная ошибка при получении треков",
                        "Возможно, какой-то информации о треке нет или трек недействителен", e));
                    return new List<AudioFile>();
                }
                
            }else
            {
                InternetService.GoToOfflineMode();

                return new List<AudioFile>();
            }
            
        }

        public async Task<List<PlaylistFile>> GetMorePlaylist(CancellationToken token, uint offset)
        {

            if(InternetService.Connected)
            {
                try
                {
                    IList<IPlaylistFile> playlistsVk;
                    List<PlaylistFile> playlists = new List<PlaylistFile>();
                    try
                    {
                        playlistsVk = await Library.Playlists(5, 0);
                        foreach (var playlist in playlistsVk) playlists.Add(await Services.VKontakte.PlaylistsService.ConvertToPlaylistFile(playlist));
                    }
                    catch (Flurl.Http.FlurlHttpException)
                    {
                        loadingPlaylists = false;
                        InternetService.GoToOfflineMode();
                        await ContentDialogService.Show(new ErrorConnectContentDialog());
                    }
                    loadingPlaylists = false;
                    if (playlists.Count == 0)
                    {
                        NoPlaylists = true;
                        VisibilityPlaylists = Visibility.Collapsed;
                        Changed("VisibilityPlaylists");
                    }
                    return playlists;
                }catch(Exception e)
                {
                    await ContentDialogService.Show(new ExceptionDialog("Неизвестная ошибка при получении плейлистов", "Мы не смогли получить определённую информацию", e));
                    return new List<PlaylistFile>();
                }
            }else
            {
                InternetService.GoToOfflineMode();
                return new List<PlaylistFile>();
            }
            
        }


        public async Task PageLoaded()
        {
            if(StaticContent.IsAuth)
            {
                if(StaticContent.CurrentSessionIsAuth)
                {
                    try
                    {
                        await AuthService.AutoAuth();
                    }
                    catch (VkNet.Exception.UserAuthorizationFailException)
                    {
                        await AuthService.LogOut();
                        StaticContent.NavigationContentService.Go(typeof(AuthView));
                    }
                    catch (VkNet.Exception.VkAuthorizationException)
                    {
                        await AuthService.LogOut();
                        StaticContent.NavigationContentService.Go(typeof(AuthView));
                    }
                    catch (VkNet.Exception.VkApiAuthorizationException)
                    {
                        await AuthService.LogOut();
                        StaticContent.NavigationContentService.Go(typeof(AuthView));
                    }
                    catch (VkNet.Exception.UserDeletedOrBannedException)
                    {
                        await AuthService.LogOut();
                        StaticContent.NavigationContentService.Go(typeof(AuthView));
                    }catch(VkNet.Exception.AccessTokenInvalidException)
                    {
                        await AuthService.LogOut();
                        StaticContent.NavigationContentService.Go(typeof(AuthView));
                    }catch (Flurl.Http.FlurlHttpException)
                    {
                        PlayerMenuViewModel.Instanse.VkontaktePages = Visibility.Collapsed;
                        await ContentDialogService.Show(new ErrorConnectContentDialog());
                        StaticContent.IsAuth = false;
                        StaticContent.NavigationContentService.Go(typeof(HomeLocalView));
                    }
                } 
            }
        }

        public PlaylistFile SelectPlaylist { get; set; }

        public AudioFile SelectAudio { get; set; }
        public bool HasMoreGetAudio() => loadingMusic;
        public bool HasMoreGetPlaylists() => loadingPlaylists;

        public async void MusicListView_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (SelectAudio == null) return; 
             
            await MusicService.PlayMusic(SelectAudio, 1);
        }

        public void ListViewPlaylists_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (SelectPlaylist == null) return;

            StaticContent.NavigationContentService.Go(typeof(PlaylistView), SelectPlaylist);
        }

        public void ListViewPlaylists_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        public Visibility VisibilityNoTracks { get; set; }
        public bool IsLoading { get; set; }

        private Visibility visibilityPlaylists;
        public Visibility VisibilityPlaylists
        {
            get => visibilityPlaylists;
            set
            {
                if (visibilityPlaylists == value) return;

                visibilityPlaylists = value;
                Changed("VisibilityPlaylists");
            }
        }


        public RelayCommand RefreshCommand { get; set; }
        public RelayCommand NavigateToSearchCommand { get; set; }
    }
}
