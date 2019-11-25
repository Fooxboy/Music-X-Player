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
        }
        private static Api _api;
        public static Api GetApi()
        {
            _api ??= new Api();
            return _api;
        }

        public Vk VKontakte { get; set; }
    }
}
