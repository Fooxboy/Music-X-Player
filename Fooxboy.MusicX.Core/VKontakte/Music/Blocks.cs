using System.Threading.Tasks;
using Fooxboy.MusicX.Core.Interfaces;
using Fooxboy.MusicX.Core.Models.Music;
using Fooxboy.MusicX.Core.Models.Music.ArtistInfo;
using Fooxboy.MusicX.Core.VKontakte.Music.Converters;
using Newtonsoft.Json;
using VkNet;
using VkNet.Utils;

namespace Fooxboy.MusicX.Core.VKontakte.Music
{
    public class Blocks
    {
        private readonly VkApi _api;
        public Blocks(VkApi api)
        {
            _api = api;
        }
        public async Task<IBlock> GetAsync(string id, long count = 100, long offset = 0)
        {
            Api.Logger.Trace("[CORE] Запрос к audio.getCatalogBlockById...");

            var parameters = new VkParameters
            {
                {"count", count},
                {"extended", 1},
                {"block_id", id},
                {"https", 1},
                {"start_from", ""},
                {"lang", "ru"},
                {"access_token", _api.Token},
                {"v", "5.131"},

                //{"v", "5.103"}
            };
            var json = await _api.InvokeAsync("audio.getCatalogBlockById", parameters);
            var blockModel = JsonConvert.DeserializeObject<Response<Models.Music.BlockInfo.ResponseItem>>(json);
            Api.Logger.Trace($"[CORE] Ответ получен: {blockModel.response.Block.Count} результатов.");

            return blockModel.ConvertToIBlock();
        }
        
        public IBlock Get(string id, long count = 100, long offset = 0)
        {
            var parameters = new VkParameters
            {
                {"count", count},
                {"extended", 1},
                {"block_id", id},
                {"https", 1},
                {"start_from", ""},
                {"lang", "ru"},
                {"access_token", _api.Token},
                {"v", "5.131"},

                //{"v", "5.103"}
            };
            var json = _api.Invoke("audio.getCatalogBlockById", parameters);
            var blockModel = JsonConvert.DeserializeObject<Response<Models.Music.BlockInfo.ResponseItem>>(json);
            return blockModel.ConvertToIBlock();
        }
    }
}