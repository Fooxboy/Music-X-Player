using System.Collections.Generic;
using System.Linq;
using Fooxboy.MusicX.Core.Interfaces;
using Fooxboy.MusicX.Core.Models;
using Fooxboy.MusicX.Core.Models.Music;

namespace Fooxboy.MusicX.Core.VKontakte.Music.Converters
{
    public static class ToIBlockConverter
    {
        public static IBlock ConvertToIBlock(this Response<Models.Music.BlockInfo.ResponseItem> b)
        {
            IBlock  block = new Block();
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
            }
            catch
            {
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