using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Core.Interfaces;
using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarSymbols;
using Newtonsoft.Json;

namespace Fooxboy.MusicX.Uwp.Models
{
    public class PlaylistFile : IPlaylistFile
    {
        public string Name { get; set; }
        public string Artist { get; set; }
        public long Id { get; set; }
        public bool IsLocal { get; set; }
        [JsonIgnore]
        public IList<IAudioFile> Tracks { get; set; }
        public List<AudioFile> TracksFiles { get; set; }
        public string Cover { get; set; }
        public bool IsAlbum { get; set; }
        public string Genre { get; set; }
        public string Year { get; set; }
    }
}
