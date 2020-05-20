using Fooxboy.MusicX.Core.Interfaces;
using Fooxboy.MusicX.Uwp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DryIoc;
using System.Threading.Tasks;
using Fooxboy.MusicX.Uwp.Services;

namespace Fooxboy.MusicX.Uwp.Converters
{
    public static class AlbumConverter
    {
        public static async  Task<Album> ToAlbum(this IAlbum album)
        {
            var cacher = Container.Get.Resolve<ImageCacheService>();
            var a = new Album();
            a.AccessKey = album.AccessKey;
            a.Artists = album.Artists;
            a.Cover = await cacher.GetImage(album.Cover);
            a.Description = album.Description;
            a.Followers = album.Followers;
            a.Genres = album.Genres;
            a.Id = album.Id;
            a.IsAvailable = album.IsAvailable;
            a.IsFollowing = album.IsFollowing;
            a.OwnerId = album.OwnerId;
            a.Plays = album.Plays;
            a.TimeCreate = album.TimeCreate;
            a.TimeUpdate = album.TimeUpdate;
            a.Title = album.Title;
            a.Tracks = album.Tracks;
            a.Type = album.Type;
            a.Year = album.Year;

            return a;
        }

        public static async  Task<List<Album>> ToAlbumsList(this List<IAlbum> albums)
        {
            var l = new List<Album>();
            foreach (var album in albums)
            {
                l.Add(await album.ToAlbum());
            }

            return l;
        }

    }
}
