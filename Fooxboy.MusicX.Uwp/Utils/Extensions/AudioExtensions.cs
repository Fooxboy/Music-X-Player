using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Uwp.Interfaces;
using Fooxboy.MusicX.Uwp.Models;
using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarSymbols;

namespace Fooxboy.MusicX.Uwp.Utils.Extensions
{
    public static class AudioExtensions
    {
        public static IAudio ToIAudio(this AudioFile sourse)
        {
            IAudio audio = new Audio()
            {
                Artist = sourse.Artist,
                Duration = TimeSpan.FromSeconds(sourse.DurationSeconds),
                Id = sourse.Id.ToString(),
                InternalId = sourse.InternalId.ToString(),
                OwnerId = sourse.OwnerId.ToString(),
                PlaylistId = sourse.PlaylistId,
                Source = new Uri(sourse.Source),
                Title = sourse.Title,
                Cover = sourse.Cover
            };

            return audio;
        }


        public static AudioFile ToAudioFile(this IAudio source)
        {
            var audio = new AudioFile()
            {
                Artist = source.Artist,
                Cover = source.Cover,
                DurationMinutes = $"{source.Duration.Minutes}:{source.Duration.Seconds}",
                DurationSeconds = source.Duration.TotalMilliseconds,
                Id = Int32.Parse(source.Id),
                InternalId = Int32.Parse(source.InternalId),
                OwnerId = Int32.Parse(source.InternalId),
                PlaylistId = source.PlaylistId,
                Source = source.Source.ToString(),
                Title = source.Title
            };

            return audio;
        }
        public static List<IAudio> ToIAudioList(this List<AudioFile> source)
        {
            var list = new List<IAudio>();
            foreach(var audio in source) list.Add(audio.ToIAudio());
            return list;
        }
    }
}
