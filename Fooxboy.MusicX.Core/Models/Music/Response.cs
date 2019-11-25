using System.Collections.Generic;
using Newtonsoft.Json;

namespace Fooxboy.MusicX.Core.Models.Music
{
    public class Response
    {
        [JsonProperty("response")]
        public ResponseItem response { get; set; }
    }

    public class Response<T>
    {
        [JsonProperty("response")]
        public T response { get; set; }
    }

    public class ResponseItem
    {
        [JsonProperty("items")]
        public List<Music.BlockInfo.Block> Items { get; set; }
    }
}