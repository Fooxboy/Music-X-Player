using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Fooxboy.MusicX.Core.Interfaces;

namespace Fooxboy.MusicX.AndroidApp.Services
{
    public static class ImagesService
    {
        static string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

        public static string CoverTrack(IAudioFile track)
        {
            if(track.PlaylistId != 0)
            {
                return CoverPlaylistById(track.PlaylistId, track.Cover);
            }else
            {
                string filename = Path.Combine(path, $"VKTrackId{track.Id}.jpg");

                if (!File.Exists(filename))
                {
                    var fileStream = File.Create(filename);

                    using (var client = new WebClient())
                    {
                        client.DownloadFile(track.Cover, filename);
                    }
                }

                return filename;
            }
        }

        public static string CoverPlaylist(IPlaylistFile playlist)
        {
            return CoverPlaylistById(playlist.Id, playlist.Cover);п
        }

        public static string CoverPlaylistById(long playlistId, string uriImage)
        {
            string filename = System.IO.Path.Combine(path, $"VKPlaylistId{playlistId}.jpg");

            if(!File.Exists(filename))
            {
                var fileStream = File.Create(filename);

                using(var client = new WebClient())
                {
                    client.DownloadFile(uriImage, filename);
                }
            }

            return filename;
        }
    }
}