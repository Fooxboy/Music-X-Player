using System;
using System.Collections.Generic;
using System.Text;
using Fooxboy.MusicX.Core.Interfaces;

namespace Fooxboy.MusicX.Core.Models
{
    public class Block: IBlock
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public long Count { get; set; }
        public string Type { get; set; }
        public string Source { get; set; }
        public string Id { get; set; }
        public List<ITrack> Tracks { get; set; }
        public List<IAlbum> Albums { get; set; }
    }
}
