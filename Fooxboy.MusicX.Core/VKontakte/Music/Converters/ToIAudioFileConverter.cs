using System;
using System.Collections.Generic;
using System.Text;
using Fooxboy.MusicX.Core.Interfaces;
using Fooxboy.MusicX.Core.Models;
using VkNet.Model.Attachments;

namespace Fooxboy.MusicX.Core.VKontakte.Music.Converters
{
    public static class ToIAudioFileConverter
    {

        public static IAudioFile ToIAudioFile(this Audio audio)
        {
            string cover;
            long idPlaylist = 0;
            if (audio.Album == null)
            {
                cover = "no";
                idPlaylist = 0;
            }
            else
            {
                cover = audio.Album.Cover.Photo270;
                idPlaylist = audio.Album.Id;
            }
            var duration = TimeSpan.FromSeconds(audio.Duration);
            IAudioFile audioFile = new AudioFileAnyPlatform()
            {
                Artist = audio.Artist,
                Cover = cover,
                DurationSeconds = audio.Duration,
                Id = audio.Id.Value,
                IsLocal = false,
                InternalId = audio.Id.Value,
                DurationMinutes = $"{duration.Minutes}:{duration.Seconds}",
                OwnerId = audio.OwnerId.Value,
                PlaylistId = idPlaylist,
                SourceString = audio.Url.DecodeAudioUrl().ToString(),
                Title = audio.Title
            };


            return audioFile;
        }
    }
}
