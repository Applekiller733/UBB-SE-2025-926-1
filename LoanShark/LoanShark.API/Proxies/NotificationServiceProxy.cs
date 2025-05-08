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
        Task<List<Notification>?> GetNotificationsAsync(int userId);
        Task SendFriendNotificationAsync(int userId, int newFriendId);
        Task SendRemoveFriendNotificationAsync(int userId, int oldFriendId);
        Task SendMessageNotificationAsync(int messageSenderId, int chatId);
        Task SendTransactionNotificationAsync(int receiverId, int chatId, string type, float amount, string currency);
        Task SendNewChatNotificationAsync(int chatId);
        Task ClearNotificationAsync(int notificationId);
        Task ClearAllNotificationsAsync(int userId);
    }

    public class NotificationServiceProxy : INotificationServiceProxy
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        public NotificationServiceProxy(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Notification>?> GetNotificationsAsync(int userId)
        {
            var response = await _httpClient.GetAsync($"api/Notification/user/{userId}");
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

        public async Task SendFriendNotificationAsync(int userId, int newFriendId)
        {
            var content = JsonContent.Create(new { UserId = userId, NewFriendId = newFriendId });
            var response = await _httpClient.PostAsync("api/Notification/friend", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task SendRemoveFriendNotificationAsync(int userId, int oldFriendId)
        {
            var content = JsonContent.Create(new { UserId = userId, OldFriendId = oldFriendId });
            var response = await _httpClient.PostAsync("api/Notification/remove-friend", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task SendMessageNotificationAsync(int messageSenderId, int chatId)
        {
            var content = JsonContent.Create(new { MessageSenderId = messageSenderId, ChatId = chatId });
            var response = await _httpClient.PostAsync("api/Notification/message", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task SendTransactionNotificationAsync(int receiverId, int chatId, string type, float amount, string currency)
        {
            var content = JsonContent.Create(new { ReceiverId = receiverId, ChatId = chatId, Type = type, Amount = amount, Currency = currency });
            var response = await _httpClient.PostAsync("api/Notification/transaction", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task SendNewChatNotificationAsync(int chatId)
        {
            var content = JsonContent.Create(new { ChatId = chatId });
            var response = await _httpClient.PostAsync("api/Notification/new-chat", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task ClearNotificationAsync(int notificationId)
        {
            var content = JsonContent.Create(notificationId);
            var response = await _httpClient.PostAsync("api/Notification/clear", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task ClearAllNotificationsAsync(int userId)
        {
            var content = JsonContent.Create(userId);
            var response = await _httpClient.PostAsync("api/Notification/clear-all", content);
            response.EnsureSuccessStatusCode();
        }
    }
}