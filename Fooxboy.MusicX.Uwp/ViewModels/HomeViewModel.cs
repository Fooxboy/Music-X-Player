using DryIoc;
using Fooxboy.MusicX.Core;
using Fooxboy.MusicX.Uwp.Converters;
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

        public int _countTracks { get; set; }
        public long _maxTracks { get; set; }

        public async Task StartLoadingAlbums()
        {
            
            //TODO: старт загрузки треков.
        }

        public async Task GetMaxTracks()
        {
            var api = Container.Get.Resolve<Api>();

            _maxTracks = await api.VKontakte.Music.Tracks.GetCountAsync();
        }

        public async Task StartLoadingTracks()
        {
            if (_maxTracks == _countTracks) return;
            var api = Container.Get.Resolve<Api>();

            var tracks = await api.VKontakte.Music.Tracks.GetAsync(10, _countTracks);

            if (tracks.Count == 0) _maxTracks = _countTracks;

            foreach (var track in tracks) Tracks.Add(track.ToTrack());

            _countTracks += tracks.Count;
            Changed("Tracks");
        }
    }
}
