using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Fooxboy.MusicX.AndroidApp.Models;
using Fooxboy.MusicX.Core.Interfaces;

namespace Fooxboy.MusicX.AndroidApp.Converters
{
    static class PlaylistConverter
    {

        public static PlaylistFile FromCoreToAndroid(IPlaylistFile playlist)
        {
            PlaylistFile output = new PlaylistFile();
            output.AccessKey = playlist.AccessKey;
            output.Artist = playlist.Artist;
            output.Cover = playlist.Cover;
            output.Description = playlist.Description;
            output.Genre = playlist.Genre;
            output.Id = playlist.Id;
            output.IsAlbum = playlist.IsAlbum;
            output.IsLocal = playlist.IsLocal;
            output.Name = playlist.Name;
            output.OnRequest = playlist.OnRequest;
            output.OwnerId = playlist.OwnerId;
            output.Plays = playlist.Plays;
            output.Tracks = playlist.Tracks;
            output.TracksFiles = null;
            output.Year = playlist.Year;
            return output;
        }

    }
}