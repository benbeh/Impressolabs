using BLL.ViewModels.API;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Interfaces
{
    public interface INotificationService : IService<Notification, NotificationViewModel>
    {
        IEnumerable<NotificationViewModel> GetAllByUserId(string userId);

        void ChangeNewest(string userId);
    }
}
