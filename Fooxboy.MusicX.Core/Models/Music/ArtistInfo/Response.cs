using System.Collections.Generic;
using Newtonsoft.Json;

namespace Fooxboy.MusicX.Core.Models.Music.ArtistInfo
{
    public class Response
    {
        [JsonProperty("items")]
        public List<ItemInfo> Items { get; set; }
    }
}