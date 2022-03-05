using Fooxboy.MusicX.Core.Models;
using Fooxboy.MusicX.Core.Models.General;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VkNet;
using VkNet.AudioBypassService.Extensions;
using VkNet.Model;
using VkNet.Utils;
using VkNet.Utils.AntiCaptcha;

namespace Fooxboy.MusicX.Core.Services
{
    public class VkService
    {

        public readonly VkApi vkApi;
        private readonly Logger logger;

        public bool IsAuth = false;

        public VkService()
        {
            var services = new ServiceCollection();
            services.AddAudioBypass();
            vkApi = new VkApi(services);

            logger = LogManager.GetLogger("Common");
        }

        public async Task<string> AuthAsync(string login, string password, Func<string> twoFactorAuth, ICaptchaSolver captchaSolver)
        {
            vkApi.CaptchaSolver = captchaSolver;
            await vkApi.AuthorizeAsync(new ApiAuthParams()
            {
                Login = login,
                Password = password,
                TwoFactorAuthorization = twoFactorAuth
            });


            var user = await vkApi.Users.GetAsync(new List<long>());
            vkApi.UserId = user[0].Id;

            IsAuth = true;

            return vkApi.Token;
        }

        public async Task SetTokenAsync(string token, ICaptchaSolver captchaSolver)
        {
            vkApi.CaptchaSolver = captchaSolver;
            await vkApi.AuthorizeAsync(new ApiAuthParams()
            {
                AccessToken = token
            });

            var user = await vkApi.Users.GetAsync(new List<long>());
            vkApi.UserId = user[0].Id;

            IsAuth = true;
        }

        public async Task<ResponseData> GetAudioCatalogAsync()
        {
            var parameters = new VkParameters
            {
                {"v", "5.131"},
                {"lang", "ru"},
                {"extended", "1"},
                {"access_token", vkApi.Token}
            };

            var json = await vkApi.InvokeAsync("catalog.getAudio", parameters);
            var model = JsonConvert.DeserializeObject<ResponseVk>(json);

            return model.Response;
            
        }

        public async Task<ResponseData> GetSectionAsync(string sectionId, string startFrom = null)
        {
            var parameters = new VkParameters
            {
                {"v", "5.131"},
                {"lang", "ru"},
                {"extended", "1"},
                {"access_token", vkApi.Token},
                {"section_id", sectionId },
            };

            if (startFrom != null) parameters.Add("start_from", startFrom);


            var json = await vkApi.InvokeAsync("catalog.getSection", parameters);
            var model = JsonConvert.DeserializeObject<ResponseVk>(json);

            return model.Response;

        }

        public async Task<ResponseData> GetBlockItemsAsync(string blockId)
        {
            var parameters = new VkParameters
            {
                {"v", "5.131"},
                {"lang", "ru"},
                {"extended", "1"},
                {"access_token", vkApi.Token},
                {"block_id", blockId },
            };


            var json = await vkApi.InvokeAsync("catalog.getBlockItems", parameters);
            var model = JsonConvert.DeserializeObject<ResponseVk>(json);

            return model.Response;
        }

        public async Task<ResponseData> GetAudioSearchAsync(string query)
        {
            var parameters = new VkParameters
            {
                {"v", "5.131"},
                {"lang", "ru"},
                {"extended", "1"},
                {"access_token", vkApi.Token},
                {"query", query}

            };


            var json = await vkApi.InvokeAsync("catalog.getAudioSearch", parameters);
            var model = JsonConvert.DeserializeObject<ResponseVk>(json);

            return model.Response;
        }

        public async Task<ResponseData> GetAudioArtist(string artistId)
        {
            var parameters = new VkParameters
            {
                {"v", "5.131"},
                {"lang", "ru"},
                {"extended", "1"},
                {"access_token", vkApi.Token},
                {"artist_id", artistId}

            };


            var json = await vkApi.InvokeAsync("catalog.getAudioArtist", parameters);
            var model = JsonConvert.DeserializeObject<ResponseVk>(json);

            return model.Response;
        }

        public async Task<ResponseData> GetPlaylistAsync(int count, long albumId, string accessKey, long ownerId, int offset = 0, int needOwner = 1)
        {
            var parameters = new VkParameters
            {
                {"v", "5.135"},

                {"lang", "ru"},
                {"audio_count", count },
                {"need_playlist", 1 },
                {"owner_id", ownerId},
                {"access_key", accessKey},
                {"func_v", 9 },
                {"id", albumId},
                {"audio_offset", offset },
                {"access_token", vkApi.Token},
                {"count", "10"},
                {"need_owner", needOwner }
            };

            var json = await vkApi.InvokeAsync("execute.getPlaylist", parameters);

            var model = JsonConvert.DeserializeObject<ResponseVk>(json);

            return model.Response;
        }

        public async Task AudioAdd(long audioId, long ownerId)
        {
            var parameters = new VkParameters
            {
                {"v", "5.135"},
                {"lang", "ru"},
                {"access_token", vkApi.Token},
                {"audio_id", audioId},
                {"owner_id", ownerId}
            };


            var json = await vkApi.InvokeAsync("audio.add", parameters);
        }

        public async Task AudioDelete(long audioId, long ownerId)
        {
            var parameters = new VkParameters
            {
                {"v", "5.135"},
                {"lang", "ru"},
                {"access_token", vkApi.Token},
                {"audio_id", audioId},
                {"owner_id", ownerId}
            };


            var json = await vkApi.InvokeAsync("audio.delete", parameters);
        }

    }
}
