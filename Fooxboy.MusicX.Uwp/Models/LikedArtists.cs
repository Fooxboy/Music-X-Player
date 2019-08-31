using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarSymbols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fooxboy.MusicX.Uwp.Models
{
    public class LikedArtist
    {
        public string Name { get; set; }
        public string Banner { get; set; }
        public long Id { get; set; }
    }

    public class LikedArtists
    {
        public List<LikedArtist> Artists { get; set; }
    }
}
