using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fooxboy.MusicX.Uwp.Models
{
    public class DownloadAudioFile
    {
        public string Title { get; set; }
        public AudioFile AudioFile { get; set; }
        public string Artist { get; set; }
        public string AlbumName { get; set; }
        public string AlbumYear { get; set; }
        public string Cover { get; set; }
        public string Url { get; set; }
        public bool FromAlbum { get; set; }
    }
}
