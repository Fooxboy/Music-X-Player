using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Fooxboy.MusicX.Core.Interfaces;
using Fooxboy.MusicX.Core.VKontakte.Music.Converters;
using System.Threading.Tasks;
using VkNet.Utils;

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


        public async static Task StreamToStatus(long audioId, long ownerId, string accessKey = null)
        {
            if (StaticContent.VkApi == null) throw new Exception("Пользователь не авторизован");
            var audioString = $"audio{ownerId}_{audioId}";
            audioString = accessKey == null ? audioString : $"{audioString}_{accessKey}";
            var param = new VkParameters();
            param.Add("audio_ids", audioId);
            param.Add("target_ids", StaticContent.UserId);
            param.Add("access_token", StaticContent.VkApi.Token);
            param.Add("v", "5.101");
            var json = await StaticContent.VkApi.InvokeAsync("audio.setBroadcast", param);
            //var b =json + "a";
            // var ab = await StaticContent.VkApi.CallAsync<List<long>>("audio.setBroadcast", param);
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


        public static void StreamToStatusSync(long audioId, long ownerId, string accessKey = null)
        {
            if (StaticContent.VkApi == null) throw new Exception("Пользователь не авторизован");
            var audioString = $"audio{ownerId}_{audioId}";
            audioString = accessKey == null ? audioString : $"{audioString}_{accessKey}";
            var param = new VkParameters();
            param.Add("audio_ids", audioId);
            param.Add("target_ids", StaticContent.UserId);
            param.Add("access_token", StaticContent.VkApi.Token);
            param.Add("v", "5.101");
            var json = StaticContent.VkApi.Invoke("audio.setBroadcast", param);
        }
    }
}
