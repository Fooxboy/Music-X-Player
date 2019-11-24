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

    
    public class AudioInBlock
    {

        public AudioFile track { get; set; }
        public string blockID { get; set; }

        public AudioInBlock(AudioFile t, string b)
        {
            track = t;
            blockID = b;
        }

    }
}