using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public RecommendationsViewModel(Api api, PlayerService player)
        {
            _api = api;
            _player = player;
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


        public void PlayAll()
        {
            var albumTemp = new Album();
            albumTemp.Tracks = new List<ITrack>();
            
            _player.Play(albumTemp, 0, _tracksForYou);
        }

        public void PlayShuffle()
        {
            _player.Play(new Album(), new Random().Next(0, _tracksForYou.Count), _tracksForYou);
            _player.SetShuffle(true);
        }

        public async Task StartLoading()
        {
            var blocks = await _api.VKontakte.Music.Recommendations.GetAsync();
            var blockForYou = blocks.Single(b => b.Source == "recoms_recoms");
            ForYouString = blockForYou.Subtitle;
            _tracksForYou = blockForYou.Tracks.ToListTrack();
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
    }
}
