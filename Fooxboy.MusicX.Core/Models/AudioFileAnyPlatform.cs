using System;
using System.Collections.Generic;
using System.Text;
using Fooxboy.MusicX.Core.Interfaces;

namespace Fooxboy.MusicX.Core.Models
{
    public class AudioFileAnyPlatform : IAudioFile
    {
        public long Id { get; set; }
        public long OwnerId { get; set; }
        public long InternalId { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public long ArtistId { get; set; }
        public bool IsLicensed { get; set; }
        public long PlaylistId { get; set; }
        public bool IsLocal { get; set; }
        public double DurationSeconds { get; set; }
        public string SourceString { get; set; }
        public string Cover { get; set; }
        public string DurationMinutes { get; set; }
        public bool IsFavorite { get; set; }
        public bool IsInLibrary { get; set; }
        public bool IsDownload { get; set; }
    }
}
