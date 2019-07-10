using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Core.Interfaces;
using Fooxboy.MusicX.Core.VKontakte.Music.Converters;

namespace Fooxboy.MusicX.Core.VKontakte.Music
{
    public static class Playlist
    {
        public async static Task<IPlaylistFile> Create(string title, string description = null, IList<string> tracks = null)
        {
            if (StaticContent.VkApi == null) throw new Exception("Пользователь не авторизован");

            var playlist = await StaticContent.VkApi.Audio.CreatePlaylistAsync(StaticContent.UserId, title, description, tracks);
            return playlist.ToIPlaylistFile(new List<IAudioFile>());
        }

        public async static Task<bool> Edit(int playlistId, string title, string description = null)
        {
            if (StaticContent.VkApi == null) throw new Exception("Пользователь не авторизован");

            var result = await StaticContent.VkApi.Audio.EditPlaylistAsync(StaticContent.UserId, playlistId, title, description);
            return result;
        }

        public async static Task<bool> Delete(int playlistId)
        {
            if (StaticContent.VkApi == null) throw new Exception("Пользователь не авторизован");

            var result = await StaticContent.VkApi.Audio.DeletePlaylistAsync(StaticContent.UserId, playlistId);
            return result;
        }

        public async static Task<IPlaylistFile> GetById(long playlistId)
        {
            if (StaticContent.VkApi == null) throw new Exception("Пользователь не авторизован");

            var playlist = await StaticContent.VkApi.Audio.GetPlaylistByIdAsync(StaticContent.UserId, playlistId);
            var music = await StaticContent.VkApi.Audio.GetAsync(new VkNet.Model.RequestParams.AudioGetParams()
            {
                PlaylistId = playlist.Id,
            });

            IList<IAudioFile> tracks = new List<IAudioFile>();
            foreach (var track in music) tracks.Add(track.ToIAudioFile());
            return playlist.ToIPlaylistFile(tracks);
        }



        public static IPlaylistFile CreateSync(string title, string description = null, IList<string> tracks = null)
        {
            if (StaticContent.VkApi == null) throw new Exception("Пользователь не авторизован");

            var playlist = StaticContent.VkApi.Audio.CreatePlaylist(StaticContent.UserId, title, description, tracks);
            return playlist.ToIPlaylistFile(new List<IAudioFile>());
        }

        public static bool EditSync(int playlistId, string title, string description = null)
        {
            if (StaticContent.VkApi == null) throw new Exception("Пользователь не авторизован");

            var result = StaticContent.VkApi.Audio.EditPlaylist(StaticContent.UserId, playlistId, title, description);
            return result;
        }

        public static bool DeleteSync(int playlistId)
        {
            if (StaticContent.VkApi == null) throw new Exception("Пользователь не авторизован");

            var result = StaticContent.VkApi.Audio.DeletePlaylist(StaticContent.UserId, playlistId);
            return result;
        }

        public static IPlaylistFile GetByIdSync(long playlistId)
        {
            if (StaticContent.VkApi == null) throw new Exception("Пользователь не авторизован");

            var playlist = StaticContent.VkApi.Audio.GetPlaylistById(StaticContent.UserId, playlistId);
            var music = StaticContent.VkApi.Audio.Get(new VkNet.Model.RequestParams.AudioGetParams()
            {
                PlaylistId = playlist.Id,
            });

            IList<IAudioFile> tracks = new List<IAudioFile>();
            foreach (var track in music) tracks.Add(track.ToIAudioFile());
            return playlist.ToIPlaylistFile(tracks);
        }
    }
}
