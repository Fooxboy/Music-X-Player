using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Core.Interfaces;
using Windows.Storage;

namespace Fooxboy.MusicX.Uwp.Services.VKontakte
{
    public static class ImagesService
    {

        public async static Task<string> CoverAudio(IAudioFile audio)
        {
            var uriImage = audio.Cover;
            StorageFile coverFile;
            try
            {
                coverFile =  await StaticContent.CoversFolder.GetFileAsync($"VK{audio.Id}Audio.jpg");
            }
            catch
            {
                using (var client = new WebClient())
                {
                    client.DownloadFile(uriImage, $"{StaticContent.CoversFolder.Path}/VK{audio.Id}Audio.jpg");
                }

                coverFile = await StaticContent.CoversFolder.GetFileAsync($"VK{audio.Id}Audio.jpg");
            }

            return coverFile.Path;
        }

        public async static Task<string> CoverPlaylist(IPlaylistFile playlist)
        {
            var uriImage = playlist.Cover;
            StorageFile coverFile;
            try
            {
                coverFile = await StaticContent.CoversFolder.GetFileAsync($"VK{playlist.Id}Playlist.jpg");
            }
            catch
            {
                using (var client = new WebClient())
                {
                    client.DownloadFile(uriImage, $"{StaticContent.CoversFolder.Path}/VK{playlist.Id}Playlist.jpg");
                }

                coverFile = await StaticContent.CoversFolder.GetFileAsync($"VK{playlist.Id}Playlist.jpg");
            }

            return coverFile.Path;
        }

    }
}
