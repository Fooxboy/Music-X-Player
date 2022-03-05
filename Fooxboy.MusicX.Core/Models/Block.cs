using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fooxboy.MusicX.Core.Models
{
    public class Block
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("data_type")]
        public string DataType { get; set; }

        [JsonProperty("layout")]
        public Layout Layout { get; set; }

        [JsonProperty("catalog_banner_ids")]
        public List<int> CatalogBannerIds { get; set; }

        [JsonProperty("buttons")]
        public List<Button> Buttons { get; set; }

        [JsonProperty("next_from")]
        public string NextFrom { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("listen_events")]
        public List<string> ListenEvents { get; set; }

        [JsonProperty("playlists_ids")]
        public List<string> PlaylistsIds { get; set; }

        [JsonProperty("badge")]
        public Badge Badge { get; set; }

        [JsonProperty("audios_ids")]
        public List<string> AudiosIds { get; set; }
    }
}
