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
        private readonly VkApi _api;
        public Auth(VkApi api)
        {
            api = _api;
        }
        public async Task<string> UserAsync(string login, string password, Func<string> twoFactorAuth, ICaptchaSolver captchaSolver)
        {
            _api.CaptchaSolver = captchaSolver;
            await _api.AuthorizeAsync(new ApiAuthParams()
            {
                Login = login,
                Password = password,
                TwoFactorAuthorization = twoFactorAuth
            });

            var userInfo = await  Users.Info.CurrentUser();
            StaticContent.UserId = userInfo.Id;
            return _api.Token;
        }


        public async Task AutoAsync(string token, ICaptchaSolver captchaSolver)
        {
            _api.CaptchaSolver = captchaSolver;
            await _api.AuthorizeAsync(new ApiAuthParams()
            {
                AccessToken = token
            });

            var userInfo = await Users.Info.CurrentUser();
            StaticContent.UserId = userInfo.Id;
        }

        public static void Auto(string token, ICaptchaSolver captchaSolver)
        {
            var services = new ServiceCollection();
            services.AddAudioBypass();

            var api = new VkApi(services);
            api.CaptchaSolver = captchaSolver;
            api.Authorize(new ApiAuthParams()
            {
                AccessToken = token
            });

            StaticContent.VkApi = api;
            var userInfo = Users.Info.CurrentUserSync();
            StaticContent.UserId = userInfo.Id;
        }

        public string User(string login, string password, Func<string> twoFactorAuth, ICaptchaSolver captchaSolver)
        {
            _api.CaptchaSolver = captchaSolver;
            _api.Authorize(new ApiAuthParams()
            {
                Login = login,
                Password = password,
                TwoFactorAuthorization = twoFactorAuth
            });

            var userInfo = Users.Info.CurrentUserSync();
            StaticContent.UserId = userInfo.Id;
            return _api.Token;
        }

    }
}
