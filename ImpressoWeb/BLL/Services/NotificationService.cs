using BLL.AutoMapper;
using BLL.Interfaces;
using BLL.ViewModels.API;
using DAL.Interfaces;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Services
{
    public class NotificationService : Service<Notification, NotificationViewModel>, INotificationService
    {
        public NotificationService(IUnitOfWork unitOfWork) : base(unitOfWork, unitOfWork.Notifications)
        {

        }

        public IEnumerable<NotificationViewModel> GetAllByUserId(string userId)
        {
            var result = Mapping.Map<IEnumerable<Notification>, IEnumerable<NotificationViewModel>>(Database.Notifications.GetAll()
                .Where(notification => notification.AppUserId == userId));
            return result;
        }

        public void ChangeNewest(string userId)
        {
            var notifications = Database.Notifications.GetAll().Where(notification => notification.AppUserId == userId);
            foreach (var notification in notifications)
                notification.IsNewest = false;
            Database.Save();
        }
    }
}
