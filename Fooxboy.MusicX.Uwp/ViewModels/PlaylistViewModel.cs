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
using Flurl.Http;

namespace Fooxboy.MusicX.Uwp.ViewModels
{
    public class PlaylistViewModel:BaseViewModel
    {

        public Album Album { get; set; }
        public string ArtistText { get; set; }
        public string Genres { get; set; }
        public bool AddButtonIsActive { get; set; }
        public bool DeleteButtonIsActive { get; set; }
        private  Api _api { get; set; }
        private NotificationService _notificationService;
        private CurrentUserService _currentUser;
        

        public ObservableCollection<Track> Tracks { get; set; }

        public RelayCommand PlayCommmand { get; }
        public RelayCommand ShuffleCommand { get; }
        public RelayCommand AddToLibraryCommand { get; }
        public RelayCommand DeleteCommand { get; }

        private IContainer _container;

        public PlaylistViewModel(IContainer container)
        {
            _container = container;
            _notificationService = _container.Resolve<NotificationService>();
            
            _api = container.Resolve<Api>();
            _currentUser = container.Resolve<CurrentUserService>();
            Tracks = new ObservableCollection<Track>();
            ArtistText = "Нет музыканта";
            Genres = "Без жанра";
            PlayCommmand = new RelayCommand(Play);
            ShuffleCommand = new RelayCommand(Shuffle);
            AddToLibraryCommand = new RelayCommand(AddToLibrary);
            DeleteCommand = new RelayCommand(async () => { await Delete(); });
            
        }

        public async Task StartLoading(Album album, bool isRecommendation = false)
        {
            try
            {
                if (album.OwnerId == _currentUser.UserId)
                {
                    DeleteButtonIsActive = true;
                    AddButtonIsActive = false;
                }
                else
                {
                    DeleteButtonIsActive = false;
                    AddButtonIsActive = true;
                }

                Changed("DeleteButtonIsActive");
                Changed("AddButtonIsActive");


                //if (album.Id == this.Album?.Id) return;

                this.Album = album;
                if (album.Artists.Count > 0)
                {

                    string s = string.Empty;
                    foreach (var trackArtist in album.Artists)
                    {
                        s += trackArtist.Name + ", ";
                    }

                    var artists = s.Remove(s.Length - 2);
                    this.ArtistText = artists;
                }
                else
                {
                    var owner = await _api.VKontakte.Users.Info.OwnerAsync(album.OwnerId);
                    this.ArtistText = owner.FirstName + " " + owner.LastName;
                }

                if (album.Genres.Count > 0) this.Genres = album.Genres[0];
                Changed("Album");
                Changed("ArtistText");



                var api = _container.Resolve<Api>();
                var loadingService = _container.Resolve<LoadingService>();
                loadingService.Change(true);


                var tracks = new List<Track>();

                if (isRecommendation)
                {

                    tracks = (await api.VKontakte.Music.Tracks.GetTracksAlbum(100, Album.Id, Album.AccessKey,
                        Album.OwnerId)).ToListTrack();
                }
                else
                {
                    tracks = (await api.VKontakte.Music.Tracks.GetAsync(200, 0, album.AccessKey, album.Id, album.OwnerId))
                        .ToListTrack();
                }
                    
                foreach (var track in tracks) Tracks.Add(track);

                Tracks.Add(new Track() {AccessKey = "space"});
                Changed("Tracks");
                loadingService.Change(false);
            }
            catch (FlurlHttpException)
            {
                _notificationService.CreateNotification("Ошибка сети", "Произошла ошибка подключения к сети.",
                    "Попробовать ещё раз", "Закрыть", new RelayCommand(
                        async () => { await this.StartLoading(album); }), new RelayCommand(() => { }));
            }
            catch (VkNet.Exception.CannotBlacklistYourselfException)
            {
                await StartLoading(album, true);
            }
            catch (Exception e)
            {
                _notificationService.CreateNotification("Ошибка при загрузке плейлиста", e.Message);
            }


        }

        public async Task Delete()
        {
            try
            {
                await _api.VKontakte.Music.Albums.Delete(Album.Id, Album.OwnerId);
                _notificationService.CreateNotification("Альбом удален", $"{Album.Title} удален из Вашей библиотеки.");
                DeleteButtonIsActive = false;
                Changed("DeleteButtonIsActive");
            }
            catch (FlurlHttpException)
            {
                _notificationService.CreateNotification("Ошибка сети", "Произошла ошибка подключения к сети.",
                    "Попробовать ещё раз", "Закрыть", new RelayCommand(
                        async () => { await this.Delete(); }), new RelayCommand(() => { }));
            }
            catch (Exception e)
            {
                _notificationService.CreateNotification("Невозможно удалить альбом", $"Ошибка: {e.Message}");
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
            try
            {
                await _api.VKontakte.Music.Albums.AddAsync(Album.Id, Album.OwnerId, Album.AccessKey);
                _notificationService.CreateNotification("Альбом добавлен",
                    $"{Album.Title} был добавлен в Вашу библиотеку.");

                AddButtonIsActive = false;
                Changed("AddButtonIsActive");
            }
            catch (FlurlHttpException)
            {
                _notificationService.CreateNotification("Ошибка сети", "Произошла ошибка подключения к сети.",
                    "Попробовать ещё раз", "Закрыть", new RelayCommand(
                        async () => { this.AddToLibrary(); }), new RelayCommand(() => { }));
            }
            catch (Exception e)
            {
                _notificationService.CreateNotification("Невозможно добавить альбом", $"Ошибка: {e.Message}");
            }

            
        }

        public void PlayTrack(Track track)
        {
            var playService = _container.Resolve<PlayerService>();
            playService.Play(this.Album, track, this.Tracks.ToList());
        }
    }
}
