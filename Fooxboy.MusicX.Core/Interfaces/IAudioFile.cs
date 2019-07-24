using System;
using System.Collections.Generic;
using System.Text;

namespace Fooxboy.MusicX.Core.Interfaces
{
    public interface IAudioFile
    {
        long Id { get; set; }
        long OwnerId { get; set; }
        long InternalId { get; set; }
        string Title { get; set; }
        string Artist { get; set; }
        long ArtistId { get; set; }
        bool IsLicensed { get; set; }
        long PlaylistId { get; set; }
        bool IsLocal { get; set; }
        double DurationSeconds { get; set; }
        string SourceString { get; set; }
        string Cover { get; set; }
        bool IsFavorite { get; set; }
        bool IsInLibrary { get; set; }
        bool IsDownload { get; set; }
        string DurationMinutes { get; set; }
    }
}
