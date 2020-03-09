using DryIoc;
using Fooxboy.MusicX.Core;
using Fooxboy.MusicX.Core.VKontakte.Music;
using Fooxboy.MusicX.Uwp.Converters;
using Fooxboy.MusicX.Uwp.Models;
using Fooxboy.MusicX.Uwp.Services;
using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarSymbols;
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

        private int _countTracks { get; set; }
        private long _maxTracks { get; set; }
        private bool _isLoading;

        private int _count;


        public HomeViewModel()
        {
            Tracks = new ObservableCollection<Track>();
            Albums = new ObservableCollection<Album>();
            _count = 20;
        }


        public async Task StartLoadingAlbums()
        {
            
            //TODO: старт загрузки альбомов.
        }

        public async Task GetMaxTracks()
        {
            var api = Container.Get.Resolve<Api>();
            _maxTracks = await api.VKontakte.Music.Tracks.GetCountAsync();
        }

        public async Task StartLoadingTracks()
        {
            if (_isLoading) return;
            _isLoading = true;
            //RemoveElements();

            //Tracks.Add(new Track() { AccessKey = "loading" });
            //Changed("Tracks");

            
            if (_maxTracks == _countTracks)
            {
                if(Tracks[Tracks.Count - 1].AccessKey != "space") Tracks.Add(new Track() { AccessKey = "space" });
                _isLoading = false;
                return;

            }
            var tracks = await LoadTracks();
            if (tracks.Count == 0)
            {
                _maxTracks = _countTracks;
                return;
            }

            AddTracksToList(tracks);
        }

        private void RemoveElements()
        {
            var elems = Tracks.Where(t => t.AccessKey == "loading" || t.AccessKey == "space").ToList();
            foreach (var elem in elems)
            {
                Tracks.Remove(elem);
            }
            var i = 1 + 1;
        }

        private void AddTracksToList(System.Collections.Generic.List<Track> tracks)
        {
            //RemoveElements();
            foreach (var track in tracks) Tracks.Add(track);     
            _countTracks += tracks.Count;

            //Tracks.Add(new Track() { AccessKey = "space" });

            Changed("Tracks");
            _isLoading = false;

        }

        private async Task<System.Collections.Generic.List<Track>> LoadTracks()
        {
            var loader = Container.Get.Resolve<TrackLoaderService>();

            var tracks = await loader.GetLibraryTracks(_countTracks, _count);

            return tracks;
        }
    }
}
