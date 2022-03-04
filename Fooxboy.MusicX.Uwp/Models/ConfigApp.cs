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
        public bool StreamMusic { get; set; }
        public bool SaveImageToCache { get; set; }
        public bool SaveTracksToCache { get; set; }
        public bool IsRateMe { get; set; }
        public string AccessTokenVkontakte { get; set; }
        public long UserId { get; set; }

        public double Volume { get; set; }

        //public NowPlayTrack NowPlayTrack { get; set; }

    }
}
