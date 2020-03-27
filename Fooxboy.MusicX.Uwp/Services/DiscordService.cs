using DryIoc;
using Fooxboy.MusicX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fooxboy.MusicX.Uwp.Services
{
    public class DiscordService
    {
        private PlayerService _playerService;
        private Api _api;

        public DiscordService(Api api, PlayerService playerService)
        {
            _api = api;
            _playerService = playerService;
        }

        public void Init()
        {
           
            _playerService.PlayStateChangedEvent += PlayerServicePlayStateChanged;
            _playerService.TrackChangedEvent += PlayerServiceTrackChanged;
        }

        private async void PlayerServiceTrackChanged(object sender, EventArgs e)
        {
            await _api.Discord.SetTrack(_playerService.CurrentTrack.Title, _playerService.CurrentTrack.Artist);
            await _api.Discord.SetTime(_playerService.CurrentTrack.Duration);
        }

        private async void PlayerServicePlayStateChanged(object sender, EventArgs e)
        {
            await _api.Discord.SetPlayPause(_playerService.IsPlaying);
        }
    }
}
