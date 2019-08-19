using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Core.Models;

namespace Fooxboy.MusicX.Uwp.Models
{
    public class Block: BlockAnyPlatform
    {
        public List<AudioFile> TrackFiles { get; set; }
        public List<PlaylistFile> PlaylistsFiles { get; set; }
    }
}
