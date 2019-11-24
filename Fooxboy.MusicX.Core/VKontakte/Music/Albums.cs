using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fooxboy.MusicX.Core.Interfaces;
using Fooxboy.MusicX.Core.VKontakte.Music.Converters;
using VkNet;

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
            var playlistsVk = await _api.Audio.GetPlaylistsAsync(id, count, offset);
            return playlistsVk.Select(playlist=> playlist.ToIAlbum()).ToList();
        }
        
        public List<IAlbum> Get(long id, uint count=10, uint offset = 0)
        {
            var playlistsVk = _api.Audio.GetPlaylists(id, count, offset);
            return playlistsVk.Select(playlist => playlist.ToIAlbum()).ToList();
        }
    }
}