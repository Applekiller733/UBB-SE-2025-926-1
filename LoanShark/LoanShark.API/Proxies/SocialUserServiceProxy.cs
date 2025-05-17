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
            var response = await _httpClient.GetAsync("https://localhost:7097/api/SocialUser/Info"); // Adjust port
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
            var response = await _httpClient.GetAsync($"https://localhost:7097/api/SocialUser/Repo");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IRepository>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        public async Task AddFriend(int userID, int newFriendID)
        {
            var response = await _httpClient.PostAsync($"https://localhost:7097/api/SocialUser/{userID}/Friends/{newFriendID}", null);
            response.EnsureSuccessStatusCode();
        }

        public async Task RemoveFriend(int userID, int oldFriendID)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:7097/api/SocialUser/{userID}/Friends/{oldFriendID}");
            response.EnsureSuccessStatusCode();
        }

        public async Task JoinChat(int userID, int chatID)
        {
            var response = await _httpClient.PostAsync($"https://localhost:7097/api/SocialUser/{userID}/Chats/{chatID}", null);
            response.EnsureSuccessStatusCode();
        }

        public async Task LeaveChat(int userID, int chatID)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:7097/api/SocialUser/{userID}/Chats/{chatID}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<List<int>> FilterUsers(string keyword, int userID)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7097/api/SocialUser/Filter?keyword={Uri.EscapeDataString(keyword)}&userID={userID}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<int>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        public async Task<List<int>> FilterFriends(string keyword, int userID)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7097/api/SocialUser/{userID}/Friends/Filter?keyword={Uri.EscapeDataString(keyword)}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<int>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        public async Task<List<int>> GetFriendsIDsByUser(int userID)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7097/api/SocialUser/{userID}/Friends/IDs");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<int>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        public async Task<List<User>> GetFriendsByUser(int userID)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7097/api/SocialUser/{userID}/Friends");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var socialEF = JsonSerializer.Deserialize<List<SocialUserViewModel>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            var result = new List<User>();
            foreach (var user in socialEF)
            {
                var newUser = new User(user.UserID, new Cnp(user.Cnp), user.Username, user.FirstName, user.LastName,
                    new Email(user.Email), new PhoneNumber(user.PhoneNumber), new HashedPassword(user.HashedPassword.ToString()));
                result.Add(newUser);
            }
            return result;

        }

        public async Task<List<int>> GetChatsByUser(int userID)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7097/api/SocialUser/{userID}/Chats");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<int>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        public async Task<List<Chat>> GetCurrentUserChats()
        {
            var response = await _httpClient.GetAsync($"https://localhost:7097/api/SocialUser/chats/current");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(json) || json == "[]")
            {
                System.Diagnostics.Debug.WriteLine("No chats found in response");
                return new List<Chat>();
            }

            return JsonSerializer.Deserialize<List<Chat>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                IncludeFields = true,
            });

        }

        public async Task<User> GetUserById(int userID)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7097/api/SocialUser/{userID}/Info");
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
            var response = await _httpClient.GetAsync($"https://localhost:7097/api/SocialUser/{userID}/NonFriends");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(json))
            {
                return new List<User>();
            }
            var userVM =  await response.Content.ReadFromJsonAsync<List<SocialUserViewModel>>();
            //var userVM = JsonSerializer.Deserialize<List<SocialUserViewModel>>(json, new JsonSerializerOptions
            //{
            //    PropertyNameCaseInsensitive = true
            //});
            if (userVM.Count == 0)
                return new List<User>();
            var list = new List<User>();
            foreach (var vm in userVM)
            {
                var user = new User
                {
                    UserID = vm.UserID,
                    Username = vm.Username,
                    FirstName = vm.FirstName,
                    LastName = vm.LastName,
                    Email = new Email(vm.Email.ToString()),
                    PhoneNumber = new PhoneNumber(vm.PhoneNumber.ToString()),
                    Cnp = new Cnp(vm.Cnp.ToString()),
                    HashedPassword = new HashedPassword(vm.HashedPassword.ToString())
                };
                list.Add(user);
            }
            return list;
        }

        public async Task<int> GetCurrentUser()
        {
            try
            {
                return UserSession.Instance.GetUserData("id_user") != null ? int.Parse(UserSession.Instance.GetUserData("id_user")) : 0;
                //var response = await _httpClient.GetAsync($"https://localhost:7097/api/SocialUser/Current");
                //response.EnsureSuccessStatusCode();
                //var json = await response.Content.ReadAsStringAsync();
                //var dto = JsonSerializer.Deserialize<int>(json, new JsonSerializerOptions
                //{
                //    PropertyNameCaseInsensitive = true
                //});
                //return dto;
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
            var response = await _httpClient.PostAsync($"https://localhost:7097/api/SocialUser/Dangerous", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task<bool> IsUserInTimeout(User user)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7097/api/SocialUser/{user.GetUserId()}/Timeout");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<bool>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        //IRepository IUserService.GetRepo()
        //{
        //    throw new NotImplementedException();
        //}

        //int IUserService.GetCurrentUser()
        //{
        //    throw new NotImplementedException();
        //}

        //void IUserService.MarkUserAsDangerousAndGiveTimeout(User user)
        //{
        //    throw new NotImplementedException();
        //}

        //bool IUserService.IsUserInTimeout(User user)
        //{
        //    throw new NotImplementedException();
        //}
    }
}