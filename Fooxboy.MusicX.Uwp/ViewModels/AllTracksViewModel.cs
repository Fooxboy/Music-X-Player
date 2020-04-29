﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flurl.Http;
using Fooxboy.MusicX.Core;
using Fooxboy.MusicX.Uwp.Converters;
using Fooxboy.MusicX.Uwp.Models;
using Fooxboy.MusicX.Uwp.Services;

namespace Fooxboy.MusicX.Uwp.ViewModels
{
    public class AllTracksViewModel:BaseViewModel
    {
        private PlayerService _player;
        private Api _api;
        private NotificationService _notify;
        public AllTracksViewModel(PlayerService player, Api api, NotificationService notify)
        {
            _player = player;
            _api = api;
            _notify = notify;
            Tracks = new ObservableCollection<Track>();
            VisibleLoading = true;
            VisibleContent = false;
        }

        public string TitlePage { get; set; }
        public ObservableCollection<Track> Tracks { get; set; }

        public bool VisibleLoading { get; set; }
        public bool VisibleContent { get; set; }


        public async Task StartLoading(object[] data)
        {
            try
            {
                var type = (string) data[0];
                if (type == "block")
                {
                    var blockId = (string) data[1];

                    var fullBlock = await _api.VKontakte.Music.Blocks.GetAsync(blockId);

                    TitlePage = fullBlock.Title;
                    Changed("TitlePage");

                    VisibleLoading = false;
                    VisibleContent = true;

                    Changed("VisibleLoading");
                    Changed("VisibleContent");

                    foreach (var track in fullBlock.Tracks)
                    {
                        Tracks.Add(track.ToTrack());
                    }
                }
            }
            catch (FlurlHttpException)
            {
                _notify.CreateNotification("Ошибка сети", "Произошла ошибка подключения к сети.", "Попробовать ещё раз",
                    "Закрыть", new RelayCommand(
                        async () => { await this.StartLoading(data); }), new RelayCommand(() => { }));
            }
            catch (Exception e)
            {
                _notify.CreateNotification("Ошибка при загрузке списка треков", $"Ошибка: {e.Message}");
            }
        }

        public void PlayTrack(Track track)
        {
            var tempTrack = Tracks.Single(t => t.Id == track.Id && t.Url == track.Url);
            var index = Tracks.IndexOf(tempTrack);

            _player.Play(new Album(), index, Tracks.ToList());
        }
    }
}
