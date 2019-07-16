using System;
using Newtonsoft.Json;

namespace Fooxboy.MusicX.Core.Models.Music.ArtistInfo
{
    public class PhotoArtistModel
    {
        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("width")]
        public long Width { get; set; }

        [JsonProperty("height")]
        public long Height { get; set; }
    }
}