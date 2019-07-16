using System;
using Newtonsoft.Json;
using VkNet.Model.Attachments;

namespace Fooxboy.MusicX.Core.Models.Music.ArtistInfo
{
    public class AudioVkModel 
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("owner_id")]
        public long OwnerId { get; set; }

        [JsonProperty("artist")]
        public string Artist { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("duration")]
        public long Duration { get; set; }

        [JsonProperty("date")]
        public long Date { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("album")]
        public AlbumVkModel Album { get; set; }

        [JsonProperty("is_licensed")]
        public bool IsLicensed { get; set; }

        [JsonProperty("is_hq")]
        public bool IsHq { get; set; }

        [JsonProperty("access_key")]
        public string AccessKey { get; set; }

        [JsonProperty("track_code")]
        public string TrackCode { get; set; }

        [JsonProperty("is_focus_track")]
        public bool IsFocusTrack { get; set; }

        [JsonProperty("is_explicit")]
        public bool IsExplicit { get; set; }

        [JsonProperty("no_search", NullValueHandling = NullValueHandling.Ignore)]
        public long? NoSearch { get; set; }
    }
}