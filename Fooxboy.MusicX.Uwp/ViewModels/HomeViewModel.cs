using DryIoc;
using Fooxboy.MusicX.Core;
using Fooxboy.MusicX.Core.Interfaces;
using Fooxboy.MusicX.Core.VKontakte.Music;
using Fooxboy.MusicX.Uwp.Converters;
using Fooxboy.MusicX.Uwp.Models;
using Fooxboy.MusicX.Uwp.Services;
using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarSymbols;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flurl.Http;
using Fooxboy.MusicX.Uwp.Views;

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
        private PlayerService _player;
        private Album _libraryAlbum;
        private LoadingService _loadingService;
        private NotificationService _notificationService;
        private TrackLoaderService loader;
        private AlbumLoaderService albumLoader;
        private IContainer _container;


        public RelayCommand OpelAllPlaylistsCommand { get; set; }


        public HomeViewModel(IContainer container)
        {
            _container = container;
            Tracks = new ObservableCollection<Track>();
            Albums = new ObservableCollection<Album>();
            
            _count = 20;
            _player = _container.Resolve<PlayerService>();
            _libraryAlbum = new Album()
            {
                Title = "Ваша музыка",
                IsAvailable = true,
                Tracks = new System.Collections.Generic.List<Core.Interfaces.ITrack>()
            };

            _loadingService = _container.Resolve<LoadingService>();

            _notificationService = _container.Resolve<NotificationService>();

            OpelAllPlaylistsCommand = new RelayCommand(OpenAllPlaylists);

            loader = _container.Resolve<TrackLoaderService>();
        }


        public void OpenAllPlaylists()
        {
            var model = new AllPlaylistsModel();
            model.TypeViewPlaylist = AllPlaylistsModel.TypeView.UserAlbum;
            model.TitlePage = "Ваши альбомы";
            model.AlbumLoader = albumLoader;
            model.Container = _container;
            var navigationService = _container.Resolve<NavigationService>();

            navigationService.Go(typeof(AllPlaylistsView), model, 1);
        }

      

        public async Task StartLoadingAlbums()
        {

            try
            {

                var albums = await LoadAlbums();
                foreach (var a in albums) Albums.Add(a);
                Changed("Albums");
            }
            catch (FlurlHttpException)
            {
                _notificationService.CreateNotification("Ошибка сети", "Произошла ошибка подключения к сети.",
                    "Попробовать ещё раз", "Закрыть", new RelayCommand(
                        async () => { await this.StartLoadingAlbums(); }), new RelayCommand(() => { }));
            }
            catch (Exception e)
            {
                _notificationService.CreateNotification("Ошибка при загрузке Ваших альбомов", $"Ошибка: {e.Message}");

            }

        }

        private async Task<System.Collections.Generic.List<Album>> LoadAlbums()
        {
            try
            {
                albumLoader = _container.Resolve<AlbumLoaderService>();
                var albums = await albumLoader.GetLibraryAlbums(0, 10);
                return albums;
            }
            catch (FlurlHttpException)
            {
                _notificationService.CreateNotification("Ошибка сети", "Произошла ошибка подключения к сети.",
                    "Попробовать ещё раз", "Закрыть", new RelayCommand(
                        async () => { await this.LoadAlbums(); }), new RelayCommand(() => { }));
                return new List<Album>();
            }
            catch (Exception e)
            {
                _notificationService.CreateNotification("Ошибка при загрузке Ваших альбомов", $"Ошибка: {e.Message}");
                return new List<Album>();
            }

        }

        public async Task GetMaxTracks()
        {
            try
            {
                var api = _container.Resolve<Api>();
                _maxTracks = await api.VKontakte.Music.Tracks.GetCountAsync();
            }
            catch (FlurlHttpException)
            {
                _notificationService.CreateNotification("Ошибка сети", "Произошла ошибка подключения к сети.",
                    "Попробовать ещё раз", "Закрыть", new RelayCommand(
                        async () => { await this.GetMaxTracks(); }), new RelayCommand(() => { }));
            }
            catch (Exception e)
            {
                _notificationService.CreateNotification("Ошибка при загрузке Ваших треков", $"Ошибка: {e.Message}");
            }

        }

        public async Task StartLoadingTracks()
        {
            try
            {
                if (_isLoading) return;
                _isLoading = true;

                _loadingService.Change(true);

                if (_maxTracks == _countTracks)
                {
                    _loadingService.Change(false);
                    if (Tracks[Tracks.Count - 1].AccessKey != "space") Tracks.Add(new Track() {AccessKey = "space"});
                    _isLoading = false;

                    return;

                }

                var tracks = await LoadTracks();
                if (tracks.Count == 0)
                {
                    _loadingService.Change(false);
                    if (Tracks[Tracks.Count - 1].AccessKey != "space") Tracks.Add(new Track() {AccessKey = "space"});
                    _maxTracks = _countTracks;
                    return;

                }

                AddTracksToList(tracks);
            }
            catch (FlurlHttpException)
            {
                _notificationService.CreateNotification("Ошибка сети", "Произошла ошибка подключения к сети.",
                    "Попробовать ещё раз", "Закрыть", new RelayCommand(
                        async () => { await this.StartLoadingTracks(); }), new RelayCommand(() => { }));
            }
            catch (Exception e)
            {
                _notificationService.CreateNotification("Ошибка при загрузке Ваших треков", $"Ошибка: {e.Message}");

            }

        }

       
        private void AddTracksToList(System.Collections.Generic.List<Track> tracks)
        {
            try
            {
                foreach (var track in tracks) Tracks.Add(track);
                _countTracks += tracks.Count;

                Changed("Tracks");
                _isLoading = false;
                _loadingService.Change(false);
            }
            catch (Exception e)
            {
                _notificationService.CreateNotification("Ошибка при добавлении треков в список", $"Ошибка: {e.Message}");
            }
        }

        private async Task<System.Collections.Generic.List<Track>> LoadTracks()
        {

            try
            {
                var tracks = await loader.GetLibraryTracks(_countTracks, _count);

                return tracks;
            }
            catch (FlurlHttpException)
            {
                _notificationService.CreateNotification("Ошибка сети", "Произошла ошибка подключения к сети.",
                    "Попробовать ещё раз", "Закрыть", new RelayCommand(
                        async () => { await this.LoadTracks(); }), new RelayCommand(() => { }));
                return new List<Track>();
            }
            catch (Exception e)
            {
                _notificationService.CreateNotification("Ошибка при загрузке Ваших треков", $"Ошибка: {e.Message}");
                return new List<Track>();
            }
        }

        public void PlayTrack(Track track)
        {
            _player.Play(_libraryAlbum, track, Tracks.ToList());
            //_notificationService.CreateNotification("Воспроизведение", $"{track.Title}");
        }
    }
}
