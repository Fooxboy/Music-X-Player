using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Fooxboy.MusicX.Uwp.Converters
{
    class AudioTimeConverter { 


        public static string Convert(double value)
        {
            TimeSpan t = TimeSpan.FromSeconds(value);
            return $"{t.Minutes}:{t.Seconds}";
        }
    }
}
