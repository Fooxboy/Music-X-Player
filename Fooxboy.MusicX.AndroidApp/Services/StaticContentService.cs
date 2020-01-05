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

namespace Fooxboy.MusicX.AndroidApp.Services
{
    public static class StaticContentService
    {
        public static string CodeTwoFactorAuth { get; set; }
        public static bool RepeatPlaylist { get; set; }
        public static bool RepeatTrack { get; set; }
        public static List<Track> NowPlay { get; set; } 
    }
}