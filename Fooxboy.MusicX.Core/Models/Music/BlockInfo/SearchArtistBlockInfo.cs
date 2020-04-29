using System;
using System.Collections.Generic;
using System.Text;
using Fooxboy.MusicX.Core.Models.Music.ArtistInfo;
using Newtonsoft.Json;

namespace Fooxboy.MusicX.Core.Models.Music.BlockInfo
{
    public class SearchArtistBlockInfo
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("subtitle")]
        public string Subtitle { get; set; }
        [JsonProperty("image")]
        public List<PhotoArtistModel> Images { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        public Uri Image { get; set; }
        [JsonProperty("meta")]
        public MetaInfo Meta { get; set; }


        public class MetaInfo
        {
            [JsonProperty("icon")]
            public string Icon { get; set; }
            [JsonProperty("content_type")]
            public string ContentType { get; set; }
        }
    }
}
