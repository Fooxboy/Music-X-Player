﻿using DryIoc;
using Fooxboy.MusicX.Core;
using Fooxboy.MusicX.Core.Interfaces;
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


        public RelayCommand OpelAllPlaylistsCommand { get; set; }


        public HomeViewModel()
        {
            Tracks = new ObservableCollection<Track>();
            Albums = new ObservableCollection<Album>();
            
            _count = 20;
            _player = Container.Get.Resolve<PlayerService>();
            _libraryAlbum = new Album()
            {
                Title = "Ваша музыка",
                IsAvailable = true,
                Tracks = new System.Collections.Generic.List<Core.Interfaces.ITrack>()
            };

            _loadingService = Container.Get.Resolve<LoadingService>();

            _notificationService = Container.Get.Resolve<NotificationService>();

            OpelAllPlaylistsCommand = new RelayCommand(OpenAllPlaylists);

            loader = Container.Get.Resolve<TrackLoaderService>();
        }


        public void OpenAllPlaylists()
        {
            var model = new AllPlaylistsModel();
            model.TypeViewPlaylist = AllPlaylistsModel.TypeView.UserAlbum;
            model.TitlePage = "Ваши альбомы";
            model.AlbumLoader = albumLoader;
            var navigationService = Container.Get.Resolve<NavigationService>();

            navigationService.Go(typeof(AllPlaylistsView), model, 1);
        }

      

        public async Task StartLoadingAlbums()
        {

            //TODO: старт загрузки альбомов.

            var albums = await LoadAlbums();
            foreach (var a in albums) Albums.Add(a);
            Changed("Albums");
        }

        private async Task<System.Collections.Generic.List<Album>> LoadAlbums()
        {
            albumLoader = Container.Get.Resolve<AlbumLoaderService>();
            var albums = await albumLoader.GetLibraryAlbums(0, 10);
            return albums;
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

            _loadingService.Change(true);
            
            if (_maxTracks == _countTracks)
            {
                _loadingService.Change(false);
                if (Tracks[Tracks.Count - 1].AccessKey != "space") Tracks.Add(new Track() { AccessKey = "space" });
                _isLoading = false;
               
                return;

            }
            var tracks = await LoadTracks();
            if (tracks.Count == 0)
            {
                _loadingService.Change(false);
                if (Tracks[Tracks.Count - 1].AccessKey != "space") Tracks.Add(new Track() { AccessKey = "space" });
                _maxTracks = _countTracks;
                return;
               
            }

            AddTracksToList(tracks);
        }

       
        private void AddTracksToList(System.Collections.Generic.List<Track> tracks)
        {
            foreach (var track in tracks) Tracks.Add(track);     
            _countTracks += tracks.Count;

            Changed("Tracks");
            _isLoading = false;
            _loadingService.Change(false);

        }

        private async Task<System.Collections.Generic.List<Track>> LoadTracks()
        {

            var tracks = await loader.GetLibraryTracks(_countTracks, _count);

            return tracks;
        }

        public void PlayTrack(Track track)
        {
            _player.Play(_libraryAlbum, track, Tracks.ToList());
            //_notificationService.CreateNotification("Воспроизведение", $"{track.Title}");
        }
    }
}
