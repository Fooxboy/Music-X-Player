using System;
using System.Collections.Generic;
using System.Text;

namespace Fooxboy.MusicX.Core.Interfaces
{
    public interface ITrack
    {
        long Id { get; set; }
        IAlbum Album { get; set; }
        long? OwnerId { get; set; }
        string AccessKey { get; set; }
        List<IArtist> Artists { get; set; }
        int GenreId { get; set; }
        bool IsAvailable { get; set; }
        bool IsLicensed { get; set; }
        Uri Url { get; set; }
        Uri UrlMp3 { get; set; }
        TimeSpan Duration { get; set; }
        string Title { get; set; }
        string Artist { get; set; }
        string Subtitle { get; set; }
    }
}
