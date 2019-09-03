using Fooxboy.MusicX.Uwp.Models;
using Fooxboy.MusicX.Uwp.Resources.ContentDialogs;
using Fooxboy.MusicX.Uwp.Services;
using Fooxboy.MusicX.Uwp.Services.VKontakte;
using Fooxboy.MusicX.Uwp.Views.VKontakte;
using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarSymbols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace Fooxboy.MusicX.Uwp.ViewModels.VKontakte
{
    public class LikedArtistsViewModel : BaseViewModel
    {

        public LikedArtistsViewModel()
        {
            DeleteCommand = new RelayCommand(async () =>
            {
                try
                {
                    await LikedArtistsService.RemoveArtist(SelectArtist.Id);
                    var msg = new MessageDialog($"Исполнитель {SelectArtist.Name} больше не находится в Ваших любимчиках!");
                    Artists.Remove(SelectArtist);
                    Changed("Artists");
                }catch(Exception e)
                {
                    await ContentDialogService.Show(new ExceptionDialog("Невозможно удалить артиста", "Пока что нет возможности удалить, но скоро все будет хорооооошо :)", e));
                }
               
            });
        }
        public List<LikedArtist> Artists { get; set; }

        public LikedArtist SelectArtist { get; set; }
        public Visibility NoArtistsVisibility { get; set; } = Visibility.Collapsed;
        public bool IsLoading { get; set; }

        public RelayCommand DeleteCommand { get; set; }

        public async Task StartLoading()
        {
            IsLoading = true;
            Changed("IsLoading");
            var artists = await LikedArtistsService.GetLikedArtists();
            Artists = artists.Artists;
            Changed("Artists");

            IsLoading = false;
            Changed("IsLoading");
            if (Artists.Count == 0)
            {
                NoArtistsVisibility = Visibility.Visible;
                Changed("NoArtistsVisibility");
            }
        }

        public void GoToArtist(object e, object o)
        {
            if (SelectArtist == null) return;
            StaticContent.NavigationContentService.Go(typeof(ArtistView), new ArtistParameter() { Id=SelectArtist.Id, Name = SelectArtist.Name });
        }
    }
}
