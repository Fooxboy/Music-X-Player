using System;
using System.Collections.Generic;
using System.Text;

namespace Fooxboy.MusicX.Core.Interfaces
{
    public interface IBlock
    {
        string Title { get; set; }
        string Description { get; set; }
        string TypeBlock { get; set; }
        long CountElements { get; set; }
        string BlockId { get; set; }
        List<ITrack> Tracks { get; set; }
        List<IAlbum> Playlists { get; set; }
    }
}
