using DryIoc;
using Fooxboy.MusicX.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fooxboy.MusicX.Core;
using Fooxboy.MusicX.Uwp.Services;
using Flurl.Http;

namespace Fooxboy.MusicX.Uwp.ViewModels
{
    public class UserInfoViewModel:BaseViewModel
    {
        public string UserPhotoUri { get; set; }
        public IUserInfo UserInfo { get; set; }
        public string Name { get; set; }

        private IContainer _container;
        private ILoggerService _logger;
        private NotificationService _notification;
        public UserInfoViewModel(IContainer container)
        {
            _container = container;
            _logger = _container.Resolve<LoggerService>();
            _notification = _container.Resolve<NotificationService>();
           UserPhotoUri = "https://docs.microsoft.com/windows/uwp/contacts-and-calendar/images/shoulder-tap-static-payload.png";
        }
        public async Task StartLoadingUserInfo()
        {
            try
            {
                _logger.Trace("Загрузка информации о пользователе.");
                var userInfo = _container.Resolve<CurrentUserService>();
                await userInfo.Init();

                var api = _container.Resolve<Core.Api>();
                var usr = await api.VKontakte.Users.Info.CurrentUserAsync();
                _logger.Info($"Информация и пользователе получена: {usr.FirstName} {usr.LastName}");
                UserPhotoUri = usr.PhotoUser;
                Name = usr.FirstName + " " + usr.LastName;
                Changed("UserPhotoUri");
                Changed("Name");
                UserInfo = usr;
                Changed("UserInfo");
            }catch(FlurlHttpException e)
            {
                _logger.Error("Ошибка сети", e);
                _notification.CreateNotification("Ошибка сети", "Попробуйте перезапустить приложение или проверить доступ к Интернету.");
            }catch(Exception e)
            {
                _logger.Error("Неизвестная ошибка", e);
                _notification.CreateNotification("Неизвестная ошибка", "Произошла неизвестная ошибка, подробнее в логах приложения.");

            }

        }

    }
}
