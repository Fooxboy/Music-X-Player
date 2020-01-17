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
using Fooxboy.MusicX.AndroidApp.Converters;
using Fooxboy.MusicX.AndroidApp.Models;
using Fooxboy.MusicX.Core.Interfaces;

namespace Fooxboy.MusicX.AndroidApp.Services
{
    public static class PlaylistsService
    {

        public static List<Album> GetPlaylistLibrary()
        {
            var playlistsVk = Core.Api.GetApi().VKontakte.Music.Albums.Get(Services.StaticContentService.UserId, 15);
            var playlists = playlistsVk.ToAlbumsList();
            return playlists;
        }
        /*
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
        }*/
    }
}