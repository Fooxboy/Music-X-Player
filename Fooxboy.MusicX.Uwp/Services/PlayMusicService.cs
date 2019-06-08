﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Uwp.Converters;
using Fooxboy.MusicX.Uwp.Models;
using Fooxboy.MusicX.Uwp.Utils.Extensions;
using Windows.Storage;

namespace Fooxboy.MusicX.Uwp.Services
{
    public static class PlayMusicService
    {
        public async static Task PlayMusicForLibrary(AudioFile audioFile, int typePlay, PlaylistFile playlistPlay = null)
        {

            if (audioFile == null) return;
            //type play:
            //1 - проигрования из списка треков
            //2 - проигрование трека по клику на него
            //3 - проигрование трека из плейлиста
            var lastPlayPlaylist = await PlaylistsService.GetById(1);
            if (audioFile.Source == null) audioFile.Source = await StorageFile.GetFileFromPathAsync(audioFile.SourceString);
            if (!(lastPlayPlaylist.Tracks.Any(t => t.SourceString == audioFile.SourceString)))
            {
                lastPlayPlaylist.Tracks.Add(audioFile);
                await PlaylistsService.SavePlaylist(lastPlayPlaylist);
            }


            var playlistNowPlay = new PlaylistFile()
            {
                Artist = "Music X",
                Cover = "ms-appx:///Assets/Images/now.png",
                Id = 1000,
                Name = "Сейчас играет",
                Tracks = new List<AudioFile>()
            };

            if (typePlay == 1)
            {
                foreach (var trackMusic in StaticContent.Music) playlistNowPlay.Tracks.Add(trackMusic);
                StaticContent.NowPlayPlaylist = playlistNowPlay;

                StaticContent.AudioService.CurrentPlaylist.CurrentItem = audioFile;
            }
            else if (typePlay == 2)
            {
                StaticContent.NowPlayPlaylist = playlistNowPlay;
                StaticContent.NowPlayPlaylist.Tracks.Add(audioFile);
            } else if (typePlay == 3)
            {
                StaticContent.NowPlayPlaylist = playlistPlay;
            }

            if (!(StaticContent.Playlists.Any(p => p.Id == 1000)))
            {
                StaticContent.Playlists.Add(StaticContent.NowPlayPlaylist);
            }
            var playlistNowPlayAsAudioPlaylist = StaticContent.NowPlayPlaylist.ToAudioPlaylist(audioFile);
            StaticContent.AudioService.SetCurrentPlaylist(playlistNowPlayAsAudioPlaylist);
        }
    }
}
