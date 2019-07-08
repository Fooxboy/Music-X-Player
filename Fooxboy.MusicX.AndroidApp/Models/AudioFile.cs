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
using Fooxboy.MusicX.Core.Interfaces;

namespace Fooxboy.MusicX.AndroidApp.Models
{
    public class AudioFile : IAudioFile
    {
        public long Id { get; set; }
        public long OwnerId { get; set; }
        public long InternalId { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public long PlaylistId { get; set; }
        public bool IsLocal { get; set; }
        public double DurationSeconds { get; set; }
        public string SourceString { get; set; }
        public string Cover { get; set; }
        public bool IsFavorite { get; set; }
        public bool IsInLibrary { get; set; }
        public bool IsDownload { get; set; }
        public string DurationMinutes { get; set; }
}