using Application.Interface.Serv.Notifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FitCore_Manager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }


        [HttpPost("send")]
        public async Task<IActionResult> SendNotification(int userId, string message)
        {
            await _notificationService.NotifyUserAsync(userId, message);
            return Ok("Notification send");
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserNotifications(int userId)
        {
            var notifs = await _notificationService.GetUserNotificationsAsync(userId);
            return Ok(notifs);
        }

    }
}
