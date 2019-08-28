using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Core.Interfaces;
using Fooxboy.MusicX.Core.Models;
using Fooxboy.MusicX.Core.Models.Music.ArtistInfo;
using Fooxboy.MusicX.Core.VKontakte.Music.Converters;
using Newtonsoft.Json;
using VkNet.Utils;

namespace Fooxboy.MusicX.Core.VKontakte.Music
{
    public static class Recommendations
    {
        public static IList<IAudioFile> TracksSync(int count, int offset)
        {
            if (StaticContent.VkApi == null) throw new Exception("Пользователь не авторизован");
            var music = StaticContent.VkApi.Audio.GetRecommendations(userId: StaticContent.UserId,  count: Convert.ToUInt32(count), offset:Convert.ToUInt32(offset));
            IList<IAudioFile> tracks = new List<IAudioFile>();
            foreach (var track in music) tracks.Add(track.ToIAudioFile());
            return tracks;
        }

        public async static Task<IList<IAudioFile>> Tracks(int count, int offset)
        {
            if (StaticContent.VkApi == null) throw new Exception("Пользователь не авторизован");
            var music = await StaticContent.VkApi.Audio.GetRecommendationsAsync(userId: StaticContent.UserId, count: Convert.ToUInt32(count), offset: Convert.ToUInt32(offset));
            IList<IAudioFile> tracks = new List<IAudioFile>();
            foreach (var track in music) tracks.Add(track.ToIAudioFile());
            return tracks;
        }

        public async static Task<IRecommendations> New()
        {
            if (StaticContent.VkApi == null) throw new Exception("Пользователь не авторизован");
            var parameters = new VkParameters();
            parameters.Add("v", "5.103");
            parameters.Add("lang", "ru");
            parameters.Add("extended", "1");
            parameters.Add("access_token", StaticContent.VkApi.Token);
            parameters.Add("count", "7");
            parameters.Add("fields", "first_name_gen, photo_100");

            var json = await StaticContent.VkApi.InvokeAsync("audio.getCatalog", parameters);
            var model = JsonConvert.DeserializeObject<Response<Models.Music.Recommendations.ResonseItem>>(json);

            IRecommendations recommendations = new RecommendationsAnyPlatform();
            recommendations.Blocks = new List<IBlock>();
            foreach (var blockVk in model.response.Items)
            {
                IBlock block = new BlockAnyPlatform();
                block.BlockId = blockVk.Id;
                block.CountElements = blockVk.Count;
                block.Description = blockVk.SubTitle;
                block.Title = blockVk.Title;
                block.TypeBlock = blockVk.Type;
                
                if (blockVk.Playlists != null)
                {
                    var plists = new List<IPlaylistFile>();
                    int i = 0;
                    foreach (var pl in blockVk.Playlists)
                    {
                        if (i > 10) break;
                        plists.Add(pl.ToIPlaylistFile(new List<IAudioFile>(), "Различные исполнители"));
                        i++;
                    }
                        
                    block.Playlists = plists;
                }

                if (blockVk.Audios != null)
                {
                    var audios = new List<AudioVkModel>();
                    for (int i = 0; i < 10; i++)
                    {
                        try
                        {
                            audios.Add(blockVk.Audios[i]);
                        }catch
                        {
                            break;
                        }
                       
                    }
                    var tracks = audios.ToIAudioFileList();
                    
                    block.Tracks = tracks;
                }

                recommendations.Blocks.Add(block);
            }

            return recommendations;
        }
    }
}
