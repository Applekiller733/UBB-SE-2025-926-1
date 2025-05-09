using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using LoanShark.API.Models;
using LoanShark.Domain;
using LoanShark.EF.Repository.SocialRepository;
using LoanShark.Service.SocialService.Interfaces;
using static System.Net.WebRequestMethods;

namespace LoanShark.API.Proxies
{
    public interface ISocialUserServiceProxy
    {
        Task<IRepository> GetRepo();

        Task AddFriend(int userID, int newFriendID);

        Task RemoveFriend(int userID, int oldFriendID);

        Task JoinChat(int userID, int chatID);

        Task LeaveChat(int userID, int chatID);

        Task<List<int>> FilterUsers(string keyword, int userID);

        Task<List<int>> FilterFriends(string keyword, int userID);

        Task<List<int>> GetFriendsIDsByUser(int userID);

        Task<List<User>> GetFriendsByUser(int userID);

        Task<List<int>> GetChatsByUser(int userID);

        Task<List<Chat>> GetCurrentUserChats();

        Task<User?> GetUserById(int userID);

        Task<List<User>> GetNonFriendsUsers(int userID);

        Task<int> GetCurrentUser();

        Task MarkUserAsDangerousAndGiveTimeout(User user);

        Task<bool> IsUserInTimeout(User user);
    }


    public class SocialUserServiceProxy : ISocialUserServiceProxy
    {
        private readonly HttpClient _httpClient;

        public SocialUserServiceProxy(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<User?> GetUserInformation()
        {
            var response = await _httpClient.GetAsync("https://localhost:7097/api/User/Info"); // Adjust port
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var dto = JsonSerializer.Deserialize<UserViewModel>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return new User(dto.UserID, new Cnp(dto.Cnp), dto.Username, dto.FirstName, dto.LastName, new Email(dto.Email), new PhoneNumber(dto.PhoneNumber), new HashedPassword(dto.Password));

        }

        public async Task<IRepository> GetRepo()
        {
            var response = await _httpClient.GetAsync($"https://localhost:7097/api/User/Repo");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IRepository>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        public async Task AddFriend(int userID, int newFriendID)
        {
            var response = await _httpClient.PostAsync($"https://localhost:7097/api/User/{userID}/Friends/{newFriendID}", null);
            response.EnsureSuccessStatusCode();
        }

        public async Task RemoveFriend(int userID, int oldFriendID)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:7097/api/User/{userID}/Friends/{oldFriendID}");
            response.EnsureSuccessStatusCode();
        }

        public async Task JoinChat(int userID, int chatID)
        {
            var response = await _httpClient.PostAsync($"https://localhost:7097/api/User/{userID}/Chats/{chatID}", null);
            response.EnsureSuccessStatusCode();
        }

        public async Task LeaveChat(int userID, int chatID)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:7097/api/User/{userID}/Chats/{chatID}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<List<int>> FilterUsers(string keyword, int userID)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7097/api/User/Filter?keyword={Uri.EscapeDataString(keyword)}&userID={userID}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<int>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        public async Task<List<int>> FilterFriends(string keyword, int userID)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7097/api/User/{userID}/Friends/Filter?keyword={Uri.EscapeDataString(keyword)}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<int>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        public async Task<List<int>> GetFriendsIDsByUser(int userID)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7097/api/User/{userID}/Friends/IDs");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<int>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        public async Task<List<User>> GetFriendsByUser(int userID)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7097/api/User/{userID}/Friends");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<User>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        public async Task<List<int>> GetChatsByUser(int userID)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7097/api/User/{userID}/Chats");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<int>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        public async Task<List<Chat>> GetCurrentUserChats()
        {
            var response = await _httpClient.GetAsync($"https://localhost:7097/api/User/Chats/Current");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Chat>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        public async Task<User> GetUserById(int userID)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7097/api/User/{userID}/Info");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var dto = JsonSerializer.Deserialize<UserViewModel>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return new User(dto.UserID, new Cnp(dto.Cnp), dto.Username, dto.FirstName, dto.LastName,
                new Email(dto.Email), new PhoneNumber(dto.PhoneNumber), new HashedPassword(dto.Password));
        }

        public async Task<List<User>> GetNonFriendsUsers(int userID)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7097/api/User/{userID}/NonFriends");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<User>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        public async Task<int> GetCurrentUser()
        {
            try
            {
                var response = await _httpClient.GetAsync($"https://localhost:7097/api/User/Current");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                var dto = JsonSerializer.Deserialize<UserViewModel>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return dto.UserID;
            }
            catch (Exception ex)
            {
                throw new Exception("GetCurrentUser Exception " + ex.Message.ToString());
            }
        }

        public async Task MarkUserAsDangerousAndGiveTimeout(User user)
        {
            var content = new StringContent(JsonSerializer.Serialize(new { UserID = user.GetUserId() }),
                System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"https://localhost:7097/api/User/Dangerous", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task<bool> IsUserInTimeout(User user)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7097/api/User/{user.GetUserId()}/Timeout");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<bool>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
    }
}