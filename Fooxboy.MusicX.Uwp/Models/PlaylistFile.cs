using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarSymbols;

namespace Fooxboy.MusicX.Uwp.Models
{
    public class PlaylistFile
    {
        public string Name { get; set; }
        public string Artist { get; set; }
        public long Id { get; set; }
        public List<AudioFile> Tracks { get; set; }
        public string Cover { get; set; }
    }
}
