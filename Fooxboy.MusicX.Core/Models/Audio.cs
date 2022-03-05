using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fooxboy.MusicX.Core.Models
{
    public class Audio
    {
        [JsonProperty("artist")]
        public string Artist { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("owner_id")]
        public int OwnerId { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("duration")]
        public int Duration { get; set; }

        [JsonProperty("access_key")]
        public string AccessKey { get; set; }

        [JsonProperty("is_explicit")]
        public bool IsExplicit { get; set; }

        [JsonProperty("is_focus_track")]
        public bool IsFocusTrack { get; set; }

        [JsonProperty("is_licensed")]
        public bool IsLicensed { get; set; }

        [JsonProperty("track_code")]
        public string TrackCode { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("date")]
        public int Date { get; set; }

        [JsonProperty("album")]
        public Album Album { get; set; }

        [JsonProperty("main_artists")]
        public List<MainArtist> MainArtists { get; set; }

        [JsonProperty("short_videos_allowed")]
        public bool ShortVideosAllowed { get; set; }

        [JsonProperty("stories_allowed")]
        public bool StoriesAllowed { get; set; }

        [JsonProperty("stories_cover_allowed")]
        public bool StoriesCoverAllowed { get; set; }

        [JsonProperty("subtitle")]
        public string Subtitle { get; set; }

        [JsonProperty("lyrics_id")]
        public int? LyricsId { get; set; }

        [JsonProperty("genre_id")]
        public int? GenreId { get; set; }
    }
}
