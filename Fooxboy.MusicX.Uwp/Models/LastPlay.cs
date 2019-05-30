using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarSymbols;

namespace Fooxboy.MusicX.Uwp.Models
{
    public class LastPlay
    {
        public AudioFile Track { get; set; }
        public double Volume { get; set; }
        public PlaylistFile Playlist { get; set; }
    }
}
