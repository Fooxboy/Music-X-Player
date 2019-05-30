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
        public static IAudio ToIAudio(this AudioFile source)
        {
            IAudio audio = new Audio()
            {
                Artist = source.Artist,
                Duration = TimeSpan.FromSeconds(source.DurationSeconds),
                Id = source.Id.ToString(),
                InternalId = source.InternalId.ToString(),
                OwnerId = source.OwnerId.ToString(),
                PlaylistId = source.PlaylistId,
                Source = new Uri(source.Source),
                Title = source.Title,
                Cover = source.Cover
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
