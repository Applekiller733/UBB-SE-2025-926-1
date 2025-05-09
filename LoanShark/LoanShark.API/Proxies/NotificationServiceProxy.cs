using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using LoanShark.API.Models;
using LoanShark.Domain;
using System.Collections.Generic;

namespace LoanShark.API.Proxies
{
    public interface INotificationServiceProxy
    {
        Task<List<Notification>?> GetNotifications(int userId);
        Task SendFriendNotification(int userId, int newFriendId);
        Task SendRemoveFriendNotification(int userId, int oldFriendId);
        Task SendMessageNotification(int messageSenderId, int chatId);
        Task SendTransactionNotification(int receiverId, int chatId, string type, float amount, string currency);
        Task SendNewChatNotification(int chatId);
        Task ClearNotification(int notificationId);
        Task ClearAllNotifications(int userId);
    }

    public class NotificationServiceProxy : INotificationServiceProxy
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        public NotificationServiceProxy(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Notification>?> GetNotifications(int userId)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7097/api/Notification/user/{userId}");
            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            var dtos = JsonSerializer.Deserialize<List<NotificationViewModel>>(json, _jsonOptions);
            if (dtos == null) return null;

            return dtos.Select(dto => new Notification(
                dto.NotificationID,
                dto.Timestamp,
                dto.Content,
                dto.UserReceiverID
            )).ToList();
        }

        public async Task SendFriendNotification(int userId, int newFriendId)
        {
            var content = JsonContent.Create(new { UserId = userId, NewFriendId = newFriendId });
            var response = await _httpClient.PostAsync("https://localhost:7097/api/Notification/friend", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task SendRemoveFriendNotification(int userId, int oldFriendId)
        {
            var content = JsonContent.Create(new { UserId = userId, OldFriendId = oldFriendId });
            var response = await _httpClient.PostAsync("https://localhost:7097/api/Notification/remove-friend", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task SendMessageNotification(int messageSenderId, int chatId)
        {
            var content = JsonContent.Create(new { MessageSenderId = messageSenderId, ChatId = chatId });
            var response = await _httpClient.PostAsync("https://localhost:7097/api/Notification/message", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task SendTransactionNotification(int receiverId, int chatId, string type, float amount, string currency)
        {
            var content = JsonContent.Create(new { ReceiverId = receiverId, ChatId = chatId, Type = type, Amount = amount, Currency = currency });
            var response = await _httpClient.PostAsync("https://localhost:7097/api/Notification/transaction", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task SendNewChatNotification(int chatId)
        {
            var content = JsonContent.Create(new { ChatId = chatId });
            var response = await _httpClient.PostAsync("https://localhost:7097/api/Notification/new-chat", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task ClearNotification(int notificationId)
        {
            var content = JsonContent.Create(notificationId);
            var response = await _httpClient.PostAsync("https://localhost:7097/api/Notification/clear", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task ClearAllNotifications(int userId)
        {
            var content = JsonContent.Create(userId);
            var response = await _httpClient.PostAsync("https://localhost:7097/api/Notification/clear-all", content);
            response.EnsureSuccessStatusCode();
        }
    }
}