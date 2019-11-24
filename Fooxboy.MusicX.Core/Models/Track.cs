using System;
using System.Collections.Generic;
using System.Text;
using Fooxboy.MusicX.Core.Interfaces;

namespace Fooxboy.MusicX.Core.Models
{
    public class Track : ITrack
    {
        public long Id { get; set; }
        public IAlbum Album { get; set; }
        public long? OwnerId { get; set; }
        public string AccessKey { get; set; }
        public List<IArtist> Artists { get; set; }
        public int GenreId { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsLicensed { get; set; }
        public Uri Url { get; set; }
        public Uri UrlMp3 { get; set; }
        public TimeSpan Duration { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Subtitle { get; set; }
    }
}
