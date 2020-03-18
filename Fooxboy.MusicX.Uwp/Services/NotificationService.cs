using Fooxboy.MusicX.Uwp.Models;
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

        }

        public void CreateNotification(string title, string description)
        {

        }
    }
}
