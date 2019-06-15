using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Core.Interfaces;
using Fooxboy.MusicX.Core.VKontakte.Music.Converters;

namespace Fooxboy.MusicX.Core.VKontakte.Music
{
    public static class Popular
    {
        public async static Task<IList<IAudioFile>> Tracks(int count=100, int offset=0, bool onlyEng = false)
        {
            if (StaticContent.VkApi == null) throw new Exception("Пользователь не авторизован");

            var music = await StaticContent.VkApi.Audio.GetPopularAsync(onlyEng: onlyEng,
                count: Convert.ToUInt32(count), offset: Convert.ToUInt32(offset));
            IList<IAudioFile> tracks = new List<IAudioFile>();
            foreach (var track in music) tracks.Add(track.ToIAudioFile());
            return tracks;
        }
    }
}
