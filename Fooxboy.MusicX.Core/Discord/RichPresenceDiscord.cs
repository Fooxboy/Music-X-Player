using DiscordRPC;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Fooxboy.MusicX.Core.Discord
{
    public class RichPresenceDiscord
    {
        private readonly DiscordRpcClient _client;
        private RichPresence _currentRichPresence;
        private int _pipe = -1;
        public RichPresenceDiscord()
        {
            _client = new DiscordRpcClient("652832654944894976", pipe: _pipe);
            _client.OnReady += _client_OnReady;
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

        private void _client_OnReady(object sender, DiscordRPC.Message.ReadyMessage args)
        {
            Debug.WriteLine("ГОТОВ");
        }

        public async Task InitAsync()
        {
            //TODO: Discord не работает в приложениях uwp.
            if (_client.IsInitialized) return;
           await Task.Run(() =>
           {
               var result = _client.Initialize();
           });
        }

        private async Task SetAsync()
        {
            try
            {
                await Task.Run(() =>
                {
                    try
                    {
                        _client.SetPresence(_currentRichPresence);
                        _client.Invoke();
                    }catch
                    {

                    }
                   
                });
            }catch
            {

            }
           
        }

        public async Task SetTrack(string title, string artist)
        {
            try
            {
                _currentRichPresence.State = $"{artist} - {title}";
                await SetAsync();
            }catch(Exception e)
            {

            }
            
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
