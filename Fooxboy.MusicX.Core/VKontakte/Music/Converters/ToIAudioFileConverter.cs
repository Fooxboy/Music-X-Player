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

        public static IAudioFile ToIAudioFile(this Audio audio, bool IsLibrary = false)
        {
            try
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
                string durationM = "";
                if (duration.Hours > 0)
                    durationM = duration.ToString("h\\:mm\\:ss");
                durationM = duration.ToString("m\\:ss");

                IAudioFile audioFile = new AudioFileAnyPlatform()
                {
                    Artist = audio.Artist,
                    Cover = cover,
                    DurationSeconds = audio.Duration,
                    Id = audio.Id.Value,
                    IsLocal = false,
                    InternalId = audio.Id.Value,
                    DurationMinutes = durationM,
                    OwnerId = audio.OwnerId.Value,
                    PlaylistId = idPlaylist,
                    SourceString = audio.Url.DecodeAudioUrl().ToString(),
                    Title = audio.Title,
                    IsDownload = false,
                    IsFavorite = false,
                    IsInLibrary = IsLibrary
                };

                return audioFile;

            }
            catch
            {
                IAudioFile audioFile = new AudioFileAnyPlatform()
                {
                    Artist = "Неизвестный исполнитель",
                    Cover = "no",
                    DurationMinutes = "00:00",
                    DurationSeconds = 0,
                    Id = 0,
                    InternalId = 0,
                    IsLocal = false,
                    OwnerId = 0,
                    PlaylistId = 0,
                    SourceString = "no",
                    Title = "Аудиозапись недоступна",
                };

                return audioFile;
            }
        }
    }
}
