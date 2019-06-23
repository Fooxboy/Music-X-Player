using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fooxboy.MusicX.Core.Interfaces;
using Fooxboy.MusicX.Core.Models;
using VkNet.Model.Attachments;

namespace Fooxboy.MusicX.Core.VKontakte.Music.Converters
{
    public static class ToIPlaylistFileConverter
    {
        public static IPlaylistFile ToIPlaylistFile(this AudioPlaylist playlist, IList<IAudioFile> tracks)
        {

            string cover;
            string artist;
            try
            {
                artist = playlist.MainArtists.FirstOrDefault().Name;
            }
            catch
            {
                artist = "Неизвестный исполнитель";
            }
            try
            {
                cover = playlist.Cover.Photo300;
            }
            catch
            {
                cover = "no";
            }

            IPlaylistFile playlistFile = new PlaylistFileAnyPlatform()
            {
                Artist = artist,
                Cover = cover,
                Id = playlist.Id.Value,
                IsLocal = false,
                Tracks = tracks,
                Name = playlist.Title
            };

            return playlistFile;
        }
    }
}
