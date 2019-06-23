using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Core.Interfaces;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;

namespace Fooxboy.MusicX.Uwp.Services.VKontakte
{
    public static class ImagesService
    {

        public async static Task<string> CoverAudio(IAudioFile audio)
        {
            try
            {
                var uriImage = audio.Cover;
                StorageFile coverFile = null;
                var a = await StaticContent.CoversFolder.TryGetItemAsync($"VK{audio.Id}Audio.jpg");
                if (a != null)
                {
                    try
                    {
                        coverFile = await StaticContent.CoversFolder.GetFileAsync($"VK{audio.Id}Audio.jpg");
                    }
                    catch
                    {
                        coverFile = await StaticContent.CoversFolder.CreateFileAsync($"VK{audio.Id}Audio.jpg");
                        BackgroundDownloader downloader = new BackgroundDownloader();
                        DownloadOperation download = downloader.CreateDownload(new Uri(uriImage), coverFile);
                        await download.StartAsync();
                    }

                }
                else
                {
                    coverFile = await StaticContent.CoversFolder.CreateFileAsync($"VK{audio.Id}Audio.jpg");
                    using (var client = new WebClient())
                    {
                        await client.DownloadFileTaskAsync(uriImage, coverFile.Path);
                    }
                }


                return coverFile.Path;
            }catch(Exception e)
            {
                return null;
            }
            
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
