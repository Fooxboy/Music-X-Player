using Fooxboy.MusicX.Core.Discord;
using Fooxboy.MusicX.Core.VKontakte;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fooxboy.MusicX.Core
{
    public class Api
    {
        private Api()
        {
            VKontakte = new Vk();
            Discord = new RichPresenceDiscord();
        }
        private static Api _api;
        public static Api GetApi()
        {
            if(_api == null) _api = new Api();

            return _api;
        }

        public Vk VKontakte { get; set; }

        public RichPresenceDiscord Discord { get; set; }

    }
}
