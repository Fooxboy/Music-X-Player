using System;
using System.Collections.Generic;
using System.Text;
using Fooxboy.MusicX.Core.Interfaces;

namespace Fooxboy.MusicX.Core.Models
{
    public class BlockAnyPlatform: IBlock
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string TypeBlock { get; set; }
        public long CountElements { get; set; }
        public string BlockId { get; set; }
        public List<IAudioFile> Tracks { get; set; }
        public List<IPlaylistFile> Playlists { get; set; }
    }
}
