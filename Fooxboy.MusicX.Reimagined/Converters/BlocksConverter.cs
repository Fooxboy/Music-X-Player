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
using Fooxboy.MusicX.Core.Interfaces;
using Fooxboy.MusicX.Core.Models;

namespace Fooxboy.MusicX.Reimagined.Converters
{
    public static class BlocksConverter
    {
        public static Block ToBlock(this IBlock block)
        {
            return new Block()
            {
                Albums = block.Albums,
                Count = block.Count,
                Id = block.Id,
                Source = block.Source,
                Subtitle = block.Subtitle,
                Title = block.Title,
                Tracks = block.Tracks,
                Type = block.Type
            };
        }

        public static List<Block> ToBlocksList(this List<IBlock> blocks) => blocks.Select(b => b.ToBlock()).ToList();
    }
}