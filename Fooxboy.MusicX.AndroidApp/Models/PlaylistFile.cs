using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Core.Interfaces;
using Fooxboy.MusicX.Core.Models;
using Newtonsoft.Json;

namespace Fooxboy.MusicX.AndroidApp.Models
{
    public class PlaylistFile : PlaylistFileAnyPlatform
    {
       public List<AudioFile> TracksFiles { get; set; }
    }
}
