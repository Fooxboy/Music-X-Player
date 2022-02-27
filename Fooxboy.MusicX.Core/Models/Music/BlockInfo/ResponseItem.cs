using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using VkNet.Model.Attachments;

namespace Fooxboy.MusicX.Core.Models.Music.BlockInfo
{
    public class ResponseItem
    {
        [JsonProperty("block")]
        public Block Block { get; set; }

        [JsonProperty("playlists")]
        public List<AudioPlaylist> Playlists { get; set; }
    }


}
