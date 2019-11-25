using System;
using System.Collections.Generic;
using System.Text;

namespace Fooxboy.MusicX.Core.Interfaces
{
    public interface IBlock
    {
        string Title { get; set; }
        string Subtitle { get; set; }
        long Count { get; set; }
        string Type { get; set; }
        string Source { get; set; }
        string Id { get; set; }
        List<ITrack> Tracks { get; set; }
        List<IAlbum> Albums { get; set; }
    }
}
