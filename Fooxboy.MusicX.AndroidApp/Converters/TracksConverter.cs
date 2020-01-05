using Fooxboy.MusicX.AndroidApp.Models;
using Fooxboy.MusicX.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Fooxboy.MusicX.AndroidApp.Converters
{
    public static class TracksConverter
    {
        public static List<Track> ToTracksList(this List<ITrack> tracks) => tracks.Select(t => t.ToTrack()).ToList();

        public static Track ToTrack(this ITrack t)
        {

            var track = new Track();
            track.AccessKey = t.AccessKey;
            track.Album = t.Album;
            track.Artist = t.Artist;
            track.Artists = t.Artists;
            track.Duration = t.Duration;
            track.GenreId = t.GenreId;
            track.Id = t.Id;
            track.IsAvailable = t.IsAvailable;
            track.IsDownloaded = false; //TODO: написать проверку на загрузку.
            track.IsLicensed = t.IsLicensed;
            track.LocalUri = null; //TODO: написать ЮрИ0))0)
            track.OwnerId = t.OwnerId;
            track.Subtitle = t.Subtitle;
            track.Title = t.Title;
            track.Url = t.Url;
            track.UrlMp3 = t.UrlMp3;
            return track;
        }
    }
}