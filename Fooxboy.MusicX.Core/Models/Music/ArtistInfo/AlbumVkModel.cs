using System;
using Newtonsoft.Json;

namespace Fooxboy.MusicX.Core.Models.Music.ArtistInfo
{
    public class AlbumVkModel
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("owner_id")]
        public long OwnerId { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("access_key")]
        public string AccessKey { get; set; }

        [JsonProperty("thumb")]
        public Covers Thumb { get; set; }
        
        public  class Covers
        {
            [JsonProperty("photo_34")]
            public Uri Photo34 { get; set; }

            [JsonProperty("photo_68")]
            public Uri Photo68 { get; set; }

            [JsonProperty("photo_135")]
            public Uri Photo135 { get; set; }

            [JsonProperty("photo_270")]
            public Uri Photo270 { get; set; }

            [JsonProperty("photo_300")]
            public Uri Photo300 { get; set; }

            [JsonProperty("photo_600")]
            public Uri Photo600 { get; set; }

            [JsonProperty("width")]
            public long Width { get; set; }

            [JsonProperty("height")]
            public long Height { get; set; }
        }
    }
}