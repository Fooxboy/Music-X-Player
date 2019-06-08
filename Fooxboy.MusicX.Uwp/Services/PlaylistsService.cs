using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Core;
using Fooxboy.MusicX.Uwp.Models;
using Fooxboy.MusicX.Uwp.Resources.ContentDialogs;
using Newtonsoft.Json;
using Windows.Storage;
using Windows.UI.Popups;

namespace Fooxboy.MusicX.Uwp.Services
{
    public static class PlaylistsService
    {
        public async static Task<PlaylistFile> GetById(long id)
        {
            var pathPlaylists = await ApplicationData.Current.LocalFolder.GetFolderAsync("Playlists");
            StorageFile file;
            try
            {
                file = await pathPlaylists.GetFileAsync($"Id{id}.json");

            } catch(Exception e)
            {
                await new ExceptionDialog("Невозможно получить плейлист", "Возможно, этого плейлиста больше не существует.", e).ShowAsync();
                return null;
            }

            var json = await FileIO.ReadTextAsync(file);
            var model = JsonConvert.DeserializeObject<PlaylistFile>(json);
            return model;
        }

        public static async Task SavePlaylist(PlaylistFile playlist)
        {
            var pathPlaylists = await ApplicationData.Current.LocalFolder.GetFolderAsync("Playlists");
            StorageFile filePlaylist;

            try
            {
                filePlaylist = await pathPlaylists.GetFileAsync($"Id{playlist.Id}.json");
            }
            catch
            {
                filePlaylist = await pathPlaylists.CreateFileAsync($"Id{playlist.Id}.json");
            }
            
            var jsonString = JsonConvert.SerializeObject(playlist);
            try
            {
                await FileIO.WriteTextAsync(filePlaylist, jsonString);
            }catch(Exception e)
            {
                await new ExceptionDialog("Невозможно сохранить плейлист", "Возможно, этот плейлист уже существует.", e).ShowAsync();

            }
        }

        public static async Task SetPlaylistLocal()
        {
            try
            {
                var pathPlaylists = await ApplicationData.Current.LocalFolder.GetFolderAsync("Playlists");
                var files = await pathPlaylists.GetFilesAsync();
                foreach (var file in files)
                {
                    var json = await FileIO.ReadTextAsync(file);
                    var playlist = JsonConvert.DeserializeObject<PlaylistFile>(json);
                    StaticContent.Playlists.Add(playlist);
                }
            }catch(Exception e)
            {
                await new ExceptionDialog("Невозможно получить список плейлистов", "Вам стоит переустановить приложение, если эта ошибка не изчезнет.", e).ShowAsync();
            }
            
        }

        public static async Task PlayPlaylist(PlaylistFile playlist)
        {

            if (playlist.Tracks.Count == 0)
            {
                var dialog = new MessageDialog("В данном плейлисте отсутсвуют треки. Пожалуйста, добавте в него треки.",
                    "Невозможно возпроизвести плейлист");
                await dialog.ShowAsync();
                return;
            }
            var folder = StaticContent.PlaylistsFolder;
            if (StaticContent.NowPlayPlaylist == playlist) return;
            await PlayMusicService.PlayMusicForLibrary(playlist.Tracks[0], 3, playlist);
        }

        public static async Task DeletePlaylist(PlaylistFile playlist)
        {
            try
            {
                if (playlist.Id == 1 || playlist.Id == 1000)
                {
                    var dialog = new MessageDialog("Данный плейлист невозможно удалить.",
                        "Невозможно удалить плейлист");
                    await dialog.ShowAsync();
                    return;
                }
                var folder = StaticContent.PlaylistsFolder;
                StaticContent.Playlists.Remove(playlist);
                var file = await folder.GetFileAsync($"Id{playlist.Id}.json");
                await file.DeleteAsync();
            }catch(Exception e)
            {
                await new ExceptionDialog("Невозможно удалить плейлист", "Возможно, этот плейлист уже был удален или он поврежден.", e).ShowAsync();

            }

        }
    }
}
