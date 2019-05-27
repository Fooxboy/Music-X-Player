using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fooxboy.MusicX.Uwp.Models
{
    public class AudioFile
    {
        public long Id { get; set; }
        public long OwnerId { get; set; }
        public long InternalId { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public long PlaylistId { get; set; }
        public double DurationSeconds { get; set; }
        public string Source { get; set; }
        public string Cover { get; set; }
        public string DurationMinutes { get; set; }
    }
}
