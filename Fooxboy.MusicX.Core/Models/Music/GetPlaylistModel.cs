using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using VkNet.Model;
using VkNet.Model.Attachments;

namespace Fooxboy.MusicX.Core.Models.Music
{
    public class GetPlaylistModel
    {
        [JsonProperty("groups")]
        public List<Group> Groups { get; set; }
        [JsonProperty("playlist")]
        public AudioPlaylist Playlist { get; set; }
        [JsonProperty("audios")]
        public List<Audio> Audios { get; set; }
    }
}
