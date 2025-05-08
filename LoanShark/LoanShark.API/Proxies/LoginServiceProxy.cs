using LoanShark.Domain;
using LoanShark.EF.EFModels;
using LoanShark.Service.Service.BankService;
using System.Diagnostics;

namespace LoanShark.API.Proxies
{
    public class LoginServiceProxy : ILoginService
    {
        private readonly HttpClient httpClient;

        public LoginServiceProxy(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<bool> ValidateUserCredentials(string email, string password)
        {
            var requestDto = new LoginRequestDto
            {
                Email = email,
                Password = password,
            };

            var response = await this.httpClient.PostAsJsonAsync("https://localhost:7097/api/Login/ValidateUserCredentials", requestDto);
            return response.IsSuccessStatusCode;
        }

        public async Task InstantiateUserSessionAfterLogin(string email)
        {
            var response = await this.httpClient.PostAsJsonAsync("https://localhost:7097/api/Login/InstantiateUserSessionAfterLogin", new LoginRequestDto { Email = email });

            if (!response.IsSuccessStatusCode)
            {
                return;
            }

            var sessionData = await response.Content.ReadFromJsonAsync<UserSessionDto>();

            UserSession.Instance.Initialize(
                sessionData.UserId,
                sessionData.Cnp,
                sessionData.FirstName,
                sessionData.LastName,
                sessionData.Email,
                sessionData.PhoneNumber,
                sessionData.Iban);
        }

        public async Task<UserEF> GetUserInfoAfterLogin(string email)
        {
            try
            {
                var response = await this.httpClient.GetAsync("https://localhost:7097/api/Login/GetUserInfoAfterLogin/{email}");

                if (!response.IsSuccessStatusCode)
                    throw new Exception("Failed to get user info");

                var user = await response.Content.ReadFromJsonAsync<UserEF>();
                return user ?? throw new Exception("User info is null");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in proxy GetUserInfoAfterLogin: {ex.Message}");
            }
        }

        public async Task<List<BankAccountEF>> GetUserBankAccounts(int userId)
        {
            try
            {
                var response = await this.httpClient.GetAsync("https://localhost:7097/api/Login/GetUserBankAccounts/{userId}");

                if (!response.IsSuccessStatusCode)
                    throw new Exception("Failed to get bank accounts");

                var accounts = await response.Content.ReadFromJsonAsync<List<BankAccountEF>>();
                return accounts ?? new List<BankAccountEF>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in proxy GetUserBankAccounts: {ex.Message}");
            }
        }
    }

    public class LoginRequestDto
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }

    public class UserSessionDto
    {
        public string UserId { get; set; }

        public string Cnp { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Iban { get; set; }
    }
}