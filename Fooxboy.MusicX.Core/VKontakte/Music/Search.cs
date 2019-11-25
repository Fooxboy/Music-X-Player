using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Core.Interfaces;
using Fooxboy.MusicX.Core.VKontakte.Music.Converters;
using VkNet;

namespace Fooxboy.MusicX.Core.VKontakte.Music
{
    public class Search
    {
        private readonly VkApi _api;
        public Search(VkApi api)
        {
            _api = api;
        }
        public async Task<List<ITrack>> TracksAsync(string text, long count=20, long offset=0, bool withLyrics= false,
            bool performerOnly = false, bool searchInLibrary = true)
        {
            var music = await _api.Audio.SearchAsync(new VkNet.Model.RequestParams.AudioSearchParams()
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
            return music.Select(track => track.ToITrack()).ToList();

        }

        public  IList<ITrack> Tracks(string text, long count = 20, long offset = 0, bool withLyrics = false,
            bool performerOnly = false, bool searchInLibrary = true)
        {
            var music = _api.Audio.Search(new VkNet.Model.RequestParams.AudioSearchParams()
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
            return music.Select(track => track.ToITrack()).ToList();
        }
    }
}
