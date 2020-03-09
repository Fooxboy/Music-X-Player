using DryIoc;
using Fooxboy.MusicX.Uwp.Converters;
using Fooxboy.MusicX.Uwp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fooxboy.MusicX.Uwp.Services
{
    public class TrackLoaderService
    {

        private async Task<List<Track>> Get(int offset = 0, int count = 20, string accessKey = null, long? albumId = null, long? ownerId = null)
        {
            var api = Container.Get.Resolve<Core.Api>();

            var tracks = await api.VKontakte.Music.Tracks.GetAsync(count, offset, accessKey, albumId, ownerId);

            return tracks.ToListTrack();
        }

        public async Task<List<Track>> GetLibraryTracks(int offset = 0, int count = 20) => await Get(offset, count);
        public async Task<List<Track>> GetPlaylistTracks(long albumId, long? ownerId = null, int offset = 0, int count = 20, string accessKey = null) => await Get(offset, count, accessKey, albumId, ownerId);
        public async Task<List<Track>> GetFriendTracks() => throw new NotImplementedException();
    }
}
