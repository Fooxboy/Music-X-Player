using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Core.Interfaces;
using Newtonsoft.Json;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace Fooxboy.MusicX.Uwp.Models
{
    public class AudioFile : IAudioFile
    {
        public long Id { get; set; }
        public long OwnerId { get; set; }
        public long InternalId { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public long PlaylistId { get; set; }
        [JsonIgnore]
        public TimeSpan Duration { get; set; }
        public double DurationSeconds { get; set; }

        [JsonIgnore]
        public StorageFile Source { get; set; }
        public string SourceString { get; set; }
        public string Cover { get; set; }
        public bool IsLocal { get; set; }
        public string DurationMinutes { get; set; }
        public bool IsFavorite { get; set; }
        public bool IsInLibrary { get; set; }
        public bool IsDownload { get; set; }
    }
}
