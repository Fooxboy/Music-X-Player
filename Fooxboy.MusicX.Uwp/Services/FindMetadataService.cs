using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Uwp.Interfaces;
using Fooxboy.MusicX.Uwp.Models;
using TagLib;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;
using static System.Net.Mime.MediaTypeNames;

namespace Fooxboy.MusicX.Uwp.Services
{
    public static class FindMetadataService
    {

        public async static Task<AudioFile> ConvertToAudioFile(StorageFile storageFile)
        {
            TagLib.File file;
            StorageFile a;
            try
            {
                 file = TagLib.File.Create(storageFile.Path);
                a = storageFile;
            }
            catch
            {
                var cache = ApplicationData.Current.LocalCacheFolder;
                var fileB = await cache.TryGetItemAsync(storageFile.Name);
                
                if (fileB != null)
                {
                    var fileC = await cache.GetFileAsync(storageFile.Name);
                    await storageFile.CopyAndReplaceAsync(fileC);
                    a = fileC;
                }
                else
                {
                    a = await storageFile.CopyAsync(cache);
                }

                file = TagLib.File.Create(a.Path);
            }

            AudioFile audio = new AudioFile();
            if (file.Tag.AlbumArtists.Count() != 0) audio.Artist = file.Tag.AlbumArtists[0];
            else
            {
                if (file.Tag.Artists.Count() != 0) audio.Artist = file.Tag.Artists[0];
                else audio.Artist = "Неизвестный исполнитель";
            }
            if (file.Tag.Title != null) audio.Title = file.Tag.Title;
            else audio.Title = storageFile.DisplayName;
            audio.DurationSeconds = file.Properties.Duration.TotalSeconds;
            audio.DurationMinutes = Converters.AudioTimeConverter.Convert(file.Properties.Duration.TotalSeconds);
            audio.Id = 0;
            audio.InternalId = 0;
            audio.OwnerId = 0;
            audio.PlaylistId = 0;
            if (file.Tag.Pictures.Any()) {

                System.IO.File.WriteAllBytes($"/Assets/temp/{file.Tag.Title}.jpg", file.Tag.Pictures[0].Data.Data);
                audio.Cover = $"/Assets/temp/{file.Tag.Title}{file.Properties.Duration.TotalSeconds}.jpg";
            }
            else
            {
                audio.Cover = "/Assets/Images/placeholder.png";
            }
            
            audio.Source = new Uri(a.Path).ToString();


            return audio;
        }

        public async static Task<IAudio> Convert(StorageFile storageFile)
        {
            TagLib.File file;
            StorageFile a;
            try
            {
                file = TagLib.File.Create(storageFile.Path);
                a = storageFile;
            }
            catch
            {
                var cache = ApplicationData.Current.LocalCacheFolder;
                var fileB = await cache.TryGetItemAsync(storageFile.Name);

                if (fileB != null)
                {
                    var fileC = await cache.GetFileAsync(storageFile.Name);
                    await storageFile.CopyAndReplaceAsync(fileC);
                    a = fileC;
                }
                else
                {
                    a = await storageFile.CopyAsync(cache);
                }

                file = TagLib.File.Create(a.Path);
            }

            IAudio audio = new Audio();
            if (file.Tag.AlbumArtists.Count() != 0) audio.Artist = file.Tag.AlbumArtists[0];
            else
            {
                if (file.Tag.Artists.Count() != 0) audio.Artist = file.Tag.Artists[0];
                else audio.Artist = "Неизвестный исполнитель";
            }
            if (file.Tag.Title != null) audio.Title = file.Tag.Title;
            else audio.Title = storageFile.DisplayName;
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
