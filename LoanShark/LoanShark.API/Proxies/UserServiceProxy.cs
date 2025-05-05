using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using LoanShark.API.Models;
using LoanShark.Domain;

//add namespace LoanShark.API.Proxies;

namespace LoanShark.API.Proxies
{
    public interface IUserServiceProxy
    {
        Task<User?> GetUserInformationAsync();
    }


    public class UserServiceProxy : IUserServiceProxy
    {
        private readonly HttpClient _httpClient;

        public UserServiceProxy(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<User?> GetUserInformationAsync()
        {
            var response = await _httpClient.GetAsync("https://localhost:7097/api/User/Info"); // Adjust port
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var dto = JsonSerializer.Deserialize<UserViewModel>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return new User(dto.UserID, new Cnp(dto.Cnp),dto.Username, dto.FirstName, dto.LastName, new Email(dto.Email), new PhoneNumber(dto.PhoneNumber), new HashedPassword(dto.Password));

        }
    }
}
