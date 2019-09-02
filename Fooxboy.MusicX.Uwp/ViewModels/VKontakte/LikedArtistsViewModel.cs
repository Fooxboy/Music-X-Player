using Fooxboy.MusicX.Uwp.Models;
using Fooxboy.MusicX.Uwp.Services.VKontakte;
using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarSymbols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fooxboy.MusicX.Uwp.ViewModels.VKontakte
{
    public class LikedArtistsViewModel:BaseViewModel
    {
        public List<LikedArtist> Artists { get; set; }
        public bool IsLoading { get; set; }

        public async Task StartLoading()
        {
            IsLoading = true;
            Changed("IsLoading");
            var artists = await LikedArtistsService.GetLikedArtists();
            Artists = artists.Artists;
            Changed("Artists");

            IsLoading = false;
            Changed("IsLoading");
        }
    }
}
