using Fooxboy.MusicX.Core.VKontakte.Users;
using System;
using System.Collections.Generic;
using System.Text;
using VkNet;

namespace Fooxboy.MusicX.Core.VKontakte
{
    public class UsersApi
    {
        public UsersApi(VkApi api)
        {
            Info = new Info(api);
        }
        public Info Info { get; set; }
    }
}
