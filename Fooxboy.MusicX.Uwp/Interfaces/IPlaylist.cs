using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fooxboy.MusicX.Uwp.Interfaces
{
    public interface IPlaylist
    {
        string Id { get; set; }

        string Title { get; set; }

        int TracksCount { get; set; }

        string Artist { get; set; }

        string Description { get; set; }
    }
}
