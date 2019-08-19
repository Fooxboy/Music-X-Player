using System;
using System.Collections.Generic;
using System.Text;
using Fooxboy.MusicX.Core.Models.Music.ArtistInfo;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NLog.Layouts;

namespace Fooxboy.MusicX.Core.Models.Music.Recommendations
{
    public class BlockRecommendation
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("subtitle")]
        public string SubTitle { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("count")]
        public long Count { get; set; }
        [JsonProperty("source")]
        public string Source { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("next_from")]
        public string NextFrom { get; set; }
        [JsonProperty("audios")]
        public List<AudioVkModel> Audios { get; set; }
        [JsonProperty("playlists")]
        public List<PlaylistInfoVkModel> Playlists { get; set; }
    }
}
