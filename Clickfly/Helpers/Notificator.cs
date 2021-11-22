using System;
using System.Collections.Generic;
using System.Linq;

namespace clickfly.Helpers
{
    public class Notificator : INotificator
    {
        public List<Notification> _notifications;

        public Notificator()
        {
            _notifications = new List<Notification>();
        }

        public List<Notification> GetNotifications()
        {
            return _notifications;
        }

        public void HandleNotification(Notification notification)
        {
            _notifications.Add(notification);
        }

        public bool HasNotification()
        {
            return _notifications.Any();
        }
    }
}
