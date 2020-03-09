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
            _api = api;
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


            var user = await _api.Users.GetAsync(new List<long>());
            _api.UserId = user[0].Id;

            return _api.Token;
        }


        public async Task AutoAsync(string token, ICaptchaSolver captchaSolver)
        {
            _api.CaptchaSolver = captchaSolver;
            await _api.AuthorizeAsync(new ApiAuthParams()
            {
                AccessToken = token
            });

            var user= await _api.Users.GetAsync(new List<long>());
            _api.UserId = user[0].Id;
        }

        public void Auto(string token, ICaptchaSolver captchaSolver)
        {
            _api.CaptchaSolver = captchaSolver;
            _api.Authorize(new ApiAuthParams()
            {
                AccessToken = token
            });

            var user = _api.Users.Get(new List<long>());
            _api.UserId = user[0].Id;
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

            var user = _api.Users.Get(new List<long>());
            _api.UserId = user[0].Id;

            return _api.Token;
        }

    }
}
