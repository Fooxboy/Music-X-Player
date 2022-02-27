using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Core.Interfaces;
using Fooxboy.MusicX.Core.Models.Music;
using Fooxboy.MusicX.Core.VKontakte.Music.Converters;
using Newtonsoft.Json;
using VkNet;
using VkNet.Utils;

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
            Api.Logger.Trace($"[CORE] Запрос к audio.search");

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

            Api.Logger.Trace($"[CORE] Ответ получен: {music.TotalCount} элементов.");

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


        public async Task<List<IBlock>> GetResultsAsync(string text)
        {
            Api.Logger.Trace($"[CORE] Запрос к audio.getCatalog...");

            var parameters = new VkParameters
            {
                //{"v", "5.103"},
                {"v", "5.131"},

                {"lang", "ru"},
                {"extended", "1"},
                {"access_token", _api.Token},
                {"query", text}
            };

            var json = await _api.InvokeAsync("audio.getCatalog", parameters);
            var model = JsonConvert.DeserializeObject<Response<ResponseItem>>(json);
            Api.Logger.Trace($"[CORE] Ответ получен: {model.response.Items.Count} элементов.");

            return model.response.Items.Select(block => block.ConvertToIBlock()).ToList();

        }

        public List<IBlock> GetResults(string text)
        {
            var parameters = new VkParameters
            {
                {"v", "5.103"},
                {"lang", "ru"},
                {"extended", "1"},
                {"access_token", _api.Token},
                {"count", "10" },
                {"query", text}
            };

            var json = _api.Invoke("audio.getCatalog", parameters);
            var model = JsonConvert.DeserializeObject<Response<ResponseItem>>(json);
            return model.response.Items.Select(block => block.ConvertToIBlock()).ToList();

        }
    }
}
