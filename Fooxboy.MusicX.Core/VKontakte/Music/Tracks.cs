using Fooxboy.MusicX.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Core.Models.Music;
using Fooxboy.MusicX.Core.VKontakte.Music.Converters;
using Newtonsoft.Json;
using VkNet;
using VkNet.Utils;

namespace Fooxboy.MusicX.Core.VKontakte.Music
{
    public class Tracks
    {
        private readonly VkApi _api;
        public Tracks(VkApi api)
        {
            _api = api;
        }
        public async Task<List<ITrack>> GetAsync(int count = 10, int offset =0, string accessKey = null, long? playlistId =null, long? ownerId = null)
        {
            Api.Logger.Trace("[CORE] Запрос audio.get...");
            var music = await _api.Audio.GetAsync(new VkNet.Model.RequestParams.AudioGetParams()
            {
                Count = count,
                Offset = offset, 
                AccessKey =accessKey,
                PlaylistId = playlistId,
                OwnerId = ownerId
            });
            Api.Logger.Trace($"[CORE] Ответ получен: {music.Count} элементов.");
            return music.Select(track=> track.ToITrack()).ToList();
        }

        public async Task<List<ITrack>> GetTracksAlbum(int count, long albumId, string accessKey, long ownerId, int offset = 0, string refType = "recoms_mood_playlists", int needOwner=1)
        {
            var parameters = new VkParameters
            {
                //{"v", "5.115"},
                {"v", "5.131"},

                {"https", 1 },
                {"lang", "ru"},
                {"audio_count", count },
                {"ref", refType},
                {"need_playlist", 1 },
                {"owner_id", ownerId},
                {"access_key", accessKey},
                {"func_v", 5 },
                {"id", albumId},
                {"audio_offset", offset },
                {"access_token", _api.Token},
                {"count", "10"},
                {"need_owner", needOwner }
            };
            Api.Logger.Trace("[CORE] Запрос execute.getPlaylist...");

            var json = await _api.InvokeAsync("execute.getPlaylist", parameters);
            var model = JsonConvert.DeserializeObject<Response<GetPlaylistModel>>(json);
            Api.Logger.Trace($"[CORE] Ответ получен.");

            return model.response.Audios.Select(track => track.ToITrack()).ToList();
        }
        
        public List<ITrack> Get(int count = 10, int offset =0, string accessKey = null, long? playlistId =null, long? ownerId = null)
        {
            var music =  _api.Audio.Get(new VkNet.Model.RequestParams.AudioGetParams()
            {
                Count = count,
                Offset = offset, 
                AccessKey =accessKey,
                PlaylistId = playlistId,
                OwnerId = ownerId
            });
            return music.Select(track=> track.ToITrack()).ToList();
        }

        public async Task<long> GetCountAsync()
        {
            Api.Logger.Trace("[CORE] Запрос audio.coutTracks...");

            long userId = 0;
            if (_api.UserId.Value == 0)
            {
                var user = await _api.Users.GetAsync(new List<long>());
                userId = user[0].Id;
            }
            else userId = _api.UserId.Value;
            Api.Logger.Trace($"[CORE] Ответ получен.");

            return await _api.Audio.GetCountAsync(userId);
        }
        public long GetCount()
        {
            long userId = 0;
            if(_api.UserId.Value == 0)
            {
                var user =  _api.Users.Get(new List<long>());
                userId = user[0].Id;
            }else userId = _api.UserId.Value;
            return  _api.Audio.GetCount(_api.UserId.Value);
        }

        public async Task AddTrackAsync(long id, long ownerId, long? albumId = null)
        {
            Api.Logger.Trace("[CORE] Запрос audio.add...");

            var result =await _api.Audio.AddAsync(id, ownerId, null, albumId);
            Api.Logger.Trace($"[CORE] Ответ получен: {result}");

        }

        public async Task DeleteTrackAsync(long id, long ownerId)
        {
            Api.Logger.Trace("[CORE] Запрос audio.delete...");

            var result = await _api.Audio.DeleteAsync(id, ownerId);
            Api.Logger.Trace($"[CORE] Ответ получен: {result}");

        }

    }
}
