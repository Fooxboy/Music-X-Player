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
        public string ArtistText { get; set; }
        public string Genres { get; set; }

        public  ObservableCollection<Track> Tracks { get; set; }

        public RelayCommand PlayCommmand { get; }
        public RelayCommand ShuffleCommand { get; }
        public RelayCommand AddToLibraryCommand { get; }

        private IContainer _container;

        public PlaylistViewModel(IContainer container)
        {
            _container = container;
            Tracks = new ObservableCollection<Track>();
            ArtistText = "Нет музыканта";
            Genres = "Без жанра";
            PlayCommmand = new RelayCommand(Play);
            ShuffleCommand = new RelayCommand(Shuffle);
            AddToLibraryCommand = new RelayCommand(AddToLibrary);
            
        }

        public async Task StartLoading(Album album)
        {
            try
            {
                if (album.Id == this.Album?.Id) return;

                this.Album = album;
                if(album.Artists.Count > 0) this.ArtistText = album.Artists[0].Name;
                if (album.Genres.Count > 0) this.Genres = album.Genres[0];
                Changed("Album");
                Changed("ArtistText");

                var api = _container.Resolve<Api>();
                var loadingService = _container.Resolve<LoadingService>();
                loadingService.Change(true);
                var tracks =
                    (await api.VKontakte.Music.Tracks.GetAsync(200, 0, album.AccessKey, album.Id, album.OwnerId))
                    .ToListTrack();
                foreach (var track in tracks) Tracks.Add(track);

                Tracks.Add(new Track() {AccessKey = "space"});
                Changed("Tracks");
                loadingService.Change(false);
            }
            catch (Exception e)
            {
                var notificationService = _container.Resolve<NotificationService>();
                notificationService.CreateNotification("Ошибка при загрузке плейлиста", e.Message);
            }
            

        }

        public void Play()
        {
            if (this.Tracks.Count > 0) this.PlayTrack(this.Tracks[0]);
        }

        public void Shuffle()
        {
            var playService = _container.Resolve<PlayerService>();
            playService.SetShuffle(true);
            int index = new Random().Next(0, Tracks.Count());
            this.PlayTrack(this.Tracks[index]);
        }

        public async void AddToLibrary()
        {
            var notificationService = _container.Resolve<NotificationService>();
            notificationService.CreateNotification("Невозможно добавить плейлист", "Данная функция пока что недоступна.");
        }

        public void PlayTrack(Track track)
        {
            var playService = _container.Resolve<PlayerService>();
            playService.Play(this.Album, track, this.Tracks.ToList());
        }
    }
}
