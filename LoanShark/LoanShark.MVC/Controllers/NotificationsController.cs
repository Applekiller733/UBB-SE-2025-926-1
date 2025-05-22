using LoanShark.API.Proxies;
using LoanShark.Domain;
using LoanShark.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace LoanShark.MVC.Controllers
{
    public class NotificationsController : Controller
    {
        private readonly INotificationServiceProxy notificationService;

        public NotificationsController(INotificationServiceProxy notificationService)
        {
            this.notificationService = notificationService;
        }

        public async Task<IActionResult> Index()
        {
            var notificationItems = await LoadNotifications();

            return View(notificationItems);
        }

        public async Task<List<NotificationViewModel>> LoadNotifications()
        {
            // hardcoded user for now, change it when login is implemented!!!
            var userId = 2;

            List<Notification> notifications = await this.notificationService.GetNotifications(userId);

            // sort descending by timestamp to display them right!
            var sortedNotificationsDescByTimestamp = notifications
                .OrderByDescending(notification => notification.Timestamp)
                .Select(notification =>
                    new NotificationViewModel
                    {
                        Id = notification.NotificationID,
                        Content = notification.Content,
                    })
                .ToList();

            return sortedNotificationsDescByTimestamp;
        }

        [HttpPost]
        public async Task<IActionResult> Clear(int notificationId)
        {
            await this.notificationService.ClearNotification(notificationId);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ClearAll()
        {
            // here the userId will be the one actually logged!!!
            var userId = 2;

            await this.notificationService.ClearAllNotifications(userId);

            return RedirectToAction("Index");
        }
    }
}
