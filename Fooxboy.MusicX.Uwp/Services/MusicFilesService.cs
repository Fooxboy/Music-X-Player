﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Uwp.Models;
using Newtonsoft.Json;
using Windows.Storage;

namespace Fooxboy.MusicX.Uwp.Services
{
    public static class MusicFilesService
    {
        public static async Task GetMusicLocal(bool refresh = false)
        {
            var musicLocal = await GetLocalMusicCollection();
            if (musicLocal.DateLastUpdate == "none") refresh = true;

            if (refresh)
            {
                musicLocal.Music.Clear();
                StaticContent.Music.Clear();
                var files = (await KnownFolders.MusicLibrary.GetFilesAsync(Windows.Storage.Search.CommonFileQuery.OrderByName)).ToList();
                foreach (var f in files)
                {
                    if (f.FileType == ".mp3" || f.FileType == ".wav")
                    {
                        AudioFile track;
                        try
                        {
                            track = await FindMetadataService.ConvertToAudioFile(f);

                        }catch { track = StaticContent.NowPlay; }

                        musicLocal.Music.Add(track);
                        StaticContent.Music.Add(track);
                    }
                }

                musicLocal.DateLastUpdate = $"{DateTime.Now.Day}.{DateTime.Now.Month} в {DateTime.Now.Hour}: {DateTime.Now.Minute}";
                var fileMusic = await StaticContent.LocalFolder.GetFileAsync("MusicCollection.json");
                var json = JsonConvert.SerializeObject(musicLocal);
                await FileIO.WriteTextAsync(fileMusic, json);
            }
            else foreach (var track in musicLocal.Music) StaticContent.Music.Add(track);
        }

        public static async Task<MusicCollection> GetLocalMusicCollection()
        {
            var fileMusic = await StaticContent.LocalFolder.GetFileAsync("MusicCollection.json");
            var stringMusic = await FileIO.ReadTextAsync(fileMusic);
            return JsonConvert.DeserializeObject<MusicCollection>(stringMusic);
        }

        public static async Task<AudioFile> GetLastPlayAudio()
        {
            var lastFile = await StaticContent.LocalFolder.GetFileAsync("LastPlay.json");
            var json = await FileIO.ReadTextAsync(lastFile);
            return JsonConvert.DeserializeObject<AudioFile>(json);
        }

        public static async Task SetLastPlayAudio(AudioFile file)
        {
            var json = JsonConvert.SerializeObject(file);
            var lastFile = await StaticContent.LocalFolder.GetFileAsync("LastPlay.json");
            await FileIO.WriteTextAsync(lastFile, json);
        }
    }
}