using LoanShark.Domain;
using LoanShark.Domain.MessageClasses;
using LoanShark.EF.Repository.SocialRepository;
using LoanShark.Service.SocialService.Interfaces;
using System.Configuration;
using System.Text.Json;

namespace LoanShark.API.Proxies
{
    public interface IChatServiceProxy
    {
        public Task<int> GetCurrentUserID(); 

        public Task<int> GetNumberOfParticipants(int chatID);

        public Task<IRepository> GetRepo();

        public Task RequestMoneyViaChat(float amount, string currency, int chatID, string description);

        public Task SendMoneyViaChat(float amount, string currency, string description, int chatID);

        public Task AcceptRequestViaChat(float amount, string currency, int accepterID, int requesterID, int chatID);

        public Task<bool> EnoughFunds(float amount, string currency, int senderID);

        public Task InitiateTransfer(int senderID, int reciverID, float amount, string currency);

        public Task CreateChat(List<int> participantsID, string chatName);

        public Task DeleteChat(int chatID);

        public Task<DateTime> GetLastMessageTimeStamp(int chatID);

        public Task<List<Message>> GetChatHistory(int chatID);

        public Task AddUserToChat(int userID, int chatID);

        public Task RemoveUserFromChat(int userID, int chatID);

        public Task<string> GetChatNameByID(int chatID);

        public Task<List<string>> GetChatParticipantsStringList(int chatID);

        public Task<List<User>> GetChatParticipantsList(int chatID);

    }

    public class ChatServiceProxy : IChatServiceProxy
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        public ChatServiceProxy(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<int> GetCurrentUserID()
        {
            try
            {
                var response = await _httpClient.GetAsync("https://localhost:7097/api/Chat/current-user-id");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadFromJsonAsync<int>();
                return content;
            }
            catch (Exception ex)
            {
                throw new Exception("GetCurrentUserId: " + ex.Message.ToString());
            }
        }

        public async Task<int> GetNumberOfParticipants(int chatID)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7097/api/Chat/{chatID}/participants/count");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return int.Parse(content);
        }

        public Task<IRepository> GetRepo()
        {
            throw new NotSupportedException("Repository access is not available via HTTP proxy.");
        }

        public async Task RequestMoneyViaChat(float amount, string currency, int chatID, string description)
        {
            var request = new
            {
                amount,
                currency,
                chatID,
                description
            };
            var content = new StringContent(JsonSerializer.Serialize(request), System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://localhost:7097/api/Chat/request-money", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task SendMoneyViaChat(float amount, string currency, string description, int chatID)
        {
            var request = new
            {
                amount,
                currency,
                description,
                chatID
            };
            var content = new StringContent(JsonSerializer.Serialize(request), System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://localhost:7097/api/Chat/send-money", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task AcceptRequestViaChat(float amount, string currency, int accepterID, int requesterID, int chatID)
        {
            var request = new
            {
                amount,
                currency,
                accepterID,
                requesterID,
                chatID
            };
            var content = new StringContent(JsonSerializer.Serialize(request), System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://localhost:7097/api/Chat/accept-request", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task<bool> EnoughFunds(float amount, string currency, int senderID)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7097/api/Chat/enough-funds?amount={amount}&currency={currency}&senderID={senderID}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return bool.Parse(content);
        }

        public async Task InitiateTransfer(int senderID, int reciverID, float amount, string currency)
        {
            var request = new
            {
                senderID,
                reciverID,
                amount,
                currency
            };
            var content = new StringContent(JsonSerializer.Serialize(request), System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://localhost:7097/api/Chat/initiate-transfer", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task CreateChat(List<int> participantsID, string chatName)
        {
            var request = new
            {
                participantsID,
                chatName
            };
            var content = new StringContent(JsonSerializer.Serialize(request), System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://localhost:7097/api/Chat/create-chat", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteChat(int chatID)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:7097/api/Chat/{chatID}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<DateTime> GetLastMessageTimeStamp(int chatID)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7097/api/Chat/{chatID}/last-message-timestamp");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return DateTime.Parse(content);
        }

        public async Task<List<Message>> GetChatHistory(int chatID)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7097/api/Chat/{chatID}/history");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Message>>(content, _jsonOptions);
        }

        public async Task AddUserToChat(int userID, int chatID)
        {
            var response = await _httpClient.PostAsync($"https://localhost:7097/api/Chat/{chatID}/add-user/{userID}", null);
            response.EnsureSuccessStatusCode();
        }

        public async Task RemoveUserFromChat(int userID, int chatID)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:7097/api/Chat/{chatID}/remove-user/{userID}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<string> GetChatNameByID(int chatID)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7097/api/Chat/{chatID}/name");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<List<string>> GetChatParticipantsStringList(int chatID)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7097/api/Chat/{chatID}/participants/strings");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<string>>(content, _jsonOptions);
        }

        public async Task<List<User>> GetChatParticipantsList(int chatID)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7097/api/Chat/{chatID}/participants");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<User>>(content, _jsonOptions);
        }
    }

}
