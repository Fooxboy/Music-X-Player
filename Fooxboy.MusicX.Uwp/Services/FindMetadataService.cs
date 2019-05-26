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
        public async static Task<IAudio> Convert(StorageFile fileA)
        {
            var cache = ApplicationData.Current.LocalCacheFolder;
            var fileB = await cache.TryGetItemAsync(fileA.Name);
            StorageFile a;
            if (fileB != null)
            {
                var fileC = await cache.GetFileAsync(fileA.Name);
                await fileA.CopyAndReplaceAsync(fileC);
                a = fileC;
            }else
            {
                a = await fileA.CopyAsync(cache);
            }
                
            var file = TagLib.File.Create(a.Path);
            IAudio audio = new Audio();
            if (file.Tag.AlbumArtists.Count() != 0) audio.Artist = file.Tag.AlbumArtists[0];
            else
            {
                if (file.Tag.Artists.Count() != 0) audio.Artist = file.Tag.Artists[0];
                else audio.Artist = "Неизвестный исполнитель";
            }
            if (file.Tag.Title != null) audio.Title = file.Tag.Title;
            else audio.Title = fileA.DisplayName;
            audio.Duration = file.Properties.Duration;
            audio.Id = "0";
            audio.InternalId = "0";
            audio.OwnerId = "0";
            audio.PlaylistId = 0;
            audio.Source = new Uri(a.Path);
            

            return audio;
        }
    }
}
