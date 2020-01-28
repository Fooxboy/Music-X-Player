using Fooxboy.MusicX.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fooxboy.MusicX.Uwp.Models
{
    public class Album : Fooxboy.MusicX.Core.Models.Album
    {
       new public List<Track> Tracks { get; set; }
    }
}
