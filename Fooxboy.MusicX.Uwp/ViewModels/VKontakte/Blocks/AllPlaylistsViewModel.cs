using Fooxboy.MusicX.Uwp.Models;
using Fooxboy.MusicX.Uwp.Services.VKontakte;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Input;

namespace Fooxboy.MusicX.Uwp.ViewModels.VKontakte.Blocks
{
    public class AllPlaylistsViewModel:BaseViewModel
    {
        public AllPlaylistsViewModel()
        {
            Albums = new ObservableCollection<PlaylistFile>();
        }
        public ObservableCollection<PlaylistFile> Albums { get; set; }
        public PlaylistFile SelectAlbum { get; set; }
        public bool IsLoading { get; set; }


        public void Playlists_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            if (SelectAlbum == null) return;
            StaticContent.NavigationContentService.Go(typeof(Views.PlaylistView), SelectAlbum);
        }


        public async Task StartLoading(string blockId)
        {
            IsLoading = true;
            Changed("IsLoading");
            var albumsVk = await MusicX.Core.VKontakte.Music.Artists.GetAlbums(blockId);
            var albums = new List<PlaylistFile>();

            foreach (var album in albumsVk)
            {
                Albums.Add(await PlaylistsService.ConvertToPlaylistFile(album));
            }

            IsLoading = false;
            Changed("Albums");
            Changed("IsLoading");
        }
    }
}
