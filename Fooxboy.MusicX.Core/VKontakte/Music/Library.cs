using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Fooxboy.MusicX.Core.Interfaces;
using Fooxboy.MusicX.Core.VKontakte.Music.Converters;
using System.Threading.Tasks;

namespace Fooxboy.MusicX.Core.VKontakte.Music
{
    public static class Library
    {
        public async static Task<IList<IAudioFile>> Tracks(int count=100, int offset=0)
        {
            if (StaticContent.VkApi == null) throw new Exception("Пользователь не авторизован");

            var music = await StaticContent.VkApi.Audio.GetAsync(new VkNet.Model.RequestParams.AudioGetParams()
            {
                Count = count,
                Offset = offset
            });

            IList<IAudioFile> tracks = new List<IAudioFile>();
            foreach (var track in music) tracks.Add(track.ToIAudioFile(true));
            return tracks;
        }

        public async static Task<long> CountTracks()
        {
            if (StaticContent.VkApi == null) throw new Exception("Пользователь не авторизован");

            var count = await StaticContent.VkApi.Audio.GetCountAsync(StaticContent.UserId);

            return count;
        }

        public async static Task<IList<IPlaylistFile>> Playlists(int count = 100, int offset = 0)
        {
            if (StaticContent.VkApi == null) throw new Exception("Пользователь не авторизован");

            var playlistsVk = await StaticContent.VkApi.Audio.GetPlaylistsAsync(StaticContent.UserId, 
                Convert.ToUInt32(count), Convert.ToUInt32(offset));

            IList<IPlaylistFile> playlists = new List<IPlaylistFile>();

            foreach (var playlist in playlistsVk)
            {
                //var music = await StaticContent.VkApi.Audio.GetAsync(new VkNet.Model.RequestParams.AudioGetParams()
                //{
                //    PlaylistId = playlist.Id,
                //});

                //IList<IAudioFile> tracks = new List<IAudioFile>();
                //foreach (var track in music) tracks.Add(track.ToIAudioFile());

                playlists.Add(playlist.ToIPlaylistFile(new List<IAudioFile>())) ;
            }
            return playlists;
        }


        public async static Task StreamToStatus()
        {
            if (StaticContent.VkApi == null) throw new Exception("Пользователь не авторизован");

            //StaticContent.VkApi.Audio.SetBroadcastAsync();
        }



        public static IList<IAudioFile> TracksSync(int count = 100, int offset = 0)
        {
            if (StaticContent.VkApi == null) throw new Exception("Пользователь не авторизован");

            var music =  StaticContent.VkApi.Audio.Get(new VkNet.Model.RequestParams.AudioGetParams()
            {
                Count = count,
                Offset = offset
            });

            IList<IAudioFile> tracks = new List<IAudioFile>();
            foreach (var track in music) tracks.Add(track.ToIAudioFile(true));
            return tracks;
        }

        public static long CountTracksSync()
        {
            if (StaticContent.VkApi == null) throw new Exception("Пользователь не авторизован");

            var count =  StaticContent.VkApi.Audio.GetCount(StaticContent.UserId);

            return count;
        }

        public static IList<IPlaylistFile> PlaylistsSync(int count = 100, int offset = 0)
        {
            if (StaticContent.VkApi == null) throw new Exception("Пользователь не авторизован");

            var playlistsVk = StaticContent.VkApi.Audio.GetPlaylists(StaticContent.UserId,
                Convert.ToUInt32(count), Convert.ToUInt32(offset));

            IList<IPlaylistFile> playlists = new List<IPlaylistFile>();

            foreach (var playlist in playlistsVk)
            {
                var music = StaticContent.VkApi.Audio.Get(new VkNet.Model.RequestParams.AudioGetParams()
                {
                    PlaylistId = playlist.Id,
                });

                IList<IAudioFile> tracks = new List<IAudioFile>();
                foreach (var track in music) tracks.Add(track.ToIAudioFile());

                playlists.Add(playlist.ToIPlaylistFile(tracks));
            }
            return playlists;
        }


        public static void StreamToStatusSync()
        {
            if (StaticContent.VkApi == null) throw new Exception("Пользователь не авторизован");

            //StaticContent.VkApi.Audio.SetBroadcastAsync();
        }
    }
}
