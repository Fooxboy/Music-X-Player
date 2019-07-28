using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkNet.Utils;

namespace Fooxboy.MusicX.Core.VKontakte.Music
{
    public static class Add
    {

        public async static Task<long> ToLibrary(long audioId, string accessKey = null)
        {
            if (StaticContent.VkApi == null) throw new Exception("Пользователь не авторизован");
            var param = new VkParameters();
            param.Add("audio_id", audioId);
            param.Add("owner_id", StaticContent.UserId);
            if(accessKey != null) param.Add("access_key", accessKey);
            var id = await StaticContent.VkApi.CallAsync<long>("audio.add", param);

            //var id = await StaticContent.VkApi.Audio.AddAsync(audioId, StaticContent.UserId);
            return id;
        }

        public async static Task<long> ToPlaylist(long audioId, long playlistId)
        {
            if (StaticContent.VkApi == null) throw new Exception("Пользователь не авторизован");
                var id = await StaticContent.VkApi.Audio.AddToPlaylistAsync(StaticContent.UserId,
                playlistId, new List<long>(){audioId});
            return id.FirstOrDefault();
        }

        public  static long ToLibrarySync(long audioId)
        {
            if (StaticContent.VkApi == null) throw new Exception("Пользователь не авторизован");

            var id =  StaticContent.VkApi.Audio.Add(audioId, StaticContent.UserId);
            return id;
        }

        public  static long ToPlaylistSync(long audioId, long playlistId)
        {
            if (StaticContent.VkApi == null) throw new Exception("Пользователь не авторизован");
            var id = StaticContent.VkApi.Audio.AddToPlaylist(StaticContent.UserId,
            playlistId, new List<long>() { audioId });
            return id.FirstOrDefault();
        }


    }
}
