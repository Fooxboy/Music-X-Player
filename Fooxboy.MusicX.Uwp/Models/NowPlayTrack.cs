using Fooxboy.MusicX.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fooxboy.MusicX.Uwp.Models
{
    public class NowPlayTrack
    {
        public long AlbumId { get; set; }
        public int Index { get; set; }
        public double Second { get; set; }
        public string AccessKey { get; set; }

        public Track Track { get; set; }

        public Album Album { get; set; }

        public Artist Artist { get; set; }

    }
}
