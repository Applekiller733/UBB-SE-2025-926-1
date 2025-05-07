using LoanShark.Domain;
using LoanShark.EF.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanShark.EF.Mappers
{
    public static class NotificationMapper
    {
        public static Notification ToDomainNotification(NotificationEF notificationEF)
        {
            if(notificationEF == null)
            {
                throw new ArgumentNullException(nameof(notificationEF));
            }

            try
            {
                Notification notification = new Notification(notificationEF.NotificationID, notificationEF.Timestamp, notificationEF.Content, notificationEF.UserReceiverID);
                return notification;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to convert NotificationEF to Notification: " + ex.Message, ex);
            }
        }
    }
}
