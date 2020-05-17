using System;
using System.Collections.Generic;
using System.Linq;
using Fooxboy.MusicX.Core.Interfaces;
using Fooxboy.MusicX.Core.Models.Music;
using Fooxboy.MusicX.Core.Models.Music.ArtistInfo;
using Fooxboy.MusicX.Core.Models.Music.BlockInfo;
using Block = Fooxboy.MusicX.Core.Models.Block;

namespace Fooxboy.MusicX.Core.VKontakte.Music.Converters
{
    public static class ToIBlockConverter
    {
        public static IBlock ConvertToIBlock(this Response<Models.Music.BlockInfo.ResponseItem> b)
        {
            //Api.Logger.Trace("[CORE] Конвертация ToIBlock...");

            IBlock block = new Block();
            try
            {
                var bl = b.response.Block;
                block.Count = bl.Count;
                block.Subtitle = bl.Subtitle;
                block.Id = bl.Id;
                block.Title = bl.Title;
                block.Source = bl.Source;
                block.Type = bl.Type;
                block.Tracks = bl.Audios?.Select(track => track.ToITrack()).ToList();
                block.Albums = bl.Playlists?.Select(playlist => playlist.ToIAlbum()).ToList();

                if (bl.Playlist != null)
                {
                    block.Albums = new List<IAlbum>() { bl.Playlist.ToIAlbum() };
                }

                if (bl.Items != null)
                {
                    block.Artists = new List<SearchArtistBlockInfo>();
                    foreach (var item in bl.Items)
                    {
                        if (item.Images != null)
                        {
                            if (item.Images.Count > 0)
                            {
                                PhotoArtistModel bigImage = new PhotoArtistModel() {Height = 0};
                                foreach (var image in item.Images)
                                {
                                    if (image.Height > bigImage.Height) bigImage = image;
                                }

                                item.Image = bigImage.Url;
                            }
                        }

                        block.Artists.Add(item);
                    }
                }
               
            }
            catch(Exception e)
            {
                Api.Logger.Error("[CORE] Ошибка при конвертации ToIBlock...", e);

                block.Title = "Информация недоступна";
                block.Tracks = new List<ITrack>();
                block.Albums = new List<IAlbum>();
            }
            
            return block;
        }

        public static IBlock ConvertToIBlock(this Models.Music.BlockInfo.Block bl)
        {
            IBlock  block = new Block();
            try
            {
                block.Count = bl.Count;
                block.Subtitle = bl.Subtitle;
                block.Id = bl.Id;
                block.Title = bl.Title;
                block.Source = bl.Source;
                block.Type = bl.Type;
                block.Tracks = bl.Audios?.Select(track => track.ToITrack()).ToList();
                block.Albums = bl.Playlists?.Select(playlist => playlist.ToIAlbum()).ToList();

                if (bl.Playlist != null)
                {
                    block.Albums = new List<IAlbum>() {bl.Playlist.ToIAlbum()};
                }

                if (bl.Items != null)
                {
                    block.Artists = new List<SearchArtistBlockInfo>();
                    foreach (var item in bl.Items)
                    {
                        if (item.Images != null)
                        {
                            if (item.Images.Count > 0)
                            {
                                PhotoArtistModel bigImage = new PhotoArtistModel() { Height = 0 };
                                foreach (var image in item.Images)
                                {
                                    if (image.Height > bigImage.Height) bigImage = image;
                                }

                                item.Image = bigImage.Url;
                            }
                        }

                        block.Artists.Add(item);
                    }
                }

            }
            catch
            {
                block.Title = "Информация недоступна";
                block.Tracks = new List<ITrack>();
                block.Albums = new List<IAlbum>();
            }
            
            return block;
        }
    }
}