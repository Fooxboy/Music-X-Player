using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Uwp.Interfaces;

namespace Fooxboy.MusicX.Uwp.Models
{
    public class Audio : IAudio
    {
        public string Cover { get; set; }
        public string Id { get; set; }
        public string OwnerId { get; set; }
        public string InternalId { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public long PlaylistId { get; set; }
        public TimeSpan Duration { get; set; }
        public Uri Source { get; set; }
    }
}
