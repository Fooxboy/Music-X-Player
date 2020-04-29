﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Core;
using Fooxboy.MusicX.Core.Interfaces;
using Fooxboy.MusicX.Core.Models;
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

        public ArtistViewModel(Api api, NotificationService notification)
        {
            VisibleLoading = true;
            VisibleContent = false;
            this._api = api;
            this._notificationService = notification;
            Blocks = new ObservableCollection<IBlock>();
        }

        public async Task StartLoading(long artistId)
        {
            var artist = await _api.VKontakte.Music.Artists.GetAsync(artistId);
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
    }
}
