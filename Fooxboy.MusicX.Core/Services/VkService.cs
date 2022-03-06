using Fooxboy.MusicX.Core.Models;
using Fooxboy.MusicX.Core.Models.General;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkNet;
using VkNet.AudioBypassService.Extensions;
using VkNet.Enums.Filters;
using VkNet.Infrastructure;
using VkNet.Model;
using VkNet.Utils;
using VkNet.Utils.AntiCaptcha;

namespace Fooxboy.MusicX.Core.Services
{
    public class VkService
    {

        public readonly VkApi vkApi;
        private readonly Logger logger;
        private readonly string vkApiVersion = "5.135";

        public bool IsAuth = false;

        public VkService(Logger logger)
        {
            var services = new ServiceCollection();
            services.AddAudioBypass();
            
            vkApi = new VkApi(services);


            var ver = vkApiVersion.Split('.');

            vkApi.VkApiVersion.SetVersion(int.Parse(ver[0]), int.Parse(ver[1]));

            var log = LogManager.Setup().GetLogger("Common");

            if (logger == null) this.logger = log;
            else this.logger = logger;
        }

        public async Task<string> AuthAsync(string login, string password, Func<string> twoFactorAuth, ICaptchaSolver captchaSolver)
        {
            logger.Info("Invoke auth with login and password");
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

            logger.Info($"User '{user[0].Id}' successful sign in");


            return vkApi.Token;
        }

        public async Task SetTokenAsync(string token, ICaptchaSolver captchaSolver)
        {
            logger.Info("Set user token");

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
            logger.Info("Invoke 'catalog.getAudio' ");

            var parameters = new VkParameters
            {
                {"v", vkApiVersion},
                {"lang", "ru"},
                {"extended", "1"},
                {"access_token", vkApi.Token}
            };

            var json = await vkApi.InvokeAsync("catalog.getAudio", parameters);
            var model = JsonConvert.DeserializeObject<ResponseVk>(json);

            logger.Info("Succeful invoke 'catalog.getAudio' ");

            return model.Response;

        }

        public async Task<ResponseData> GetSectionAsync(string sectionId, string startFrom = null)
        {
            logger.Info($"Invoke 'catalog.getSection' with sectionId = '{sectionId}' ");

            var parameters = new VkParameters
            {
                {"v", vkApiVersion},
                {"lang", "ru"},
                {"extended", "1"},
                {"access_token", vkApi.Token},
                {"section_id", sectionId },
            };

            if (startFrom != null) parameters.Add("start_from", startFrom);


            var json = await vkApi.InvokeAsync("catalog.getSection", parameters);
            var model = JsonConvert.DeserializeObject<ResponseVk>(json);

            logger.Info("Succeful invoke 'catalog.getSection' ");


            return model.Response;

        }

        public async Task<ResponseData> GetBlockItemsAsync(string blockId)
        {
            logger.Info($"Invoke 'catalog.getBlockItems' with blockId = '{blockId}' ");

            var parameters = new VkParameters
            {
                {"v", vkApiVersion},
                {"lang", "ru"},
                {"extended", "1"},
                {"access_token", vkApi.Token},
                {"block_id", blockId },
            };


            var json = await vkApi.InvokeAsync("catalog.getBlockItems", parameters);
            var model = JsonConvert.DeserializeObject<ResponseVk>(json);

            logger.Info("Succeful invoke 'catalog.getBlockItems' ");

            return model.Response;
        }

        public async Task<ResponseData> GetAudioSearchAsync(string query)
        {

            logger.Info($"Invoke 'catalog.getAudioSearch' with query = '{query}' ");

            var parameters = new VkParameters
            {
                {"v", vkApiVersion},
                {"lang", "ru"},
                {"extended", "1"},
                {"access_token", vkApi.Token},
                {"query", query}

            };


            var json = await vkApi.InvokeAsync("catalog.getAudioSearch", parameters);
            var model = JsonConvert.DeserializeObject<ResponseVk>(json);
            logger.Info("Succeful invoke 'catalog.getAudioSearch' ");

            return model.Response;
        }

        public async Task<ResponseData> GetAudioArtistAsync(string artistId)
        {
            logger.Info($"Invoke 'catalog.getAudioArtist' with artistId = '{artistId}' ");

            var parameters = new VkParameters
            {
                {"v", vkApiVersion},
                {"lang", "ru"},
                {"extended", "1"},
                {"access_token", vkApi.Token},
                {"artist_id", artistId}

            };


            var json = await vkApi.InvokeAsync("catalog.getAudioArtist", parameters);
            var model = JsonConvert.DeserializeObject<ResponseVk>(json);

            logger.Info("Succesful invoke 'catalog.getAudioArtist' ");


            return model.Response;
        }

        public async Task<ResponseData> GetPlaylistAsync(int count, long albumId, string accessKey, long ownerId, int offset = 0, int needOwner = 1)
        {
            logger.Info($"Invoke 'execute.getPlaylist' with albumId = '{albumId}' ");

            var parameters = new VkParameters
            {
                {"v", vkApiVersion },

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

            logger.Info("Succesful invoke 'execute.getPlaylist' ");


            return model.Response;
        }

        public async Task AudioAddAsync(long audioId, long ownerId)
        {
            var parameters = new VkParameters
            {
                {"v", vkApiVersion},
                {"lang", "ru"},
                {"access_token", vkApi.Token},
                {"audio_id", audioId},
                {"owner_id", ownerId}
            };


            var json = await vkApi.InvokeAsync("audio.add", parameters);
        }

        public async Task AudioDeleteAsync(long audioId, long ownerId)
        {
            var parameters = new VkParameters
            {
                {"v", vkApiVersion},
                {"lang", "ru"},
                {"access_token", vkApi.Token},
                {"audio_id", audioId},
                {"owner_id", ownerId}
            };


            var json = await vkApi.InvokeAsync("audio.delete", parameters);
        }

        public async Task<User> GetCurrentUserAsync()
        {
            var users = await vkApi.Users.GetAsync(new List<long>(), ProfileFields.Photo200);
            var currentUser = users?.FirstOrDefault();
            return currentUser;
        }

        public async Task<User> GetUserAsync(long userId)
        {
           
            var users = await vkApi.Users.GetAsync(new List<long> { userId }, ProfileFields.Photo200);
            var currentUser = users?.FirstOrDefault();
            return currentUser;
        }

        public async Task<Owner> OwnerAsync(long ownerId)
        {
            if (ownerId > 0)
            {
                var users = await vkApi.Users.GetAsync(new List<long>() { ownerId });
                var user = users?.FirstOrDefault();
                if (user != null)
                {
                    var owner = new Owner()
                    {
                        Id = user.Id,
                        Name = user.LastName + " " + user.FirstName
                    };
                    return owner;
                }
                else return null;
            }
            else
            {
                ownerId *= (-1);
                var groups = await vkApi.Groups.GetByIdAsync(new List<string>() { ownerId.ToString() }, "",
                    GroupsFields.Description);
                var group = groups?.FirstOrDefault();
                if (group != null)
                {
                    var owner = new Owner()
                    {
                        Id = ownerId,
                        Name = group.Name,
                    };
                    return owner;
                }
                else return null;
            }

        }

        public async Task AddPlaylistAsync(long playlistId, long ownerId, string accessKey)
        {
            var parameters = new VkParameters
            {
                {"v", vkApiVersion},
                {"lang", "ru"},
                {"access_token", vkApi.Token},
                {"playlist_id", playlistId},
                {"owner_id", ownerId},

            };

            if (accessKey != null) parameters.Add("access_key", accessKey);

            var json = await vkApi.InvokeAsync("audio.followPlaylist", parameters);
        }

        public async Task DeletePlaylistAsync(long playlistId, long ownerId)
        {
            var result = await vkApi.Audio.DeletePlaylistAsync(ownerId, playlistId);
        }
    }
}
