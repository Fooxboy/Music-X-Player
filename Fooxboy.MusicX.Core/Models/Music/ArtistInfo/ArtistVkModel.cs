using System.Collections.Generic;
using Newtonsoft.Json;

namespace Fooxboy.MusicX.Core.Models.Music.ArtistInfo
{
    public class ArtistVkModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("is_followed")]
        public bool IsFollowed { get; set; }

        [JsonProperty("can_follow")]
        public bool CanFollow { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("domain")]
        public string Domain { get; set; }

        [JsonProperty("photo")]
        public List<PhotoArtistModel> Photo { get; set; }

        [JsonProperty("is_album_cover")]
        public bool IsAlbumCover { get; set; }
    }
}