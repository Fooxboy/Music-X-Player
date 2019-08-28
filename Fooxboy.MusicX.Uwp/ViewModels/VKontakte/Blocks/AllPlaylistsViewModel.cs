using Fooxboy.MusicX.Uwp.Models;
using Fooxboy.MusicX.Uwp.Services.VKontakte;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Fooxboy.MusicX.Core.VKontakte.Music.Converters;
using System.Threading.Tasks;
using Windows.UI.Xaml.Input;
using Fooxboy.MusicX.Core.Interfaces;
using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarSymbols;
using Fooxboy.MusicX.Uwp.Services;
using Fooxboy.MusicX.Uwp.Resources.ContentDialogs;

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
            try
            {
                IsLoading = true;
                Changed("IsLoading");
                var almbs = (await MusicX.Core.VKontakte.Music.Block.GetById(blockId)).Playlists;
                var albumsVk = new List<IPlaylistFile>();
                foreach (var al in almbs) albumsVk.Add(al.ToIPlaylistFile(new List<IAudioFile>(), "Различные исполнители"));
                var albums = new List<PlaylistFile>();

                foreach (var album in albumsVk)
                {
                    Albums.Add(await Services.VKontakte.PlaylistsService.ConvertToPlaylistFile(album));
                }

                IsLoading = false;
                Changed("Albums");
                Changed("IsLoading");
            }catch(Exception e)
            {
                IsLoading = false;
                Changed("IsLoading");

                await ContentDialogService.Show(new ExceptionDialog("Ошибка при загрузке плейлистов", "Попробуйте ещё раз", e));
            }
           
        }
    }
}
