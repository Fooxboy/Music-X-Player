using DryIoc;
using Fooxboy.MusicX.Uwp.Converters;
using Fooxboy.MusicX.Uwp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fooxboy.MusicX.Uwp.Services
{
    public class AlbumLoaderService
    {
        private IContainer _container;

        public AlbumLoaderService(Core.Api api)
        {
            _api = api;

        }

        private Core.Api _api;

        private async Task<List<Album>> Get(long id, uint offset=0, uint count=5)
        {
            var albums = await _api.VKontakte.Music.Albums.GetAsync(id, count, offset);
            var l = new List<Album>();
            foreach (var a in albums) l.Add(a.ToAlbum());
            return l;
        }

        public async Task<List<Album>> GetLibraryAlbums(uint offset =0, uint count = 5)
        {
            var userId = (await _api.VKontakte.Users.Info.CurrentUserAsync()).Id;
            var albums = await Get(userId, offset, count);
            return albums;
        }

        public async Task<List<Album>> GetUserAlbums(long userId, uint offset = 0, uint count = 5)
        {
            var albums = await Get(userId, offset, count);
            return albums;
        }

        public async Task<List<Album>> GetRecomsAlbums(string blockId)
        {
            var blockFull = await _api.VKontakte.Music.Blocks.GetAsync(blockId);
            return blockFull.Albums.ToAlbumsList();
        }
    }
}
