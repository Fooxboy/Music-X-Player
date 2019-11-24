using System;
using System.Collections.Generic;
using System.Text;

namespace Fooxboy.MusicX.Core.Interfaces
{
    public interface IAlbum
    {
        long Id { get; set; }
        long OwnerId { get; set; }
        string Title { get; set; }
        List<IArtist> Artists { get; set; }
        string MainArtist { get; set; }
        string Cover { get; set; }
        string AccessKey { get; set; }
        List<ITrack> Tracks { get; set; }
        List<string> Genres { get; set; }
        long Plays { get; set; }
        string Year { get; set; }
        string Description { get; set; }
        public DateTime TimeUpdate { get; set; }
        public DateTime TimeCreate { get; set; }
        long Followers { get; set; }
        bool IsFollowing { get; set; }
        long Type { get; set; }
    }
}
