using DryIoc;
using Fooxboy.MusicX.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fooxboy.MusicX.Uwp.ViewModels
{
    public class UserInfoViewModel:BaseViewModel
    {
        public string UserPhotoUri { get; set; }
        public IUserInfo UserInfo { get; set; }
        public string Name { get; set; }

        private IContainer _container;

        public UserInfoViewModel(IContainer container)
        {
            _container = container;
           UserPhotoUri = "https://docs.microsoft.com/windows/uwp/contacts-and-calendar/images/shoulder-tap-static-payload.png";
        }
        public async Task StartLoadingUserInfo()
        {
            var api = _container.Resolve<Core.Api>();
            var usr = await api.VKontakte.Users.Info.CurrentUserAsync();
            UserPhotoUri = usr.PhotoUser;
            Name = usr.FirstName + " " + usr.LastName;
            Changed("UserPhotoUri");
            Changed("Name");
            UserInfo = usr;
            Changed("UserInfo");
        }

    }
}
