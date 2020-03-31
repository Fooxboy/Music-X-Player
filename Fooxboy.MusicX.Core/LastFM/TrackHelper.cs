using System;
using System.Collections.Generic;
using System.Text;
using Fooxboy.MusicX.Core.Interfaces;
using Lpfm.LastFmScrobbler;

namespace Fooxboy.MusicX.Core.LastFM
{
    public static class TrackHelper
    {
        public static Track ToTrack(this ITrack track)
        {
            var model = new Track();
            model.AlbumArtist = track.Album is null? track.Artist: track.Album.Artists[0].Name;
            model.AlbumName = track.Album is null? track.Title: track.Album.Title;
            model.ArtistName = track.Artist;
            model.Duration = track.Duration;
            model.TrackName = track.Title;
            model.WhenStartedPlaying = DateTime.Now - track.Duration;

            return model;
        }
    }
}
