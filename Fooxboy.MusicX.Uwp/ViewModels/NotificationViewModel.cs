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
        private IContainer _container;
        public NotificationViewModel(IContainer container)
        {
            this._container = container;
            Notifications = new ObservableCollection<Notification>();
            _notificationService = _container.Resolve<NotificationService>();
            _notificationService.NewNotificationEvent += NewNotification;
            _notificationService.CloseNotificationEvent += CloseNotification;


        }

        private void CloseNotification(Notification notification)
        {
            Notification not;
            if (notification.IsClickButton)
            {
                try
                {
                    not = Notifications.Single(n => n.Title == notification.Title && n.Description == notification.Description);
                }
                catch
                {
                    not = null;
                }
            }
            else not = notification;
            

            try
            {
                Notifications.Remove(not);
            }
            catch
            {

            }
            
            Changed("Notifications");
        }

        private void NewNotification(Notification notification)
        {
            Notifications.Add(notification);
            Changed("Notifications");
        }
    }
}
