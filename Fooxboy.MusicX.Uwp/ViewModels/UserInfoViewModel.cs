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

        public UserInfoViewModel()
        {
           UserPhotoUri = "https://docs.microsoft.com/windows/uwp/contacts-and-calendar/images/shoulder-tap-static-payload.png";
        }
        public async Task StartLoadingUserInfo()
        {
            var api = Container.Get.Resolve<Core.Api>();
            var usr = await api.VKontakte.Users.Info.CurrentUserAsync();
            UserPhotoUri = usr.PhotoUser;
            Changed("UserPhotoUri");
            UserInfo = usr;
            Changed("UserInfo");
        }

    }
}
