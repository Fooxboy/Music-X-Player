using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Core.Interfaces;
using Fooxboy.MusicX.Core.Models;
using VkNet.Enums.Filters;

namespace Fooxboy.MusicX.Core.VKontakte.Users
{
    public static class Info
    {
        public async static Task<IUserInfo> CurrentUser()
        {
            var users = await StaticContent.VkApi.Users.GetAsync(new List<long>(), ProfileFields.Photo200);
            var currentUser = users.FirstOrDefault();
            IUserInfo userInfo = new UserInfo()
            {
                Id = currentUser.Id,
                FirstName = currentUser.FirstName,
                LastName = currentUser.LastName,
                PhotoUser = currentUser.Photo200.ToString()
            };
            return userInfo;
        }
    }
}
