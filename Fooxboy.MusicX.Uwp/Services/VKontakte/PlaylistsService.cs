using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Core.Interfaces;
using Fooxboy.MusicX.Uwp.Models;

namespace Fooxboy.MusicX.Uwp.Services.VKontakte
{
    public static class PlaylistsService
    {
        public static PlaylistFile ConvertToPlaylistFile(IPlaylistFile playlist)
        {
            var playlistFile = new PlaylistFile()
            {
                Artist = playlist.Artist,
                Cover = playlist.Cover,
                IsLocal = false,
                Tracks = playlist.Tracks,
                Id = playlist.Id,
                Name = playlist.Name,
                TracksFiles = MusicService.ConvertToAudioFile(playlist.Tracks)
            };

            return playlistFile;
        }
    }
}
