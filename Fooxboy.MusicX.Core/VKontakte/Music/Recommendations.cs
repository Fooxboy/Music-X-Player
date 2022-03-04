using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Core.Interfaces;
using Fooxboy.MusicX.Core.Models;
using Fooxboy.MusicX.Core.Models.Music;
using Fooxboy.MusicX.Core.Models.Music.ArtistInfo;
using Fooxboy.MusicX.Core.VKontakte.Music.Converters;
using Newtonsoft.Json;
using VkNet;
using VkNet.Utils;

namespace Fooxboy.MusicX.Core.VKontakte.Music
{
    public class Recommendations
    {
        private readonly VkApi _api;
        public Recommendations(VkApi api)
        {
            _api = api;
        }
        public async Task<List<IBlock>> GetAsync()
        {
            Api.Logger.Trace("[CORE] Запрос к audio.getCatalog (Рекомендации)");
            var parameters = new VkParameters
            {
               // {"v", "5.103"},
                {"v", "5.131"},
                {"lang", "ru"},
                {"extended", "1"},
                {"access_token", _api.Token},
                {"count", "10"},
                {"fields", "first_name_gen, photo_100"}
            };

            var json = await _api.InvokeAsync("audio.getCatalog", parameters);
            var model = JsonConvert.DeserializeObject<Response<ResponseItem>>(json);
            Api.Logger.Trace($"[CORE] Ответ получен: {model.response.Items.Count} элементов.");

            return model.response.Items.Select(block => block.ConvertToIBlock()).ToList();
        }
        
        public  List<IBlock> Get()
        {
            var parameters = new VkParameters
            {
                //{"v", "5.103"},
                {"v", "5.131"},

                {"lang", "ru"},
                {"extended", "1"},
                {"access_token", _api.Token},
                {"count", "10"},
                {"fields", "first_name_gen, photo_100"}
            };

            var json =  _api.Invoke("audio.getCatalog", parameters);
            var model = JsonConvert.DeserializeObject<Response<ResponseItem>>(json);
            return model.response.Items.Select(block => block.ConvertToIBlock()).ToList();
        }

        public async Task<string> GetHomeAsync()
        {
            Api.Logger.Trace("[CORE] Запрос к audio.getCatalog (Рекомендации)");
            var parameters = new VkParameters
            {
               // {"v", "5.103"},
                {"v", "5.131"},
                {"lang", "ru"},
                {"extended", "1"},
                {"access_token", _api.Token},
                {"count", "10"},
                {"fields", "first_name_gen, photo_100"}
            };

            var json = await _api.InvokeAsync("catalog.getAudioStory?", parameters);

            return json;
           /* var model = JsonConvert.DeserializeObject<Response<ResponseItem>>(json);
            Api.Logger.Trace($"[CORE] Ответ получен: {model.response.Items.Count} элементов.");

            return model.response.Items.Select(block => block.ConvertToIBlock()).ToList();*/
        }

        public async Task<IBlock> GetAlghoritmsPlaylists()
        {
            var parameters = new VkParameters
            {
                {"block_id", "PUlQVA8GR0R3W0tMF0QHBz8HAAVBRzQUIwgGG0YWR0R-SVNFBQxcUHJcUUAZFl5EfEkOE1tRGQcqSUVUARZRV2pJWEMXDlpKZFlfVA8HW15xXV1BDQIW"},

                {"lang", "ru"},
                {"access_token", _api.Token},
                {"v", "5.131"},

                //{"v", "5.103"}
            };
            var json = await _api.InvokeAsync("catalog.getBlockItems", parameters);

            var blockModel = JsonConvert.DeserializeObject<Response<Models.Music.BlockInfo.ResponseItem>>(json);

            blockModel.response.Block.Playlists = blockModel.response.Playlists;
            blockModel.response.Block.Type = "alghoritm";
            blockModel.response.Block.Title = "СОБРАНО АЛГОРИТМАМИ";

            return blockModel.ConvertToIBlock();
        }
        
    }
}
