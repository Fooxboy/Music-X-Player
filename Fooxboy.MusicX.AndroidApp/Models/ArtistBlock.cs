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
    class ArtistBlock
    {

        public string Title { get; set; }
        public List<IPlaylistFile> Playlists { get; set; }
        public List<IAudioFile> Tracks { get; set; }

        public ArtistBlock(string t)
        {
            Title = t;
        }

    }
}