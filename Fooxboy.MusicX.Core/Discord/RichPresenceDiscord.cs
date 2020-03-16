using DiscordRPC;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fooxboy.MusicX.Core.Discord
{
    public class RichPresenceDiscord
    {
        private readonly DiscordRpcClient _client;
        private RichPresence _currentRichPresence;
        public RichPresenceDiscord()
        {
            _client = new DiscordRpcClient("652832654944894976");
            _currentRichPresence = new RichPresence();
            _currentRichPresence.Assets = new Assets();
            _currentRichPresence.Assets.LargeImageKey = "album";
            _currentRichPresence.Assets.LargeImageText = "Сейчас слушает";
            _currentRichPresence.Assets.SmallImageKey = "pause";
            _currentRichPresence.Assets.SmallImageText = "Пауза";
            _currentRichPresence.Details = "Пауза";
            _currentRichPresence.State = "";
            _currentRichPresence.Timestamps = new Timestamps();
        }

        public async Task InitAsync()
        {
            if (_client.IsInitialized) return;
           await Task.Run(() =>
           {
               var result = _client.Initialize();
           });
        }

        private async Task SetAsync()
        {
            await Task.Run(() =>
            {
                _client.SetPresence(_currentRichPresence);
                _client.Invoke();
            });
        }

        public async Task SetTrack(string title, string artist)
        {
            _currentRichPresence.State = $"{artist} - {title}";
            await SetAsync();
        }

        public async Task SetPlayPause(bool state)
        {
            if (state)
            {
                _currentRichPresence.Details = "Слушает";
                _currentRichPresence.Assets.SmallImageKey = "play_discord";
                _currentRichPresence.Assets.SmallImageText = "Сейчас слушает";
            }
            else
            {
                _currentRichPresence.Details = "Пауза";
                _currentRichPresence.Assets.SmallImageKey = "pause";
                _currentRichPresence.Assets.SmallImageText = "Музыка на паузе";
            }

            await SetAsync();
        }

        public async Task SetTime(TimeSpan timeTrack)
        {
            var endTime = DateTime.Now.Add(timeTrack);
            _currentRichPresence.Timestamps.End = endTime;
            _currentRichPresence.Timestamps.Start = DateTime.Now;
            await SetAsync();
        }

        public void Close()
        {
            _client.Dispose();
        }
    }
}
