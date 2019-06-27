using System;
using System.Collections.Generic;
using System.Text;
using VkNet.AudioBypassService.Extensions;
using Microsoft.Extensions.DependencyInjection;
using VkNet;
using VkNet.Model;
using System.Threading.Tasks;
using VkNet.Utils.AntiCaptcha;

namespace Fooxboy.MusicX.Core.VKontakte
{
    public class Auth
    {
        public async static Task<string> User(string login, string password, Func<string> twoFactorAuth, ICaptchaSolver captchaSolver)
        {
            var services = new ServiceCollection();
            services.AddAudioBypass();
            var api = new VkApi(services);


            api.CaptchaSolver = captchaSolver;


            await api.AuthorizeAsync(new ApiAuthParams()
            {
                Login = login,
                Password = password,
                TwoFactorAuthorization = twoFactorAuth
            });

            StaticContent.VkApi = api;
            var userInfo = await  Users.Info.CurrentUser();
            StaticContent.UserId = userInfo.Id;
            return api.Token;
        }


        public async static Task Auto(string token, ICaptchaSolver captchaSolver)
        {
            var services = new ServiceCollection();
            services.AddAudioBypass();

            var api = new VkApi(services);
            api.CaptchaSolver = captchaSolver;
            await api.AuthorizeAsync(new ApiAuthParams()
            {
                AccessToken = token
            });

            StaticContent.VkApi = api;
            var userInfo = await Users.Info.CurrentUser();
            StaticContent.UserId = userInfo.Id;
        }

    }
}
