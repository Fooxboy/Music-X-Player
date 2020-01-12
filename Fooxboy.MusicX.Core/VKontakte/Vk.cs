using Fooxboy.MusicX.Core.VKontakte.Music;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using VkNet;
using VkNet.AudioBypassService.Extensions;

namespace Fooxboy.MusicX.Core.VKontakte
{
    public class Vk
    {
        public readonly VkApi vkApi;
        public Vk()
        {
            var services = new ServiceCollection();
            services.AddAudioBypass();
            vkApi = new VkApi(services);
            Auth = new Auth(vkApi);
            Music = new MusicApi(vkApi);
            Users = new UsersApi(vkApi);
        }
        public Auth Auth { get; }
        public MusicApi Music { get; }
        public UsersApi Users { get; set; }
    }
}
