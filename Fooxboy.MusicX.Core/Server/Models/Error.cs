using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Fooxboy.MusicX.Core.Server.Models
{
    public class Error
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("code")]
        public int Code { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
