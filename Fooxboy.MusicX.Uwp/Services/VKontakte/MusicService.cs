using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Core.Interfaces;
using Fooxboy.MusicX.Uwp.Models;

namespace Fooxboy.MusicX.Uwp.Services.VKontakte
{
    public static class MusicService
    {
        public static List<AudioFile> ConvertToAudioFile(IList<IAudioFile> music)
        {
            var tracks = new List<AudioFile>();

            foreach(var track in music)
            {
                var audiofile = new AudioFile()
                {
                    Artist = track.Artist,
                    Cover = track.Cover,
                    Duration = TimeSpan.FromSeconds(track.DurationSeconds),
                    DurationMinutes = track.DurationMinutes,
                    DurationSeconds = track.DurationSeconds,
                    Id = track.Id,
                    InternalId = track.InternalId,
                    IsLocal = false,
                    OwnerId = track.OwnerId,
                    PlaylistId = track.PlaylistId,
                    Source = null,
                    SourceString = track.SourceString,
                    Title = track.Title
                };

                tracks.Add(audiofile);
            }

            return tracks;
        }

    }
}
