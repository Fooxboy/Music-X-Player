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
                {"v", "5.103"},
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
                {"v", "5.103"},
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
        
    }
}
