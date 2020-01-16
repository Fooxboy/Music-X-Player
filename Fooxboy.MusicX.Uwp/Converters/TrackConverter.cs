
using Fooxboy.MusicX.Core.Interfaces;
using Fooxboy.MusicX.Uwp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fooxboy.MusicX.Uwp.Converters
{
    public static class TrackConverter
    {
        public static Track ToTrack(this ITrack track)
        {
            //TODO: сделатт
            return track as Track;
        }

        public static List<Track> ToListTrack(this List<ITrack> tracks)
        {
            return tracks.Select(t => t.ToTrack()).ToList();
        }
    }
}
