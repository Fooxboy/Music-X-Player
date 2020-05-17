using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fooxboy.MusicX.Core.Interfaces;
using Fooxboy.MusicX.Core.Models.Music;
using Fooxboy.MusicX.Core.VKontakte.Music.Converters;
using Newtonsoft.Json;
using VkNet;
using VkNet.Utils;

namespace Fooxboy.MusicX.Core.VKontakte.Music
{
    public class Albums
    {
        private readonly VkApi _api;
        public Albums(VkApi api)
        {
            _api = api;
        }
        public async Task<List<IAlbum>> GetAsync(long id, uint count=10, uint offset = 0)
        {
            Api.Logger.Trace("[CORE] Запрос audio.getPlaylists...");
            var playlistsVk = await _api.Audio.GetPlaylistsAsync(id, count, offset);
            Api.Logger.Trace($"[CORE] Ответ получен: {playlistsVk.TotalCount} элементов.");
            return playlistsVk.Select(playlist => playlist.ToIAlbum()).ToList();
        }
        
        public List<IAlbum> Get(long id, uint count=10, uint offset = 0)
        {
            var playlistsVk = _api.Audio.GetPlaylists(id, count, offset);
            return playlistsVk.Select(playlist => playlist.ToIAlbum()).ToList();
        }

        public async Task AddAsync(long albumId, long ownerId, string accessKey)
        {
            Api.Logger.Trace("[CORE] Запрос audio.followPlaylist...");

            var parameters = new VkParameters
            {
                {"v", "5.103"},
                {"lang", "ru"},
                {"access_token", _api.Token},
                {"playlist_id", albumId.ToString()},
                {"owner_id", ownerId.ToString() },
                
            };

            if(accessKey != null) parameters.Add("access_key", accessKey);

            var json = await _api.InvokeAsync("audio.followPlaylist", parameters);
            Api.Logger.Trace($"[CORE] Ответ получен, результат: {json}");

        }

        public async Task Delete(long playlistId, long ownerId)
        {
            Api.Logger.Trace("[CORE] Запрос audio.deletePlaylistAsync...");

            var result = await _api.Audio.DeletePlaylistAsync(ownerId, playlistId);
            Api.Logger.Trace($"[CORE] Ответ получен, результат: {result}");

        }
    }
}