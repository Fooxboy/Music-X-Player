using Fooxboy.MusicX.Core.Discord;
using Fooxboy.MusicX.Core.VKontakte;
using System;
using System.Collections.Generic;
using System.Text;
using Fooxboy.MusicX.Core.LastFM;

namespace Fooxboy.MusicX.Core
{
    public class Api
    {
        private Api()
        {
            VKontakte = new Vk();
            Discord = new RichPresenceDiscord();
            LastFM = new LastFmScrobblerApi();
        }
        private static Api _api;
        public static ILoggerService Logger;
        public static Api GetApi(ILoggerService logger)
        {
            Logger = logger;
            if(_api == null) _api = new Api();

            return _api;
        }

        public Vk VKontakte { get; set; }

        public RichPresenceDiscord Discord { get; set; }
        public LastFmScrobblerApi LastFM { get; set; }

    }
}
