using System.Collections.Generic;

namespace Fooxboy.MusicX.Core.Interfaces
{
    public interface IArtist
    { 
        long Id { get; set; }
        string Name { get; set; }
        string Domain { get; set; }
        string Banner { get; set; }
        IPlaylistFile LastRelease { get; set; }
        List<IAudioFile> PopularTracks { get; set; }
        List<IPlaylistFile> Albums { get; set; }
        string BlockPoularTracksId { get; set; }
        string BlockAlbumsId { get; set; }
    }
}