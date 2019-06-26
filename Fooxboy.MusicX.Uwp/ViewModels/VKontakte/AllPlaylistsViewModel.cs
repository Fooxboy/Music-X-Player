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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Fooxboy.MusicX.Uwp.ViewModels.VKontakte
{
    public  class AllPlaylistsViewModel:BaseViewModel
    {

        private static AllPlaylistsViewModel instanse;
        public static AllPlaylistsViewModel Instanse
        {
            get
            {
                if(instanse == null)
                {
                    instanse = new AllPlaylistsViewModel();
                }

                return instanse;
            }
        }

        private AllPlaylistsViewModel()
        {
            Playlists = new LoadingCollection<PlaylistFile>();
            Playlists.OnMoreItemsRequested = GetMorePlaylist;
            Playlists.HasMoreItemsRequested = HasMorePlaylists;
        }


        private bool hasMorePlaylists = true;

        public PlaylistFile SelectPlaylist { get; set; }

        public LoadingCollection<PlaylistFile> Playlists { get; set; }

        public void Playlists_ItemClick(object sender, ItemClickEventArgs e)
        {

            if (SelectPlaylist == null) return;
            StaticContent.NavigationContentService.Go(typeof(Views.PlaylistView), SelectPlaylist);
        }

        public void Playlists_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (SelectPlaylist == null) return;

            StaticContent.NavigationContentService.Go(typeof(Views.PlaylistView), SelectPlaylist);
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
                        playlistsVk = await Library.Playlists(20, Playlists.Count);
                        foreach (var playlist in playlistsVk) playlists.Add(await Services.VKontakte.PlaylistsService.ConvertToPlaylistFile(playlist));
                    }
                    catch (Flurl.Http.FlurlHttpException)
                    {
                        hasMorePlaylists = false;
                        await ContentDialogService.Show(new ErrorConnectContentDialog());
                        InternetService.GoToOfflineMode();
                    }

                    if (playlists.Count == 0)
                    {
                        VisibilityNoPlaylists = Visibility.Visible;
                        Changed("VisibilityNoPlaylists");
                    }

                    if (playlists.Count < 20)
                    {
                        hasMorePlaylists = false;
                    }
                    return playlists;
                }catch(Exception e)
                {
                    await ContentDialogService.Show(new ExceptionDialog("Неизвестная ошибка при получении плейлистов", "Мы не смогли получить информацию о Ваших плейлистах", e));
                    return new List<PlaylistFile>();
                }
                
            }else
            {
                InternetService.GoToOfflineMode();
                return new List<PlaylistFile>();
            }
        }

        public bool HasMorePlaylists() => hasMorePlaylists;

        public Visibility VisibilityNoPlaylists { get; set; }
    }
}
