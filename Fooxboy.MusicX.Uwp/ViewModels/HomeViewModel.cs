using Fooxboy.MusicX.Uwp.Models;
using Fooxboy.MusicX.Uwp.Services;
using System;
using System.Collections.ObjectModel;
using ReactiveUI;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using DynamicData;
using ReactiveUI.Fody.Helpers;
using VkNet.Model.Attachments;

namespace Fooxboy.MusicX.Uwp.ViewModels
{
    public class HomeViewModel : ReactiveObject, IRoutableViewModel
    {
        private readonly PlayerService _playerService;
        private readonly TrackLoaderService _trackLoaderService;
        private readonly AlbumLoaderService _albumLoader;
        private readonly ReadOnlyObservableCollection<AudioPlaylist> _albums;
        private readonly ReadOnlyObservableCollection<Audio> _tracks;

        public HomeViewModel(PlayerService playerService, TrackLoaderService trackLoaderService,
            AlbumLoaderService albumLoaderService)
        {
            _playerService = playerService;
            _albumLoader = albumLoaderService;
            _trackLoaderService = trackLoaderService;

            OpelAllPlaylistsCommand = ReactiveCommand.Create(OpenAllPlaylists);

            var canPlay = this.WhenAnyValue(model => model.SelectedTrack, selector: track => track != null);
            PlayCommand = ReactiveCommand.Create<Audio>(PlayTrack, canPlay);

            LoadTracks().ObserveOn(RxApp.MainThreadScheduler).Bind(out _tracks).Subscribe();
            LoadAlbums().ObserveOn(RxApp.MainThreadScheduler).Bind(out _albums).Subscribe();

            this.WhenAnyValue(model => model.SelectedTrack).InvokeCommand(PlayCommand);
        }

        public ReadOnlyObservableCollection<AudioPlaylist> Albums => _albums;

        public ReadOnlyObservableCollection<Audio> Tracks => _tracks;

        [Reactive]
        public Audio SelectedTrack { get; set; }

        public string UrlPathSegment => "home";

        public IScreen HostScreen { get; }

        public ReactiveCommand<Unit, Unit> OpelAllPlaylistsCommand { get; set; }

        public ReactiveCommand<Audio, Unit> PlayCommand { get; set; }

        public void OpenAllPlaylists()
        {
            var model = new AllPlaylistsModel
            {
                TypeViewPlaylist = AllPlaylistsModel.TypeView.UserAlbum,
                TitlePage = "Ваши альбомы",
                AlbumLoader = _albumLoader
            };

            //_navigationService.Go(typeof(AllPlaylistsView), model, 1);
        }

        private IObservable<IChangeSet<AudioPlaylist, long?>> LoadAlbums()
        {
            return _albumLoader.GetLibraryAlbums(0, 10).ToObservable().ToObservableChangeSet(x => x.Id);
        }

        private IObservable<IChangeSet<Audio, long?>> LoadTracks()
        {
            return _trackLoaderService.GetLibraryTracks().ToObservable().ToObservableChangeSet(x => x.Id);
        }

        public void PlayTrack(Audio track)
        {
            _playerService.Play(track);
            //_notificationService.CreateNotification("Воспроизведение", $"{track.Title}");
        }
    }
}