using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Core;
using Fooxboy.MusicX.Core.Interfaces;

namespace Fooxboy.MusicX.Uwp.Services
{
    public class CurrentUserService
    {
        private Api _api;
        public CurrentUserService(Api api)
        {
            _api = api;
        }

        public long UserId { get; private set; }
        public IUserInfo UserInfo { get; private set; }

        public async Task Init()
        {
            var user = await _api.VKontakte.Users.Info.CurrentUserAsync();

            this.UserId = user.Id;
            this.UserInfo = user;
        }

    }
}
