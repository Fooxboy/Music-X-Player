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

namespace Fooxboy.MusicX.AndroidApp.Converters
{
    public static class AlbumsConverter
    {

        public static List<Album> ToAlbumsList(this List<IAlbum> albums) => albums.Select(a => a.ToAlbum()).ToList();

        public static Album ToAlbum(this IAlbum a)
        {
            var album = new Album();
            album.AccessKey = a.AccessKey;
            album.Artists = a.Artists;
            album.Cover = a.Cover;
            album.Description = a.Description;
            album.Followers = a.Followers;
            album.Genres = a.Genres;
            album.Id = a.Id;
            album.IsAvailable = a.IsAvailable;
            album.IsDownloaded = false; //TODO: проверка загрузки
            album.IsFollowing = a.IsFollowing;
            //TODO: album.MainArtist =
            album.OwnerId = a.OwnerId;
            album.Plays = a.Plays;
            album.TimeCreate = a.TimeCreate;
            album.TimeUpdate = a.TimeUpdate;
            album.Title = a.Title;
            album.Tracks = a.Tracks.ToTracksList();
            album.Type = a.Type;
            album.Year = a.Year;
            return album;
        }
    }
}