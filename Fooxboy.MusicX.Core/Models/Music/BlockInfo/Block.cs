﻿using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using VkNet.Model.Attachments;

namespace Fooxboy.MusicX.Core.Models.Music.BlockInfo
{
    public class Block
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("subtitle")]
        public string Subtitle { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("audios")]
        public List<Audio> Audios { get; set; }
        [JsonProperty("next_from")]
        public string NextFrom { get; set; }

        [JsonProperty("playlists")]
        public List<AudioPlaylist> Playlists { get; set; }
    }
}
