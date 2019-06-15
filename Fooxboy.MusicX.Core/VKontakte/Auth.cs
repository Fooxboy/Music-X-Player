using System;
using System.Collections.Generic;
using System.Text;
using VkNet.AudioBypassService.Extensions;
using Microsoft.Extensions.DependencyInjection;
using VkNet;
using VkNet.Model;

namespace Fooxboy.MusicX.Core.VKontakte
{
    public class Auth
    {
        public async static void User(string login, string password)
        {
            if (StaticContent.VkApi != null) StaticContent.VkApi = null;

            var services = new ServiceCollection();
            services.AddAudioBypass();

            var api = new VkApi(services);

            await api.AuthorizeAsync(new ApiAuthParams()
            {
                Login = login,
                Password = password
            });

            StaticContent.VkApi = api;

            var userInfo = Users.Info.CurrentUser();
            StaticContent.UserId = userInfo.Id;
            
        }

    }
}
