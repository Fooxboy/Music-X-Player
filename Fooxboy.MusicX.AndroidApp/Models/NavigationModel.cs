﻿using System;
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
    public class NavigationModel
    {
        public string Title { get; set; }
        public Fragment Fragment { get; set; }
    }
}