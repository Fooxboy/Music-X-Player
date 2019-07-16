using System.Collections.Generic;
using Fooxboy.MusicX.Core.VKontakte.Music;
using Newtonsoft.Json;

namespace Fooxboy.MusicX.Core.Models.Music.ArtistInfo
{
    public class ItemInfo
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
        
        [JsonProperty("next_from", NullValueHandling = NullValueHandling.Ignore)]
        public string NextFrom { get; set; }
        
        [JsonProperty("artist", NullValueHandling = NullValueHandling.Ignore)]
        public ArtistVkModel Artist { get; set; }
        
        [JsonProperty("audios", NullValueHandling = NullValueHandling.Ignore)]
        public List<AudioVkModel> Audios { get; set; }
        
        [JsonProperty("playlist", NullValueHandling = NullValueHandling.Ignore)]
        public PlaylistInfoVkModel Playlist { get; set; }
        
        [JsonProperty("playlists", NullValueHandling = NullValueHandling.Ignore)]
        public List<PlaylistInfoVkModel> Playlists { get; set; }
    }
}