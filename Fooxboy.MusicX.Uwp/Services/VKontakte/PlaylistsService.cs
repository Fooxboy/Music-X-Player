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
        public async static Task<PlaylistFile> ConvertToPlaylistFile(IPlaylistFile playlist)
        {
            string cover;
            if(playlist.Cover == "no")
            {
                cover = "ms-appx:///Assets/Images/playlist-placeholder.png";
            }else
            {
                cover = await ImagesService.CoverPlaylist(playlist);
            }

            List<AudioFile> tracksFiles;

            if(playlist.IsAlbum)
            {
                tracksFiles = await MusicService.ConvertToAudioFile(playlist.Tracks, cover);
            }else
            {
                tracksFiles = await MusicService.ConvertToAudioFile(playlist.Tracks, cover);
            }

            var playlistFile = new PlaylistFile()
            {
                Artist = playlist.Artist,
                Cover = cover,
                IsLocal = false,
                Tracks = playlist.Tracks,
                Id = playlist.Id,
                Name = playlist.Name,
                TracksFiles = tracksFiles,
                Genre = playlist.Genre,
                IsAlbum = playlist.IsAlbum,
                Year = playlist.Year
            };

            return playlistFile;
        }
    }
}
