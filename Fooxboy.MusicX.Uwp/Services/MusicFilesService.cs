using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Uwp.Models;
using Fooxboy.MusicX.Uwp.Resources.ContentDialogs;
using Newtonsoft.Json;
using Windows.Storage;

namespace Fooxboy.MusicX.Uwp.Services
{
    public static class MusicFilesService
    {
        public static async Task GetMusicLocal(bool refresh = false)
        {

            try
            {
                var musicLocal = await GetLocalMusicCollection();
                if (musicLocal.DateLastUpdate == "none") refresh = true;

                if (refresh)
                {
                    musicLocal.Music.Clear();
                    StaticContent.Music.Clear();
                    if (StaticContent.Config.FindInMusicLibrary)
                    {
                        var files = await KnownFolders.MusicLibrary.GetFilesAsync();
                        foreach (var f in files)
                        {
                            if (f.FileType == ".mp3" || f.FileType == ".wav" || f.FileType == ".flac")
                            {
                                AudioFile track;
                                track = await FindMetadataService.ConvertToAudioFile(f);
                                musicLocal.Music.Add(track);
                                StaticContent.Music.Add(track);
                            }
                        }
                    }

                    if (StaticContent.Config.FindInDocumentsLibrary)
                    {
                        var folder = KnownFolders.DocumentsLibrary;
                        var files = (await folder.GetFilesAsync(Windows.Storage.Search.CommonFileQuery.OrderByDate)).ToList();
                        foreach (var f in files)
                        {
                            if (f.FileType == ".mp3" || f.FileType == ".wav" || f.FileType == ".flac")
                            {
                                AudioFile track;
                                track = await FindMetadataService.ConvertToAudioFile(f);
                                musicLocal.Music.Add(track);
                                StaticContent.Music.Add(track);
                            }
                        }
                    }



                    musicLocal.DateLastUpdate = $"{DateTime.Now.Day}.{DateTime.Now.Month} в {DateTime.Now.Hour}: {DateTime.Now.Minute}";
                    var fileMusic = await StaticContent.LocalFolder.GetFileAsync("MusicCollection.json");
                    var json = JsonConvert.SerializeObject(musicLocal);
                    await FileIO.WriteTextAsync(fileMusic, json);
                }
                else foreach (var track in musicLocal.Music)
                    {
                        if (track.Source == null)
                        {
                            track.Source = await StorageFile.GetFileFromPathAsync(track.SourceString);
                        }

                        track.Duration = TimeSpan.FromSeconds(track.DurationSeconds);
                        StaticContent.Music.Add(track);
                    }
            }catch(Exception e)
            {
                await new ExceptionDialog("Ошибка при поиске файлов", "Возможно, нет доступа к папке", e).ShowAsync();
            }
            
        }

        public static async Task<MusicCollection> GetLocalMusicCollection()
        {
            var fileMusic = await StaticContent.LocalFolder.GetFileAsync("MusicCollection.json");
            var stringMusic = await FileIO.ReadTextAsync(fileMusic);
            return JsonConvert.DeserializeObject<MusicCollection>(stringMusic);
        }

        public static async Task UpdateMusicCollection()
        {
            var musicCollection = new MusicCollection()
            {
                DateLastUpdate = $"{DateTime.Now.Day}.{DateTime.Now.Month} в {DateTime.Now.Hour}: {DateTime.Now.Minute}",
                Music = StaticContent.Music.ToList()
            };

            var fileMusic = await StaticContent.LocalFolder.GetFileAsync("MusicCollection.json");
            var json = JsonConvert.SerializeObject(musicCollection);
            await FileIO.WriteTextAsync(fileMusic, json);
        }

        public static async Task<LastPlay> GetLastPlayAudio()
        {
            var lastFile = await StaticContent.LocalFolder.GetFileAsync("LastPlay.json");
            var json = await FileIO.ReadTextAsync(lastFile);
            return JsonConvert.DeserializeObject<LastPlay>(json);
        }
    }
}
