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
            bool isAlbum;
            string cover;
            string artist;
            string genre;
            string year;
            try
            {
                artist = playlist.MainArtists.FirstOrDefault().Name;
                isAlbum = true;
            }
            catch
            {
                artist = "Неизвестный исполнитель";
                isAlbum = false;
            }
            try
            {
                cover = playlist.Cover.Photo300;
            }
            catch
            {
                cover = "no";
            }

            if(isAlbum)
            {
                try
                {
                    genre = playlist.Genres.First().Name;
                }
                catch
                {
                    genre = "no";
                }

                try
                {
                    year = playlist.Year.Value.ToString();
                }
                catch
                {
                    year = "no";
                }
                
            }else
            {
                genre = "no";
                year = "no";
            }


            IPlaylistFile playlistFile = new PlaylistFileAnyPlatform()
            {
                Artist = artist,
                Cover = cover,
                Id = playlist.Id.Value,
                IsLocal = false,
                Tracks = tracks,
                Name = playlist.Title,
                IsAlbum = isAlbum,
                Genre = genre,
                Year = year
            };

            return playlistFile;
        }
    }
}
