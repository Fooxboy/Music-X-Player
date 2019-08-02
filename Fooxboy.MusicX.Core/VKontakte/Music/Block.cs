using System.Threading.Tasks;
using Fooxboy.MusicX.Core.Models.Music.ArtistInfo;
using Newtonsoft.Json;
using VkNet.Utils;

namespace Fooxboy.MusicX.Core.VKontakte.Music
{
    public static class Block
    {
        public async static Task<Models.Music.BlockInfo.Block> GetById(string id, long count = 100, long offset = 0)
        {
            var parameters = new VkParameters();
            parameters.Add("count", count);
            parameters.Add("extended", 1);
            parameters.Add("block_id", id);
            parameters.Add("https", 1);
            parameters.Add("start_from", "");
            parameters.Add("lang", "ru");
            parameters.Add("access_token", StaticContent.VkApi.Token);
            parameters.Add("v", "5.103");
            var json = await StaticContent.VkApi.InvokeAsync("audio.getCatalogBlockById", parameters);
            var blockModel = JsonConvert.DeserializeObject<Response<Models.Music.BlockInfo.ResponseItem>>(json);
            return blockModel.response.Block;
        }
    }
}