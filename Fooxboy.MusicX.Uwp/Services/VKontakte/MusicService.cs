using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Core.Interfaces;
using Fooxboy.MusicX.Uwp.Models;
using Fooxboy.MusicX.Uwp.Utils.Extensions;

namespace Fooxboy.MusicX.Uwp.Services.VKontakte
{
    public static class MusicService
    {
        public async static Task<List<AudioFile>> ConvertToAudioFile(IList<IAudioFile> music, string cover = null)
        {
            var tracks = new List<AudioFile>();

            foreach(var track in music)
            {
                string coverImage;

                if(cover == null)
                {
                    if (track.Cover == "no")
                    {
                        coverImage = "ms-appx:///Assets/Images/placeholder.png";
                    }
                    else
                    {
                        coverImage = await ImagesService.CoverAudio(track);
                    }
                }else
                {
                    coverImage = cover;
                }
               

                var audiofile = new AudioFile()
                {
                    Artist = track.Artist,
                    Cover = coverImage,
                    Duration = TimeSpan.FromSeconds(track.DurationSeconds),
                    DurationMinutes = track.DurationMinutes,
                    DurationSeconds = track.DurationSeconds,
                    Id = track.Id,
                    InternalId = track.InternalId,
                    IsLocal = false,
                    OwnerId = track.OwnerId,
                    PlaylistId = track.PlaylistId,
                    Source = null,
                    SourceString = track.SourceString,
                    Title = track.Title
                };

                tracks.Add(audiofile);
            }

            return tracks;
        }


        public async static Task PlayMusic(AudioFile audioFile, int typePlay, PlaylistFile playlistPlay= null)
        {
            //type play:
            //1 - проигрования из списка треков
            //2 - проигрование трека из плейлиста
            StaticContent.AudioService.Seek(TimeSpan.Zero);
            var playlistNowPlay = new PlaylistFile()
            {
                Artist = "Music X",
                Cover = "ms-appx:///Assets/Images/now.png",
                Id = 1000,
                Name = "Сейчас играет",
                TracksFiles = new List<AudioFile>(),
                IsLocal = false
            };


            if (typePlay == 1)
            {
                foreach (var trackMusic in StaticContent.MusicVKontakte) playlistNowPlay.TracksFiles.Add(trackMusic);
                StaticContent.AudioService.SetCurrentPlaylist(playlistNowPlay.ToAudioPlaylist(), false);
                StaticContent.AudioService.CurrentPlaylist.CurrentItem = audioFile;
                StaticContent.NowPlayPlaylist = playlistNowPlay;
            }
            else if(typePlay == 2)
            {
                StaticContent.NowPlayPlaylist = playlistPlay;

                var index = playlistPlay.TracksFiles.IndexOf(playlistPlay.TracksFiles.Single(t => t.Id == audioFile.Id));

                if (index != 0)
                {
                    StaticContent.AudioService.SetCurrentPlaylist(playlistPlay.ToAudioPlaylist(), false);
                    StaticContent.AudioService.CurrentPlaylist.CurrentItem = audioFile;
                }else
                {
                    StaticContent.AudioService.SetCurrentPlaylist(playlistPlay.ToAudioPlaylist(), true);
                }

            }

            if (!(StaticContent.PlaylistsVKontakte.Any(p => p.Id == 1000)))
            {
                StaticContent.PlaylistsVKontakte.Add(playlistNowPlay);
            }
        }

    }
}
