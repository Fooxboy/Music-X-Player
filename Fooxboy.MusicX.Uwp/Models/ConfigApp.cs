using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fooxboy.MusicX.Uwp.Models
{
    public class ConfigApp
    {
        public int ThemeApp { get; set; }
        public bool FindInMusicLibrary { get; set; }
        public bool FindInDocumentsLibrary { get; set; }
        public bool StreamMusic { get; set; }
        public bool SaveImage { get; set; } = true;

        public bool IsRateMe { get; set; }
        
    }
}
