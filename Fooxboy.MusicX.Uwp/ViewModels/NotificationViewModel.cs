using DryIoc;
using Fooxboy.MusicX.Uwp.Models;
using Fooxboy.MusicX.Uwp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fooxboy.MusicX.Uwp.ViewModels
{
    public class NotificationViewModel:BaseViewModel
    {
        public ObservableCollection<Notification> Notifications { get; set; }

        private NotificationService _notificationService;
        public NotificationViewModel()
        {
            Notifications = new ObservableCollection<Notification>();
            _notificationService = Container.Get.Resolve<NotificationService>();
            _notificationService.NewNotificationEvent += NewNotification;
            _notificationService.CloseNotificationEvent += CloseNotification;


        }

        private void CloseNotification(Notification notification)
        {
            Notifications.Remove(notification);
            Changed("Notifications");
        }

        private void NewNotification(Notification notification)
        {
            Notifications.Add(notification);
            Changed("Notifications");
        }
    }
}
