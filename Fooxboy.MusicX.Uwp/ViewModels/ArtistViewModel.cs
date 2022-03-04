using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flurl.Http;
using Fooxboy.MusicX.Core;
using Fooxboy.MusicX.Core.Interfaces;
using Fooxboy.MusicX.Core.Models;
using Fooxboy.MusicX.Uwp.Converters;
using Fooxboy.MusicX.Uwp.Models;
using Fooxboy.MusicX.Uwp.Services;

namespace Fooxboy.MusicX.Uwp.ViewModels
{
    public class ArtistViewModel:BaseViewModel
    {
        public string PhotoUrl { get; set; }

        public string Name { get; set; }

        public bool VisibleLoading { get; set; }
        public bool VisibleContent { get; set; }

        public RelayCommand PlayArtist { get; set; }

        public ObservableCollection<IBlock> Blocks { get; set; }

        private Api _api;
        private NotificationService _notificationService;
        private PlayerService _player;
        private ILoggerService _logger;

        public ArtistViewModel(Api api, NotificationService notification, PlayerService player, ILoggerService logger)
        {
            VisibleLoading = true;
            VisibleContent = false;
            this._api = api;
            this._logger = logger;
            this._notificationService = notification;
            this._player = player;
            Blocks = new ObservableCollection<IBlock>();
            PlayArtist = new RelayCommand(PlayArtistMusic);

        }

        public async void PlayArtistMusic()
        {
            var blockPopular = Blocks.SingleOrDefault(b => b.Source == "artist_top_audios");

            if (blockPopular != null)
            {
                var tracks = await blockPopular.Tracks.ToListTrack();

                await _player.Play(0, tracks);
            }
        }

        public async Task StartLoading(long artistId)
        {
            try
            {
                _logger.Trace("Загрузка информации об артисте...");
                var artist = await _api.VKontakte.Music.Artists.GetAsync(artistId);
                _logger.Info($"Загружен артист {artist.Name} с {artist.Blocks.Count} блоками информации.");
                PhotoUrl = artist.Banner;
                Name = artist.Name;
                var blockF = artist.Blocks.SingleOrDefault(b => b.Source == "artist_info");
                artist.Blocks.Remove(blockF);

                foreach (var block in artist.Blocks)
                {
                    Blocks.Add(block);
                }

                Changed("PhotoUrl");
                Changed("Name");
                Changed("Blocks");
                VisibleLoading = false;
                VisibleContent = true;
                Changed("VisibleLoading");
                Changed("VisibleContent");
            }
            catch (FlurlHttpException e)
            {
                _logger.Error("Ошибка сети", e);
                _notificationService.CreateNotification("Ошибка сети", "Произошла ошибка подключения к сети.",
                    "Попробовать ещё раз", "Закрыть", new RelayCommand(
                        async () => { await this.StartLoading(artistId); }), new RelayCommand(() => { }));
            }
            catch (Exception e)
            {
                _logger.Error("Неизвестная ошибка", e);
                _notificationService.CreateNotification("Ошибка при загрузке карточки музыканта", $"Ошибка: {e.Message}");
            }
            
        }
    }
}
