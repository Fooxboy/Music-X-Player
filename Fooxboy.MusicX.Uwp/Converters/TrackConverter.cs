
using Fooxboy.MusicX.Core.Interfaces;
using Fooxboy.MusicX.Uwp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Fooxboy.MusicX.Uwp.Converters
{
    public static class TrackConverter
    {
        public static Track ToTrack(this ITrack track)
        {
            var tr = new Track()
            {
                AccessKey = track.AccessKey,
                Album = track.Album,
                Artist = track.Artist,
                Artists = track.Artists,
                Duration = track.Duration,
                GenreId = track.GenreId,
                Id = track.Id,
                IsAvailable = track.IsAvailable,
                IsLicensed = track.IsLicensed,
                OwnerId = track.OwnerId,
                Subtitle = track.Subtitle,
                Title = track.Title,
                Url = track.Url,
                UrlMp3 = track.UrlMp3,
                DurationString = track.Duration.TotalSeconds.ConvertToTime()
            };
            return tr;
        }

        public static List<Track> ToListTrack(this List<ITrack> tracks)
        {
            return tracks.Select(t => t.ToTrack()).ToList();
        }
    }
}
