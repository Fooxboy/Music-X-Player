using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Fooxboy.MusicX.Core.Interfaces;
using Java.IO;
using Java.Net;

namespace Fooxboy.MusicX.AndroidApp.Services
{
    public static class ImagesService
    {
        // static string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal)

        static string path = Xamarin.Essentials.FileSystem.AppDataDirectory;
        public static string CoverTrack(IAudioFile track)
        {
            if(track.PlaylistId != 0)
            {
                return CoverPlaylistById(track.PlaylistId, track.Cover);
            }else
            {
                string filename = Path.Combine(path, $"VKTrackId{track.Id}.jpg");

                if (!System.IO.File.Exists(filename))
                {
                    using (var client = new WebClient())
                    {
                        client.DownloadFile(new Uri(track.Cover), filename);
                    }

                    //var fileStream = System.IO.File.Create(filename);
                    //int count;
                    //var url = new URL(filename);
                    //var connection = url.OpenConnection();
                    //connection.Connect();
                    //var lengthFile = connection.ContentLength;
                    //var input = new BufferedInputStream(url.OpenStream(), lengthFile);
                    //OutputStream output = new FileOutputStream(filename);
                    //var data = new byte[1024];
                    //long total = 0;
                    //while ((count = input.Read(data)) != -1)
                    //{
                    //    total += count;
                    //    output.Write(data, 0, count);
                    //}

                    //output.Flush();
                    //output.Close();
                    //input.Close();
                }

                return filename;
            }
        }

        public static string CoverPlaylist(IPlaylistFile playlist)
        {
            return CoverPlaylistById(playlist.Id, playlist.Cover);
        }

        public static string CoverPlaylistById(long playlistId, string uriImage)
        {
            string filename = System.IO.Path.Combine(path, $"VKPlaylistId{playlistId}.jpg");

            if(!System.IO.File.Exists(filename))
            {
                using(var client = new WebClient())
                {
                    client.DownloadFile(new Uri(uriImage), filename);
                }

                //var fileStream = System.IO.File.Create(filename);
                //int count;
                //var url = new URL(filename);
                //var connection = url.OpenConnection();
                //connection.Connect();
                //var lengthFile = connection.ContentLength;
                //var input = new BufferedInputStream(url.OpenStream(), lengthFile);
                //OutputStream output = new FileOutputStream(filename);
                //var data = new byte[1024];
                //long total = 0;
                //while((count = input.Read(data)) != -1)
                //{
                //    total += count;
                //    output.Write(data, 0, count);
                //}

                //output.Flush();
                //output.Close();
                //input.Close();
            }

            return filename;
        }
    }
}