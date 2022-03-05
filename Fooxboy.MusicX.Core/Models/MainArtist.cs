using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fooxboy.MusicX.Core.Models
{
    public class MainArtist
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("domain")]
        public string Domain { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
