using System.Collections.Generic;
using Fooxboy.MusicX.Core.Interfaces;

namespace Fooxboy.MusicX.Core.Models
{
    public class Artist:IArtist
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Domain { get; set; }
        public string Banner { get; set; }
        public List<IBlock> Blocks { get; set; }
    }
}