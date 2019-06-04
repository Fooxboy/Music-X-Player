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
                audio.Duration = mp3File.Properties.Duration;
                audio.Id = (file.Name.GetHashCode() *1);
                audio.InternalId = 0;
                audio.OwnerId = 0;
                audio.PlaylistId = 0;
                if (mp3File.Tag.Pictures.Any())
                {
                    StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                    if( (await localFolder.GetFolderAsync("Covers")).TryGetItemAsync($"{audio.Id}.jpg") == null)
                    {
                        System.IO.File.WriteAllBytes($"{localFolder.Path}/Covers/{audio.Id}.jpg", mp3File.Tag.Pictures[0].Data.Data);

                    }
                    audio.Cover = $"{localFolder.Path}/Covers/{audio.Id}.jpg";
                }
                else
                {
                    audio.Cover = "ms-appx:///Assets/Images/placeholder.png";
                }
                audio.Source = file;
                audio.SourceString = file.Path;
                return audio;
            }
        } 
    }
}
