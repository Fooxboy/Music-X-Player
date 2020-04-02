using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VkNet.Abstractions;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;

namespace Fooxboy.MusicX.Uwp.Services
{
    public class TrackLoaderService
    {
        private IVkApi _api;

        public TrackLoaderService(IVkApi api)
        {
            _api = api;
        }

        private async Task<IEnumerable<Audio>> Get(int offset = 0, int count = 20, string accessKey = null,
            long? albumId = null, long? ownerId = null)
        {
            return await _api.Audio.GetAsync(new AudioGetParams
            {
                AccessKey = accessKey,
                Count = count,
                Offset = offset,
                PlaylistId = albumId
            }).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Audio>> GetLibraryTracks(int offset = 0, int count = 20) =>
            await Get(offset, count);

        public async Task<IEnumerable<Audio>> GetPlaylistTracks(long albumId, long? ownerId = null, int offset = 0,
            int count = 20, string accessKey = null) => await Get(offset, count, accessKey, albumId, ownerId);

        public Task<IEnumerable<Audio>> GetFriendTracks() => throw new NotImplementedException();
    }
}