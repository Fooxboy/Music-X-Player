using Fooxboy.MusicX.Uwp.Models;
using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Fooxboy.MusicX.Uwp.Services
{
    public class NotificationService
    {
        public event NewNotification NewNotificationEvent;
        public event CloseNotification CloseNotificationEvent;

        public void CreateNotification(string title, string description, string buttonOneText, string buttonTwoText, ICommand buttonOneCommand, ICommand buttonTwoCommand)
        {
            var notification = new Notification();

            if (buttonOneText == null) notification.HasButtons = false;
            else notification.HasButtons = true;

            notification.Title = title;
            notification.Description = description;
            notification.ButtonOneText = buttonOneText;
            notification.IsClickButton = false;
            notification.ButtonTwoText = buttonTwoText;
            notification.ButtonOneCommand = buttonOneCommand;
            notification.ButtonTwoCommand = buttonTwoCommand;

            CreateNotification(notification);
        }

        public void CreateNotification(Notification notification)
        {
            DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                NewNotificationEvent?.Invoke(notification);

            });

            Task.Run(async () =>
            {
                await Task.Delay(3000);
                await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                {
                    ClosedNotification(notification);
                });
            });
        }

        public void ClosedNotification(Notification notification)
        {
            DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                CloseNotificationEvent?.Invoke(notification);
            });
        }

        public void CreateNotification(string title, string description)
        {
            CreateNotification(title, description, null, null, null, null);
        }
    }
}
