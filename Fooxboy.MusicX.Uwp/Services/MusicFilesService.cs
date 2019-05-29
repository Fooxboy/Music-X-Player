using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Uwp.Models;
using Windows.Storage;

namespace Fooxboy.MusicX.Uwp.Services
{
    public static class MusicFilesService
    {
        public static async Task GetMusicLocal()
        {
            var files = (await KnownFolders.MusicLibrary.GetFilesAsync(Windows.Storage.Search.CommonFileQuery.OrderByName)).ToList();
            files.AddRange((await KnownFolders.DocumentsLibrary.GetFilesAsync(Windows.Storage.Search.CommonFileQuery.OrderByName)).ToList());
            foreach (var f in files)
            {
                if (f.FileType == ".mp3" || f.FileType == ".wav")
                {
                    var track = await FindMetadataService.ConvertToAudioFile(f);
                    StaticContent.Music.Add(track);
                }
            }
        }
    }
}
