using Application.Dto.Notification;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface.Serv.Notifications

{
    public interface INotificationService
    {
        Task NotifyUserAsync(int userId,string message);
        Task<List<NotificationDto>> GetUserNotificationsAsync(int userId);
    }
}
