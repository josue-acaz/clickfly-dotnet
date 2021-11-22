using System;
using System.Collections.Generic;

namespace clickfly.Helpers
{
    public interface INotificator
    {
        bool HasNotification();
        List<Notification> GetNotifications();
        void HandleNotification(Notification notification);
    }
}
