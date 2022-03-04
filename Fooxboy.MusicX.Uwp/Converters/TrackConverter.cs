
using Fooxboy.MusicX.Core.Interfaces;
using Fooxboy.MusicX.Uwp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using DryIoc;
using Fooxboy.MusicX.Uwp.Services;

namespace Fooxboy.MusicX.Uwp.Converters
{
    public static class TrackConverter
    {
        public static async Task<Track> ToTrack(this ITrack track)
        {
            var cacher = Container.Get.Resolve<ImageCacheService>();
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
                IsLicensed = true,
                OwnerId = track.OwnerId,
                Subtitle = track.Subtitle,
                Title = track.Title,
                Url = track.Url,
                UrlMp3 = track.UrlMp3,
                DurationString = track.Duration.TotalSeconds.ConvertToTime()
            };

            if (tr.Album != null) tr.Album.Cover = await cacher.GetImage(tr.Album.Cover);
            return tr;
        }

        public static async Task<List<Track>> ToListTrack(this List<ITrack> tracks)
        {
            var l = new List<Track>();

            foreach(var tr in tracks)
            {
                l.Add(await tr.ToTrack());
            }

            return l;
        }
    }
}
