using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Core.Interfaces;
using Fooxboy.MusicX.Core.Models;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model.RequestParams;

namespace Fooxboy.MusicX.Core.VKontakte.Users
{
    public class Info
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

        public async Task<IUserInfo> OwnerAsync(long ownerId)
        {
            if (ownerId > 0)
            {
                var users = await _api.Users.GetAsync(new List<long>() { ownerId });
                var user = users?.FirstOrDefault();
                if (user != null)
                {
                    IUserInfo info = new UserInfo()
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        PhotoUser = null
                    };
                    return info;
                }
                else return null;
            }
            else
            {
                ownerId = ownerId * (-1);
                var groups = await _api.Groups.GetByIdAsync(new List<string>() {ownerId.ToString()}, "",
                    GroupsFields.Description);
                var group = groups?.FirstOrDefault();
                if (group != null)
                {
                    IUserInfo info = new UserInfo()
                    {
                        Id = ownerId, FirstName = group.Name, LastName = "", PhotoUser = null
                    };
                    return info;
                }
                else return null;
            }
        }
    }
}
