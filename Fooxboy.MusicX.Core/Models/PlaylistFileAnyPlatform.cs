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
        public IList<IAudioFile> Tracks { get; set; }
        public bool IsAlbum { get; set; }
        public string Genre { get; set; }
        public string Year { get; set; }
        public string Description { get; set; }
    }
}
