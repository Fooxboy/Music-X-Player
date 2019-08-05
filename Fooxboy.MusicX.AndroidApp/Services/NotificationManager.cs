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
using MediaManager;

namespace Fooxboy.MusicX.AndroidApp.Services
{
    public class NotificationManager:INotificationManager
    {
        public void UpdateNotification()
        {
            //throw new NotImplementedException();
        }

        public bool Enabled { get; set; }
        public bool ShowPlayPauseControls { get; set; }
        public bool ShowNavigationControls { get; set; }
    }
}