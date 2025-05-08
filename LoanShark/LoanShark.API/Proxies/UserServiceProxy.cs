using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using LoanShark.API.Models;
using LoanShark.Domain;
using LoanShark.Service.BankService;
using Microsoft.Identity.Client;

namespace LoanShark.API.Proxies
{

    public class UserServiceProxy : IUserService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        public UserServiceProxy(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<User?> GetUserInformation()
        {
            var response = await _httpClient.GetAsync("api/User/info");
            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            var dto = JsonSerializer.Deserialize<UserViewModel>(json, _jsonOptions);
            if (dto == null) return null;

            return new User(dto.UserID, new Cnp(dto.Cnp), dto.Username, dto.FirstName, dto.LastName, new Email(dto.Email), new PhoneNumber(dto.PhoneNumber), new HashedPassword(dto.Password));
        }

        public async Task<string?> CheckCnp(string cnp)
        {
            var content = JsonContent.Create(cnp);
            var response = await _httpClient.PostAsync("api/User/check-cnp", content);
            return response.IsSuccessStatusCode ? null : await response.Content.ReadAsStringAsync();
        }

        public async Task<string?> CheckEmail(string email)
        {
            var content = JsonContent.Create(email);
            var response = await _httpClient.PostAsync("api/User/check-email", content);
            return response.IsSuccessStatusCode ? null : await response.Content.ReadAsStringAsync();
        }

        public async Task<string?> CheckPhoneNumber(string phone)
        {
            var content = JsonContent.Create(phone);
            var response = await _httpClient.PostAsync("api/User/check-phone", content);
            return response.IsSuccessStatusCode ? null : await response.Content.ReadAsStringAsync();
        }

        public async Task CreateUser(string cnp, string userName, string firstName, string lastName, string email, string phoneNumber, string password)
        {
            CreateUserDto content = new CreateUserDto
            {
                Cnp = cnp,
                UserName = userName,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                PhoneNumber = phoneNumber,
                Password = password
            };
            var response = await _httpClient.PostAsJsonAsync("api/User/create", content);
            //return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateUser(User user)
        {
            var response = await _httpClient.PutAsJsonAsync("api/User/update", user);
            return response.IsSuccessStatusCode;
        }

        public async Task<string> DeleteUser(string password)
        {
            var content = JsonContent.Create(password);
            var response = await _httpClient.PostAsync("api/User/delete", content);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string[]> GetUserPasswordHashSalt()
        {
            var response = await _httpClient.GetAsync("api/User/hash-salt");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<string[]>() ?? [];
        }


    }
}
