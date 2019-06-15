using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fooxboy.MusicX.Core.VKontakte.Music
{
    public static class Add
    {

        public async static Task<long> ToLibrary(long audioId)
        {
            if (StaticContent.VkApi == null) throw new Exception("Пользователь не авторизован");

            var id = await StaticContent.VkApi.Audio.AddAsync(audioId, StaticContent.UserId);
            return id;
        }

        public async static Task<long> ToPlaylist(long audioId, long playlistId)
        {
            if (StaticContent.VkApi == null) throw new Exception("Пользователь не авторизован");
                var id = await StaticContent.VkApi.Audio.AddToPlaylistAsync(StaticContent.UserId,
                playlistId, new List<long>(){audioId});
            return id.FirstOrDefault();
        }


    }
}
