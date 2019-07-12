using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Android.Provider;
using Fooxboy.MusicX.AndroidApp.Models;
using Fooxboy.MusicX.Core.Interfaces;

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

        public  static List<AudioFile> GetMusicLibrary(int count, int offset)
        {
            var tracksvk = Fooxboy.MusicX.Core.VKontakte.Music.Library.TracksSync(count, offset);
            return tracksvk.ConvertToAudioFile();
        }

        public static List<AudioFile> ConvertToAudioFile(this IList<IAudioFile> music, string cover = null)
        {
            var tracks = new List<AudioFile>();

            foreach (var track in music)
            {
                string coverImage;

                if (cover == null)
                {
                    if (track.Cover == "no")
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


                var audiofile = new AudioFile()
                {
                    Artist = track.Artist,
                    Cover = coverImage,
                    DurationMinutes = track.DurationMinutes,
                    DurationSeconds = track.DurationSeconds,
                    Id = track.Id,
                    InternalId = track.InternalId,
                    IsLocal = false,
                    OwnerId = track.OwnerId,
                    PlaylistId = track.PlaylistId,
                    SourceString = track.SourceString,
                    Title = track.Title,
                    IsFavorite = false,
                    IsDownload = false,
                    IsInLibrary = track.IsInLibrary
                };

                tracks.Add(audiofile);
            }

            return tracks;

        }
    }
}