using Fooxboy.MusicX.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Core.VKontakte.Music.Converters;
using VkNet;

namespace Fooxboy.MusicX.Core.VKontakte.Music
{
    public class Tracks
    {
        private readonly VkApi _api;
        public Tracks(VkApi api)
        {
            _api = api;
        }
        public async Task<List<ITrack>> GetAsync(int count = 10, int offset =0, string accessKey = null, long? playlistId =null, long? ownerId = null)
        {
            var music = await _api.Audio.GetAsync(new VkNet.Model.RequestParams.AudioGetParams()
            {
                Count = count,
                Offset = offset, 
                AccessKey =accessKey,
                PlaylistId = playlistId,
                OwnerId = ownerId
            });

            var l = new List<ITrack>();
            foreach (var audio in music)
            {
                l.Add(audio.ToITrack());
            }
            return l;
        }
        
        public List<ITrack> Get(int count = 10, int offset =0, string accessKey = null, long? playlistId =null, long? ownerId = null)
        {
            var music =  _api.Audio.Get(new VkNet.Model.RequestParams.AudioGetParams()
            {
                Count = count,
                Offset = offset, 
                AccessKey =accessKey,
                PlaylistId = playlistId,
                OwnerId = ownerId
            });

            var l = new List<ITrack>();
            foreach (var audio in music)
            {
                l.Add(audio.ToITrack());
            }
            return l;
        }
    }
}
