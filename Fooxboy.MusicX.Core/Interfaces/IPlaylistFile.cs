using System;
using System.Collections.Generic;
using System.Text;

namespace Fooxboy.MusicX.Core.Interfaces
{
    public interface IPlaylistFile
    {
        string Name { get; set; }
        string Artist { get; set; }
        bool IsLocal { get; set; }
        long Id { get; set; }
        string Cover { get; set; }
    }
}
