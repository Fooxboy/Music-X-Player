using DryIoc;
using Fooxboy.MusicX.Core;
using Fooxboy.MusicX.Uwp.Converters;
using Fooxboy.MusicX.Uwp.Models;
using Fooxboy.MusicX.Uwp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fooxboy.MusicX.Uwp.ViewModels
{
    public class PlaylistViewModel:BaseViewModel
    {

        public Album Album { get; set; }

        public  ObservableCollection<Track> Tracks { get; set; }

        public RelayCommand PlayCommmand { get; }
        public RelayCommand ShuffleCommand { get; }
        public RelayCommand AddToLibraryCommand { get; }

        public PlaylistViewModel()
        {
            Tracks = new ObservableCollection<Track>();

            PlayCommmand = new RelayCommand(Play);
            ShuffleCommand = new RelayCommand(Shuffle);
            AddToLibraryCommand = new RelayCommand(AddToLibrary);
            
        }

        public async Task StartLoading(Album album)
        {
            if(album.Id == this.Album?.Id) return;
            
            this.Album = album;
            Changed("Album");
            
            var api = Container.Get.Resolve<Api>();
            var loadingService = Container.Get.Resolve<LoadingService>();
            loadingService.Change(true);
            var tracks = (await api.VKontakte.Music.Tracks.GetAsync(200, 0, album.AccessKey, album.Id, album.OwnerId)).ToListTrack();
            foreach (var track in tracks) Tracks.Add(track);
            Changed("Tracks");
            loadingService.Change(false);

        }

        public void Play()
        {

        }

        public void Shuffle()
        {

        }

        public async void AddToLibrary()
        {

        }
    }
}
