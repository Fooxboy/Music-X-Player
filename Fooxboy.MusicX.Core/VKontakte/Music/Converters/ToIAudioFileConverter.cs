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
            IAudioFile audioFile = new AudioFileAnyPlatform()
            {
                Artist = audio.Artist,
                Cover = audio.Album.Cover.Photo135,
                DurationSeconds = audio.Duration,
                Id = audio.Id.Value,
                IsLocal = false,
                InternalId = audio.Id.Value,
                OwnerId = audio.OwnerId.Value,
                PlaylistId = audio.Album.Id,
                SourceString = audio.Url.DecodeAudioUrl().ToString(),
                Title = audio.Title
            };


            return audioFile;
        }
    }
}
