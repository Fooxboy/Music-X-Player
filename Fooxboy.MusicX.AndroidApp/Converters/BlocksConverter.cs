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
    public static class BlocksConverter
    {
        public static List<Block> ToBlocksList(this List<IBlock> blocks) => blocks.Select(b => b.ToBlock()).ToList();
        public static Block ToBlock(this IBlock b)
        {
            Block block = new Block();
            block.Albums = b.Albums is null ? null : b.Albums;
            block.Count = b.Count;
            block.Id = b.Id;
            block.Source = b.Source;
            block.Subtitle = b.Subtitle;
            block.Title = b.Title;
            block.Tracks = b.Tracks is null ? null : b.Tracks;
            block.Type = b.Type;
            return block;
        }
    }
}