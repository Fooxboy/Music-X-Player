using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DryIoc;
using Fooxboy.MusicX.Core;
using Fooxboy.MusicX.Uwp.Models;
using Fooxboy.MusicX.Uwp.Services;

namespace Fooxboy.MusicX.Uwp.ViewModels
{
    public class AllPlaylistsViewModel:BaseViewModel
    {

        public AllPlaylistsViewModel()
        {
            Albums = new ObservableCollection<Album>();
        }

        public ObservableCollection<Album> Albums { get; set; }

        public string TitlePage { get; set; }

        public async Task StartLoading(AllPlaylistsModel model)
        {
            TitlePage = model.TitlePage;
            Changed("TitlePage");
            var loadingService = Container.Get.Resolve<LoadingService>();
            loadingService.Change(true);
            var api = Container.Get.Resolve<Api>();
            if (model.TypeViewPlaylist == AllPlaylistsModel.TypeView.UserAlbum)
            {
                var loader = Container.Get.Resolve<AlbumLoaderService>();
                var albums = await loader.GetLibraryAlbums(0, 20);
                foreach (var album in albums)
                {
                    Albums.Add(album);
                }

                loadingService.Change(false);
            }else if (model.TypeViewPlaylist == AllPlaylistsModel.TypeView.ArtistAlbum)
            {
                //todo: доделать.
            }

            Changed("Albums");
        }

        public async Task LoadMore()
        {

        }
    }
}
