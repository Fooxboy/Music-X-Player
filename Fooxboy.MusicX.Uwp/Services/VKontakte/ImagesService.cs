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

            if (audio.PlaylistId != 0)
            {
                var cover = await CoverPlaylistById(audio.PlaylistId, audio.Cover);
                return cover;
            }
            else
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
                    BackgroundDownloader downloader = new BackgroundDownloader();
                    DownloadOperation download = downloader.CreateDownload(new Uri(uriImage), coverFile);
                    await download.StartAsync();
                }

                return coverFile.Path;
            }   
        }


        public async static Task<string> AvatarUser(string url)
        {
            StorageFile avatarUser;
            try
            {
                avatarUser = await StaticContent.CoversFolder.GetFileAsync($"User{url.GetHashCode()}Photo.jpg");
            }
            catch
            {
                avatarUser = await StaticContent.CoversFolder.CreateFileAsync($"User{url.GetHashCode()}Photo.jpg");
                BackgroundDownloader downloader = new BackgroundDownloader();
                DownloadOperation download = downloader.CreateDownload(new Uri(url), avatarUser);
                await download.StartAsync();
            }

            return avatarUser.Path;
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
                coverFile = await StaticContent.CoversFolder.CreateFileAsync($"VK{playlist.Id}Playlist.jpg");
                BackgroundDownloader downloader = new BackgroundDownloader();
                DownloadOperation download = downloader.CreateDownload(new Uri(uriImage), coverFile);
                await download.StartAsync();
            }

            return coverFile.Path;
        }

        public async static Task<string> CoverPlaylistById(long playlistId, string uriImage)
        {
            StorageFile coverFile;
            try
            {
                coverFile = await StaticContent.CoversFolder.GetFileAsync($"VK{playlistId}Playlist.jpg");
            }
            catch
            {
                coverFile = await StaticContent.CoversFolder.CreateFileAsync($"VK{playlistId}Playlist.jpg");
                BackgroundDownloader downloader = new BackgroundDownloader();
                DownloadOperation download = downloader.CreateDownload(new Uri(uriImage), coverFile);
                await download.StartAsync();
            }

            return coverFile.Path;
        }
    }
}
