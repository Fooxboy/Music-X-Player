using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Uwp.Models;
using TagLib;
using Windows.Storage;
using Windows.Storage.Search;

namespace Fooxboy.MusicX.Uwp.Services.VKontakte
{
    public static class DownloadLibraryService
    {

        public async static Task<List<AudioFile>> GetTracks()
        {
            var folderMusicX = await KnownFolders.MusicLibrary.GetFolderAsync("Music X");
            List<AudioFile> audios = new List<AudioFile>();
            var files = await folderMusicX.GetFilesAsync(CommonFileQuery.OrderByName);
            foreach (var f in files)
            {
                if (f.FileType == ".mp3" || f.FileType == ".wav" || f.FileType == ".flac")
                {
                    AudioFile track;
                    track = await FindMetadataService.ConvertToAudioFile(f);
                    if(track != null)
                    {
                        audios.Add(track);
                    }
                }
            }
            return audios;
        }
    }
}
