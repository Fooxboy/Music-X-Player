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
        string Cover { get; set; }
        string AccessKey { get; set; }
        List<ITrack> Tracks { get; set; }
        List<string> Genres { get; set; }
        long Plays { get; set; }
        long Year { get; set; }
        bool IsAvailable { get; set; }
        string Description { get; set; }
        DateTime TimeUpdate { get; set; }
        DateTime TimeCreate { get; set; }
        long Followers { get; set; }
        bool IsFollowing { get; set; }
        long Type { get; set; }
    }
}
