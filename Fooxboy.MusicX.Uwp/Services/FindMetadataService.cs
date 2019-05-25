using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Uwp.Interfaces;
using Fooxboy.MusicX.Uwp.Models;
using Windows.Storage;

namespace Fooxboy.MusicX.Uwp.Services
{
    public static class FindMetadataService
    {
        public async static Task<IAudio> Convert(Windows.Storage.StorageFile fileA)
        {
            var cache = ApplicationData.Current.LocalCacheFolder;
            var a = await fileA.CopyAsync(cache);

            var file = TagLib.File.Create(a.Path);
            IAudio audio = new Audio()
            {
                Artist = file.Tag.AlbumArtists[0],
                Duration = file.Properties.Duration,
                Id = file.Name,
                InternalId = "0",
                OwnerId = "0",
                PlaylistId = 0,
                Source = new Uri(a.Path),
                Title = file.Tag.Title
            };
            return audio;
        }
    }
}
