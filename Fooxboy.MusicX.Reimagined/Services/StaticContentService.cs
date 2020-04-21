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

namespace Fooxboy.MusicX.Reimagined.Services
{
    public static class StaticContentService
    {
        public static long UserId { get; set; }
        public static string UserName { get; set; }
        public static string TwoFactorAuthCode { get; set; }
        public static string Token { get; set; }
        public static bool RepeatPlaylist { get; set; }
        public static bool RepeatTrack { get; set; }
        public static string ErrorCaption { get; set; }
        public static string ErrorDescription { get; set; }
        //public static List<Track> NowPlay { get; set; }
        //public static NavigationService NavigationService { get; set; }
    }
}