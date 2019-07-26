using System.Collections.Generic;
using Fooxboy.MusicX.Core.Interfaces;

namespace Fooxboy.MusicX.Core.Models
{
    public class ArtistAnyPlatform:IArtist
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Domain { get; set; }
        public string Banner { get; set; }
        public IPlaylistFile LastRelease { get; set; }
        public List<IAudioFile> PopularTracks { get; set; }
        public List<IPlaylistFile> Albums { get; set; }
        public string BlockPoularTracksId { get; set; }
        public string BlockAlbumsId { get; set; }
    }
}