using Application.Dto.Notification;
using Application.Interface.Reppo.NotificationRepo;
using Domain.Model;
using infrastructure.SignalR;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interface.Serv.Notifications; 



namespace Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _repo;
        private readonly IHubContext<NotificationHub> _hub;
        public NotificationService(INotificationRepository repo, IHubContext<NotificationHub> hub)
        {
            _repo = repo;
            _hub = hub;
        }

        public async Task NotifyUserAsync(int userId, string message)
        {
            var notification = new Notification
            {
                UserId = userId,
                Message = message
            };
            await _repo.AddNotificationAsync(notification);
            await _hub.Clients.User(userId.ToString())
                .SendAsync("ReceiveMessage", message);
        }



        public async Task<List<NotificationDto>> GetUserNotificationsAsync(int userId)

        {
            var notifs = await _repo.GetUserNotificationsAsync(userId);
            return notifs.Select(n => new NotificationDto
            {
                Message = n.Message,
                CreatedAt = n.CreatedAt
            }).ToList();
            
        }
    }
}
