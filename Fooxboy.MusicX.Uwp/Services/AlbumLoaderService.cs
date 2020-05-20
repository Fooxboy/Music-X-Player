using DryIoc;
using Fooxboy.MusicX.Uwp.Converters;
using Fooxboy.MusicX.Uwp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Core;

namespace Fooxboy.MusicX.Uwp.Services
{
    public class AlbumLoaderService
    {
        private IContainer _container;
        private ILoggerService _logger;

        public AlbumLoaderService(Core.Api api, LoggerService logger)
        {
            _api = api;
            _logger = logger;
        }

        private Core.Api _api;

        private async Task<List<Album>> Get(long id, uint offset=0, uint count=5)
        {
            _logger.Trace($"Получение альбомов: ID = {id} | Offset = {offset} | Count = {count}");
            var albums = await _api.VKontakte.Music.Albums.GetAsync(id, count, offset);
            _logger.Info($"Получено {albums.Count} альбомов.");
            var l = new List<Album>();
            foreach (var a in albums) l.Add( await a.ToAlbum());
            return l;
        }

        public async Task<List<Album>> GetLibraryAlbums(uint offset =0, uint count = 5)
        {
            _logger.Trace($"Получение библиотеки альбомов: Offset = {offset} | Count = {count}");

            var userId = (await _api.VKontakte.Users.Info.CurrentUserAsync()).Id;
            var albums = await Get(userId, offset, count);
            _logger.Info($"Получено {albums.Count} альбомов.");

            return albums;
        }

        public async Task<List<Album>> GetUserAlbums(long userId, uint offset = 0, uint count = 5)
        {
            var albums = await Get(userId, offset, count);
            return albums;
        }

        public async Task<List<Album>> GetRecomsAlbums(string blockId)
        {
            _logger.Trace($"Получение альбомов рекомендаций: BlockID = {blockId}");
            var blockFull = await _api.VKontakte.Music.Blocks.GetAsync(blockId);
            _logger.Info($"Получено {blockFull.Albums.Count} альбомов");
            return await blockFull.Albums.ToAlbumsList();
        }
    }
}
