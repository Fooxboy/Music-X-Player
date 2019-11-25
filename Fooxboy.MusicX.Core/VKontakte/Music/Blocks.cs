using System.Threading.Tasks;
using Fooxboy.MusicX.Core.Interfaces;
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
            var parameters = new VkParameters();
            parameters.Add("count", count);
            parameters.Add("extended", 1);
            parameters.Add("block_id", id);
            parameters.Add("https", 1);
            parameters.Add("start_from", "");
            parameters.Add("lang", "ru");
            parameters.Add("access_token", _api.Token);
            parameters.Add("v", "5.103");
            var json = await _api.InvokeAsync("audio.getCatalogBlockById", parameters);
            var blockModel = JsonConvert.DeserializeObject<Response<Models.Music.BlockInfo.ResponseItem>>(json);
            return blockModel.ConvertToIBlock();
        }
        
        public IBlock Get(string id, long count = 100, long offset = 0)
        {
            var parameters = new VkParameters();
            parameters.Add("count", count);
            parameters.Add("extended", 1);
            parameters.Add("block_id", id);
            parameters.Add("https", 1);
            parameters.Add("start_from", "");
            parameters.Add("lang", "ru");
            parameters.Add("access_token", _api.Token);
            parameters.Add("v", "5.103");
            var json = _api.Invoke("audio.getCatalogBlockById", parameters);
            var blockModel = JsonConvert.DeserializeObject<Response<Models.Music.BlockInfo.ResponseItem>>(json);
            return blockModel.ConvertToIBlock();
        }
    }
}