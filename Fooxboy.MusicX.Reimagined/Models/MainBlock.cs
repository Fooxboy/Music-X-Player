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
using Fooxboy.MusicX.Core.Models;

namespace Fooxboy.MusicX.Reimagined.Models
{
    public class MainBlock
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<Block> Blocks { get; set; } = null;
    }
}