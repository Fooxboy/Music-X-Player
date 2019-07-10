using System.Collections.Generic;
using System.IO;
using Android.Provider;
using Fooxboy.MusicX.AndroidApp.Models;

namespace Fooxboy.MusicX.AndroidApp.Services
{
    public static class MusicService
    {

        public static List<AudioFile> GetLocal()
        {
            var tracks = new List<AudioFile>();
            string dir = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryMusic).ToString();
            var files = Directory.GetFiles(dir);
            foreach (var track in files)
            {
                
            }

            return tracks;

        }
        
    }
}