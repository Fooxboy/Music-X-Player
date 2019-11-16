using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Fooxboy.MusicX.AndroidApp.Models;
using Fooxboy.MusicX.Core.Interfaces;

namespace Fooxboy.MusicX.AndroidApp.Services
{
    public static class PlaylistsService
    {

        public static List<PlaylistFile> GetPlaylistLibrary()
        {
            var playlistsVk = Fooxboy.MusicX.Core.VKontakte.Music.Library.PlaylistsSync(20);
            var playlists = playlistsVk.CovertToPlaylistFiles();
            return playlists;
        }

        public static List<PlaylistFile> CovertToPlaylistFiles(this IList<IPlaylistFile> playlists)
        {
            var list = new List<PlaylistFile>();
            foreach (var playlist in playlists) list.Add(playlist.ConvertToPlaylistFile());
            return list;
        }

        public static PlaylistFile ConvertToPlaylistFile(this IPlaylistFile playlist)
        {
            string cover;
            if (playlist.Cover == "no")
            {
                cover = "playlist_placeholder";
            }
            else
            {
                cover = ImagesService.CoverPlaylist(playlist);
            }

            List<AudioFile> tracksFiles;

            if (playlist.IsAlbum)
            {
                tracksFiles =  MusicService.ConvertToAudioFile(playlist.Tracks, cover);
            }
            else
            {
                tracksFiles = MusicService.ConvertToAudioFile(playlist.Tracks);
            }

            var playlistFile = new PlaylistFile()
            {
                AccessKey = playlist.AccessKey,
                OwnerId = playlist.OwnerId,
                Artist = playlist.Artist,
                Cover = cover,
                IsLocal = false,
                Tracks = playlist.Tracks,
                Id = playlist.Id,
                Name = playlist.Name,
                TracksFiles = tracksFiles,
                Genre = playlist.Genre,
                IsAlbum = playlist.IsAlbum,
                Year = playlist.Year,
                Description = playlist.Description
            };

            return playlistFile;
        }
    }
}