using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Fooxboy.MusicX.Core.Models.Music.Recommendations
{
    public class ResonseItem
    {
        [JsonProperty("items")]
        public List<BlockRecommendation> Items { get; set; }
    }
}
