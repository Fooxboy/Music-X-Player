using Fooxboy.MusicX.Core.Interfaces;
using Fooxboy.MusicX.Core.Models.Music;
using Fooxboy.MusicX.Core.VKontakte.Music.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VkNet;
using VkNet.Utils;

namespace Fooxboy.MusicX.Core.VKontakte.Music
{
    public class Catalog
    {
        private readonly VkApi _api;
        public Catalog(VkApi api)
        {
            _api = api;
        }


        public async Task<string> GetSectionById(string sectionId)
        {

            var parameters = new VkParameters
            {
                {"section_id", sectionId},

                {"lang", "ru"},
                {"access_token", _api.Token},
                {"v", "5.131"},

                //{"v", "5.103"}
            };
            var json = await _api.InvokeAsync("catalog.getSection", parameters);
            return json;
        }

        public async Task<IBlock> GetBlockItems(string blockId)
        {
            var parameters = new VkParameters
            {
                {"block_id", blockId},

                {"lang", "ru"},
                {"access_token", _api.Token},
                {"v", "5.131"},

                //{"v", "5.103"}
            };
            var json = await _api.InvokeAsync("catalog.getBlockItems", parameters);

            var blockModel = JsonConvert.DeserializeObject<Response<Models.Music.BlockInfo.ResponseItem>>(json);

            blockModel.response.Block.Playlists = blockModel.response.Playlists;
            blockModel.response.Block.Type = blockModel.response.Block.DataType;

            return blockModel.ConvertToIBlock();

        }
    }
}
