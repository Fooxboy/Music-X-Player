using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Uwp.Interfaces;
using Fooxboy.MusicX.Uwp.Models;
using TagLib;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using static System.Net.Mime.MediaTypeNames;

namespace Fooxboy.MusicX.Uwp.Services
{
    public static class FindMetadataService
    {

        public async static Task<AudioFile> ConvertToAudioFile(StorageFile file)
        {
            var mp3FileAbs = new FileMp3Abstraction()
            {
                Name = file.Name,
                ReadStream = await file.OpenStreamForReadAsync(),
                WriteStream = await file.OpenStreamForWriteAsync(),
            };

            using (var mp3File = TagLib.File.Create(mp3FileAbs))
            {
                AudioFile audio = new AudioFile();
                if (mp3File.Tag.AlbumArtists.Count() != 0) audio.Artist = mp3File.Tag.AlbumArtists[0];
                else
                {
                    if (mp3File.Tag.Artists.Count() != 0) audio.Artist = mp3File.Tag.Artists[0];
                    else audio.Artist = "Неизвестный исполнитель";
                }
                if (mp3File.Tag.Title != null) audio.Title = mp3File.Tag.Title;
                else audio.Title = file.DisplayName;
                audio.DurationSeconds = mp3File.Properties.Duration.TotalSeconds;
                audio.DurationMinutes = Converters.AudioTimeConverter.Convert(mp3File.Properties.Duration.TotalSeconds);
                audio.Id = (file.Name.GetHashCode() *1);
                audio.InternalId = 0;
                audio.OwnerId = 0;
                audio.PlaylistId = 0;
                if (mp3File.Tag.Pictures.Any())
                {
                    StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                    System.IO.File.WriteAllBytes($"{localFolder.Path}/{audio.Id}.jpg", mp3File.Tag.Pictures[0].Data.Data);
                    audio.Cover = $"{localFolder.Path}/{audio.Id}.jpg";
                }
                else
                {
                    audio.Cover = "ms-appx:///Assets/Images/placeholder.png";
                }
                audio.Source = new Uri(file.Path).ToString();

                return audio;
            }
        }

        public async static Task<AudioFile> ConvertToAudioFileOld(StorageFile storageFile)
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
            audio.Id = a.Name.GetHashCode();
            audio.InternalId = 0;
            audio.OwnerId = 0;
            audio.PlaylistId = 0;
            if (file.Tag.Pictures.Any()) {
                StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                System.IO.File.WriteAllBytes($"{file.Name}.jpg", file.Tag.Pictures[0].Data.Data);
                audio.Cover = $"{file.Name}.jpg";
            }
            else
            {
                audio.Cover = "ms-appx:///Assets/Images/placeholder.png";
            }
            
            audio.Source = new Uri(a.Path).ToString();


            return audio;
        }
    }
}
