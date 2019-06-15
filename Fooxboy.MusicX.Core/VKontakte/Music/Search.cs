using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Core.Interfaces;
using Fooxboy.MusicX.Core.VKontakte.Music.Converters;

namespace Fooxboy.MusicX.Core.VKontakte.Music
{
    public static class Search
    {
        public async static Task<IList<IAudioFile>> Tracks(string text, long count=20, long offset=0, bool withLyrics= false,
            bool performerOnly = false, bool searchInLibrary = true)
        {
            if (StaticContent.VkApi == null) throw new Exception("Пользователь не авторизован");
            var music = await StaticContent.VkApi.Audio.SearchAsync(new VkNet.Model.RequestParams.AudioSearchParams()
            {
                Query = text,
                Autocomplete = true,
                Count = count,
                Offset = offset,
                Lyrics= withLyrics,
                PerformerOnly = performerOnly,
                SearchOwn = searchInLibrary,
                Sort = VkNet.Enums.AudioSort.Popularity
            });

            IList<IAudioFile> tracks = new List<IAudioFile>();
            foreach (var track in music) tracks.Add(track.ToIAudioFile());
            return tracks;

        }
    }
}
