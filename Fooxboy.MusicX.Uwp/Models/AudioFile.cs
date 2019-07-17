using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Core.Interfaces;
using Fooxboy.MusicX.Core.Models;
using Newtonsoft.Json;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace Fooxboy.MusicX.Uwp.Models
{
    public class AudioFile : AudioFileAnyPlatform
    {
        [JsonIgnore]
        public TimeSpan Duration { get; set; }
        
        [JsonIgnore]
        public StorageFile Source { get; set; }
    }
}
