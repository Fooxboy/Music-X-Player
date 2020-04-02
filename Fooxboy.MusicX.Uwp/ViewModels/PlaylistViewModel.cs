using Fooxboy.MusicX.Uwp.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using VkNet.Model.Attachments;

namespace Fooxboy.MusicX.Uwp.ViewModels
{
    public class PlaylistViewModel : BaseViewModel
    {
        private readonly TrackLoaderService _trackLoaderService;
        private readonly PlayerService _playerService;
        private readonly NotificationService _notificationService;

        public PlaylistViewModel(TrackLoaderService trackLoaderService, PlayerService playerService,
            NotificationService notificationService)
        {
            _trackLoaderService = trackLoaderService;
            _playerService = playerService;
            _notificationService = notificationService;

            Tracks = new ObservableCollection<Audio>();

            PlayCommmand = new RelayCommand(Play);
            ShuffleCommand = new RelayCommand(Shuffle);
            AddToLibraryCommand = new RelayCommand(AddToLibrary);
        }

        public AudioPlaylist Album { get; set; }

        public ObservableCollection<Audio> Tracks { get; set; }

        public RelayCommand PlayCommmand { get; }

        public RelayCommand ShuffleCommand { get; }

        public RelayCommand AddToLibraryCommand { get; }

        public async Task StartLoading(AudioPlaylist album)
        {
            if (album.Id == Album?.Id) return;

            Album = album;


            var tracks = await _trackLoaderService.GetLibraryTracks(0, 200);
            foreach (var track in tracks) Tracks.Add(track);

            Tracks.Add(new Audio {AccessKey = "space"});
        }

        public void Play()
        {
            if (Tracks.Count > 0) PlayTrack(Tracks[0]);
        }

        public void Shuffle()
        {
            _playerService.SetShuffle(true);
            int index = new Random().Next(0, Tracks.Count());
            PlayTrack(Tracks[index]);
        }

        public void AddToLibrary()
        {
            _notificationService.CreateNotification("Невозможно добавить плейлист",
                "Данная функция пока что недоступна.");
        }

        public void PlayTrack(Audio track)
        {
            _playerService.Play(track);
        }
    }
}