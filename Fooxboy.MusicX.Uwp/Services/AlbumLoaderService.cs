using System.Collections.Generic;
using System.Threading.Tasks;
using VkNet.Abstractions;
using VkNet.Model.Attachments;

namespace Fooxboy.MusicX.Uwp.Services
{
    public class AlbumLoaderService
    {
        private readonly IVkApi _api;

        public AlbumLoaderService(IVkApi api)
        {
            _api = api;
        }

        public async Task<IEnumerable<AudioPlaylist>> GetLibraryAlbums(uint offset = 0, uint count = 5)
        {
            return await _api.Audio.GetPlaylistsAsync(_api.UserId.Value, count, offset).ConfigureAwait(false);
        }
    }
}