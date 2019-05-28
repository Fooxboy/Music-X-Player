using System;
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
    public static class PlaylistsService
    {
        public async static Task<PlaylistFile> GetById(int id)
        {
            var pathPlaylists = await ApplicationData.Current.LocalFolder.GetFolderAsync("Playlists");
            StorageFile file;
            try
            {
                file = await pathPlaylists.GetFileAsync($"Id{id}.json");

            } catch
            {
                return null;
            }

            var json = await FileIO.ReadTextAsync(file);
            var model = JsonConvert.DeserializeObject<PlaylistFile>(json);
            return model;
        }

        public static async Task SetPlaylistLocal()
        {
            var pathPlaylists = await ApplicationData.Current.LocalFolder.GetFolderAsync("Playlists");
            var files = await pathPlaylists.GetFilesAsync();
            foreach (var file in files)
            {
                var json = await FileIO.ReadTextAsync(file);
                var playlist = JsonConvert.DeserializeObject<PlaylistFile>(json);
                StaticContent.Playlists.Add(playlist);
            }
        }
    }
}
