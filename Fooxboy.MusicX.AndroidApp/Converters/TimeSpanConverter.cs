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

namespace Fooxboy.MusicX.AndroidApp.Converters
{
    public static class TimeSpanConverter
    {
        public static string ToDuration(this TimeSpan time)
        {
            var hours = time.Hours != 0 ? time.TotalHours.ToString() + ":": "";
            var minutes = time.Minutes !=0? time.Minutes <10? "0" + time.Minutes.ToString(): time.Minutes.ToString(): "00";
            var seconds = time.Seconds != 0 ? time.Seconds < 10 ? "0" + time.Seconds.ToString() : time.Seconds.ToString() : "00";
            return hours + minutes + ":" + seconds;
        }
    }
}