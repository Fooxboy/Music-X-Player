using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarSymbols;

namespace Fooxboy.MusicX.Uwp.Models
{
    public class MusicCollection
    {
        public List<AudioFile> Music { get; set; }
        public string DateLastUpdate { get; set; }
    }
}
