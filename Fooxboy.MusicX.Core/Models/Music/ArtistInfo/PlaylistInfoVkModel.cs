using System.Collections.Generic;
using Newtonsoft.Json;

namespace Fooxboy.MusicX.Core.Models.Music.ArtistInfo
{
    public class PlaylistInfoVkModel
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("owner_id")]
        public long OwnerId { get; set; }

        [JsonProperty("type")]
        public long Type { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("genres")]
        public List<Genre> Genres { get; set; }

        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("is_following")]
        public bool IsFollowing { get; set; }

        [JsonProperty("followers")]
        public long Followers { get; set; }

        [JsonProperty("plays")]
        public long Plays { get; set; }

        [JsonProperty("create_time")]
        public long CreateTime { get; set; }

        [JsonProperty("update_time")]
        public long UpdateTime { get; set; }

        [JsonProperty("year")]
        public long Year { get; set; }

        [JsonProperty("photo")]
        public AlbumVkModel.Covers Photo { get; set; }

        [JsonProperty("access_key")]
        public string AccessKey { get; set; }

        [JsonProperty("is_explicit")]
        public bool IsExplicit { get; set; }
        

        [JsonProperty("album_type")]
        public string AlbumType { get; set; }
        

        [JsonProperty("subtitle", NullValueHandling = NullValueHandling.Ignore)]
        public string Subtitle { get; set; }
        
        public partial class Genre
        {
            [JsonProperty("id")]
            public long Id { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }
        }
    }
}