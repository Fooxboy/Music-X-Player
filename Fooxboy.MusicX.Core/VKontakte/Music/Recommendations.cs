using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Core.Interfaces;
using Fooxboy.MusicX.Core.VKontakte.Music.Converters;

namespace Fooxboy.MusicX.Core.VKontakte.Music
{
    public static class Recommendations
    {
        public async static Task<IList<IAudioFile>> Tracks(int count, int offset)
        {
            if (StaticContent.VkApi == null) throw new Exception("Пользователь не авторизован");
            var music = await StaticContent.VkApi.Audio.GetRecommendationsAsync(userId: StaticContent.UserId,  count: Convert.ToUInt32(count), offset:Convert.ToUInt32(offset));
            IList<IAudioFile> tracks = new List<IAudioFile>();
            foreach (var track in music) tracks.Add(track.ToIAudioFile());
            return tracks;
        }
    }
}
