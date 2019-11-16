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
    public class PlaylistInBlock
    {

        public PlaylistFile Playlist{get; set;}
        public string BlockID { get; set; }

        public PlaylistInBlock(PlaylistFile plist, string block)
        {
            Playlist = plist;
            BlockID = block;
        }


    }
}