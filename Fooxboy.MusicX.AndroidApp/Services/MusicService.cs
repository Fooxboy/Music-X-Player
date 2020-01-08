using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Android.Provider;
using Fooxboy.MusicX.AndroidApp.Converters;
using Fooxboy.MusicX.AndroidApp.Models;
using Fooxboy.MusicX.Core.Interfaces;

namespace Fooxboy.MusicX.AndroidApp.Services
{
    public static class MusicService
    {

        public static List<Track> GetLocal()
        {
            var tracks = new List<Track>();
            string dir = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryMusic).ToString();
            var files = Directory.GetFiles(dir);
            foreach (var track in files)
            {

            }

            return tracks;

        }

        public  static List<Track> GetMusicLibrary(int count, int offset)
        {
            //TODO: получение библиотеки
            //var tracksvk = Fooxboy.MusicX.Core.VKontakte.Music.Library.TracksSync(count, offset);
            /*var tracks = tracksvk.ConvertToAudioFile();
            return tracks;*/
            return null;
        }

        public static List<Track> ConvertToAudioFile(this IList<ITrack> music, string cover = null)
        {
            var tracks = new List<Track>();

            foreach (var track in music)
            {
                string coverImage;

                if (cover == null)
                {
                    if (track.Album.Cover is null)
                    {
                        coverImage = "placeholder";
                    }
                    else
                    {
                        coverImage =  ImagesService.CoverTrack(track);
                    }
                }
                else
                {
                    coverImage = cover;
                }


                var audiofile = track.ToTrack();
                audiofile.Album.Cover = coverImage;

                tracks.Add(audiofile);
            }

            return tracks;

        }
    }
}