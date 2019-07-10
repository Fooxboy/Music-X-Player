using System;
using System.Collections.Generic;
using System.Text;

namespace Fooxboy.MusicX.Core.Interfaces
{
    public interface IPlaylistFile
    {
        string Name { get; set; }
        string Artist { get; set; }
        IList<IAudioFile> Tracks { get; set; }
        bool IsLocal { get; set; }
        long Id { get; set; }
        string Cover { get; set; }
        bool IsAlbum { get; set; }
        string Genre { get; set; }
        string Year { get; set; }
        string Description { get; set; }
    }
}
