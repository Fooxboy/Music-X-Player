using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Fooxboy.MusicX.Core.Server.Models
{
    public class RootResponse<T>
    {
        [JsonProperty("status")]
        public bool Status { get; set; }
        [JsonProperty("result")]
        public T Result { get; set; }
        [JsonProperty("error")]
        public Error Error { get; set; }
    }
}
