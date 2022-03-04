using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flurl.Http;
using Fooxboy.MusicX.Core;
using Fooxboy.MusicX.Core.Interfaces;
using Fooxboy.MusicX.Uwp.Converters;
using Fooxboy.MusicX.Uwp.Models;
using Fooxboy.MusicX.Uwp.Services;

namespace Fooxboy.MusicX.Uwp.ViewModels
{
    public class RecommendationsViewModel:BaseViewModel
    {
        private readonly Api _api;
        private readonly PlayerService _player;
        private List<Track> _tracksForYou;
        private readonly NotificationService _notificationService;
        private readonly ILoggerService _logger;
        public RecommendationsViewModel(Api api, PlayerService player, NotificationService notificationService, ILoggerService logger)
        {
            _api = api;
            _player = player;
            _logger = logger;
            _notificationService = notificationService;
            this.Blocks = new ObservableCollection<IBlock>();
            PlayAllCommand = new RelayCommand(PlayAll);
            PlayShuffleCommand = new RelayCommand(PlayShuffle);
            VisibileContent = false;
            VisibleLoading = true;
        }

        public ObservableCollection<IBlock> Blocks { get; set; }

        public string ForYouString { get; set; }
        public string PatchImage { get; set; }

        public bool VisibleLoading { get; set; }
        public bool VisibileContent { get; set; }

        public RelayCommand PlayAllCommand { get; set; }
        public RelayCommand PlayShuffleCommand { get; set; }


        public async void PlayAll()
        {
            await _player.Play(0, _tracksForYou);
        }

        public async void PlayShuffle()
        {
            await _player.Play(new Random().Next(0, _tracksForYou.Count), _tracksForYou);
            _player.SetShuffle(true);
        }

        public async Task StartLoading()
        {
            try
            {
                _logger.Trace("Загрузка рекомендаций..");

                var blocks = new List<IBlock>();
                var blocksOne = await _api.VKontakte.Music.Recommendations.GetAsync();
                var br = await _api.VKontakte.Music.Recommendations.GetAlghoritmsPlaylists();
                blocks.Add(br);
                blocks.AddRange(blocksOne);
                _logger.Info($"Загружено {blocks.Count} блоков рекомендаций.");
                var blockForYou = blocks.Single(b => b.Source == "recoms_recoms");
                ForYouString = blockForYou.Subtitle;
                _tracksForYou = await blockForYou.Tracks.ToListTrack();
                PatchImage = blockForYou.Tracks[new Random().Next(0, blockForYou.Tracks.Count)].Album?.Cover;
                Changed("ForYouString");
                Changed("PatchImage");

                VisibileContent = true;
                VisibleLoading = false;
                Changed("VisibileContent");
                Changed("VisibleLoading");

                blocks.Remove(blockForYou);
                foreach (var block in blocks)
                {
                    this.Blocks.Add(block);
                }

                Changed("Blocks");
            }
            catch (FlurlHttpException e)
            {
                _logger.Error("Ошибка сети", e);
                _notificationService.CreateNotification("Ошибка сети", "Произошла ошибка подключения к сети.",
                    "Попробовать ещё раз", "Закрыть", new RelayCommand(
                        async () => { await this.StartLoading(); }), new RelayCommand(() => { }));
            }
            catch (Exception e)
            {
                _logger.Error("Неизвестная ошибка", e);
                _notificationService.CreateNotification("Невозможно получить рекомендации.", $"Ошибка: {e.Message}");
            }

        }
    }
}
