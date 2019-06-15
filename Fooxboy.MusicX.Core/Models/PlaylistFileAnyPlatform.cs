using System;
using System.Collections.Generic;
using System.Text;
using Fooxboy.MusicX.Core.Interfaces;

namespace Fooxboy.MusicX.Core.Models
{
    public class PlaylistFileAnyPlatform : IPlaylistFile
    {
        public string Name { get; set; }
        public string Artist { get;set; }
        public bool IsLocal { get; set; }
        public long Id { get; set; }
        public string Cover { get; set; }
    }
}
