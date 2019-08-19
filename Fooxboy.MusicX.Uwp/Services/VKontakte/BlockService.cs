using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Core.Interfaces;
using Fooxboy.MusicX.Uwp.Models;

namespace Fooxboy.MusicX.Uwp.Services.VKontakte
{
    public  static class BlockService
    {
        public static Block ConvertToBlock(IBlock block)
        {
            var b = new Block()
            {
                BlockId = block.BlockId,
                CountElements = block.CountElements,
                PlaylistsFiles = null,
                Description = block.Description,
                Playlists = block.Playlists,
                Title = block.Title,
                Tracks = block.Tracks,
                TrackFiles = null,
                TypeBlock = block.TypeBlock
            };

            return b;
        }
    }
}
