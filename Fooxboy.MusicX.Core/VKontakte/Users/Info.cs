using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Core.Interfaces;
using Fooxboy.MusicX.Core.Models;
using VkNet;
using VkNet.Enums.Filters;

namespace Fooxboy.MusicX.Core.VKontakte.Users
{
    public  class Info
    {
        private readonly VkApi _api;
        public Info(VkApi api)
        {
            _api = api;
        }
        public async Task<IUserInfo> CurrentUserAsync()
        {
            var users = await _api.Users.GetAsync(new List<long>(), ProfileFields.Photo200);
            var currentUser = users?.FirstOrDefault();
            if (currentUser != null)
            {
                IUserInfo userInfo = new UserInfo()
                {
                    Id = currentUser.Id,
                    FirstName = currentUser.FirstName,
                    LastName = currentUser.LastName,
                    PhotoUser = currentUser.Photo200.ToString()
                };
                return userInfo;
            }
            else return null;
        }

        public IUserInfo CurrentUser()
        {
            var users =  _api.Users.Get(new List<long>(), ProfileFields.Photo200);
            var currentUser = users?.FirstOrDefault();
            if (currentUser != null)
            {
                IUserInfo userInfo = new UserInfo()
                {
                    Id = currentUser.Id,
                    FirstName = currentUser.FirstName,
                    LastName = currentUser.LastName,
                    PhotoUser = currentUser.Photo200.ToString()
                };
                return userInfo;
            }
            else return null;
        }
    }
}
