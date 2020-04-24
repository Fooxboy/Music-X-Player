using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DryIoc;
using Fooxboy.MusicX.Uwp.Services;

namespace Fooxboy.MusicX.Uwp.Models
{
    public class AllPlaylistsModel
    {
        public long Id { get; set; }
        public  string TitlePage { get; set; }
        public TypeView TypeViewPlaylist { get; set; }
        public AlbumLoaderService AlbumLoader { get; set; }

        public string BlockId { get; set; }
        public IContainer Container { get; set; }
        public enum TypeView { UserAlbum, ArtistAlbum, RecomsAlbums }
    }
}
