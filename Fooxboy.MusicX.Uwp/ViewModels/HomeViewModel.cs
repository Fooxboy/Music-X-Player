using Fooxboy.MusicX.Uwp.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fooxboy.MusicX.Uwp.ViewModels
{
    public class HomeViewModel:BaseViewModel
    {
        public ObservableCollection<Album> Albums { get; set; }
        public ObservableCollection<Track> Tracks { get; set; }

        public async Task StartLoadingAlbums()
        {
            //TODO: старт загрузки треков.
        }
    }
}
