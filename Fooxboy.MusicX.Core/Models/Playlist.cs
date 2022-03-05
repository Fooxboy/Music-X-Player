using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fooxboy.MusicX.Core.Models
{
    public class Playlist
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("owner_id")]
        public int OwnerId { get; set; }

        [JsonProperty("type")]
        public int Type { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("followers")]
        public int Followers { get; set; }

        [JsonProperty("plays")]
        public int Plays { get; set; }

        [JsonProperty("create_time")]
        public int CreateTime { get; set; }

        [JsonProperty("update_time")]
        public int UpdateTime { get; set; }

        [JsonProperty("genres")]
        public List<Genre> Genres { get; set; }

        [JsonProperty("is_following")]
        public bool IsFollowing { get; set; }

        [JsonProperty("year")]
        public int Year { get; set; }

        [JsonProperty("original")]
        public Original Original { get; set; }

        [JsonProperty("followed")]
        public Followed Followed { get; set; }

        [JsonProperty("photo")]
        public Photo Photo { get; set; }

        [JsonProperty("permissions")]
        public Permissions Permissions { get; set; }

        [JsonProperty("subtitle_badge")]
        public bool SubtitleBadge { get; set; }

        [JsonProperty("play_button")]
        public bool PlayButton { get; set; }

        [JsonProperty("access_key")]
        public string AccessKey { get; set; }

        [JsonProperty("is_explicit")]
        public bool IsExplicit { get; set; }

        [JsonProperty("main_artists")]
        public List<MainArtist> MainArtists { get; set; }

        [JsonProperty("album_type")]
        public string AlbumType { get; set; }

        [JsonProperty("audios")]
        public List<Audio> Audios { get; set; }
    }
}
