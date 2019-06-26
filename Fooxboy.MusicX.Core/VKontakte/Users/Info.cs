using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Core.Interfaces;
using Fooxboy.MusicX.Core.Models;

namespace Fooxboy.MusicX.Core.VKontakte.Users
{
    public static class Info
    {
        public async static Task<IUserInfo> CurrentUser()
        {
            var users = await StaticContent.VkApi.Users.GetAsync(new List<long>());
            var currentUser = users.FirstOrDefault();
            IUserInfo userInfo = new UserInfo()
            {
                Id = currentUser.Id,
                FirstName = currentUser.FirstName,
                LastName = currentUser.LastName
            };
            return userInfo;
        }
    }
}
