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

namespace Fooxboy.MusicX.AndroidApp.Models
{
    public class Track : Fooxboy.MusicX.Core.Models.Track
    {
        public bool IsDownloaded { get; set; }
        public string LocalUri { get; set; }
    }
}