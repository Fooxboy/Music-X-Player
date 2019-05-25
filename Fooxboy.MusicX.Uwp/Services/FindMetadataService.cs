using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Uwp.Interfaces;
using Fooxboy.MusicX.Uwp.Models;

namespace Fooxboy.MusicX.Uwp.Services
{
    public static class FindMetadataService
    {
        public static IAudio Convert(string path)
        {
            var file = TagLib.File.Create(path);
            IAudio audio = new Audio()
            {
                Artist = file.Tag.AlbumArtists[0],
                Duration = file.Properties.Duration,
                Id = file.Name,
                InternalId = "0",
                OwnerId = "0",
                PlaylistId = 0,
                Source = new Uri(path),
                Title = file.Tag.Title
            };
            return audio;
        }
    }
}
