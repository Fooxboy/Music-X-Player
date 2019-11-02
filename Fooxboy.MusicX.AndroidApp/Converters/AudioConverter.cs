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
    static class AudioConverter
    {

        public static AudioFile FromCoreToAndroid(IAudioFile audio)
        {
            AudioFile output = new AudioFile();
            output.AccessKey = audio.AccessKey;
            output.Artist = audio.Artist;
            output.ArtistId = audio.ArtistId;
            output.Cover = audio.Cover;
            output.DurationMinutes = audio.DurationMinutes;
            output.DurationSeconds = audio.DurationSeconds;
            output.Id = audio.Id;
            output.InternalId = audio.InternalId;
            output.IsDownload = audio.IsDownload;
            output.IsFavorite = audio.IsFavorite;
            output.IsInLibrary = audio.IsInLibrary;
            output.IsLicensed = audio.IsLicensed;
            output.IsLocal = audio.IsLocal;
            output.OwnerId = audio.OwnerId;
            output.PlaylistId = audio.PlaylistId;
            output.SourceString = audio.SourceString;
            output.Title = audio.Title;
            return output;
        } 

    }
}