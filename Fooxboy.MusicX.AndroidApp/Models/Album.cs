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
    public class Album : Fooxboy.MusicX.Core.Models.Album
    {
        new public List<Track> Tracks { get; set; }
        public bool IsDownloaded { get; set; }
    }
}