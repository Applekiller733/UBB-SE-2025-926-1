using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Transactions;
using LoanShark.API.Models;
using LoanShark.Data;
using LoanShark.Domain;
using LoanShark.EF.Repository.BankRepository;
using LoanShark.Service.BankService;

//add namespace LoanShark.API.Proxies;

namespace LoanShark.API.Proxies
{
    public class BankAccountServiceProxy : IBankAccountService
    {
        private readonly HttpClient _httpClient;

        public BankAccountServiceProxy(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<BankAccount>?> GetUserBankAccounts(int userID)
        {
            return await _httpClient.GetFromJsonAsync<List<BankAccount>>($"https://localhost:7097/api/bankaccount/user/{userID}");
        }

        public async Task<BankAccount?> FindBankAccount(string iban)
        {
            return await _httpClient.GetFromJsonAsync<BankAccount>($"https://localhost:7097/api/bankaccount/{iban}");
        }

        public async Task<bool> CreateBankAccount(int userID, string customName, string currency)
        {
            var payload = new
            {
                UserId = userID,
                CustomName = customName,
                Currency = currency
            };
            var response = await _httpClient.PostAsJsonAsync("https://localhost:7097/api/bankaccount", payload);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveBankAccount(string iban)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:7097/api/bankaccount/{iban}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> CheckIBANExists(string iban)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7097/api/bankaccount/exists/{iban}");
            return response.IsSuccessStatusCode && bool.Parse(await response.Content.ReadAsStringAsync());
        }

        public async Task<string> GenerateIBAN()
        {
            return await _httpClient.GetStringAsync("https://localhost:7097/api/bankaccount/generate-iban");
        }

        public async Task<List<string>> GetCurrencies()
        {
            return await _httpClient.GetFromJsonAsync<List<string>>("https://localhost:7097/api/bankaccount/currencies");
        }

        public async Task<bool> VerifyUserCredentials(string email, string password)
        {
            var payload = new { Email = email, Password = password };
            var response = await _httpClient.PostAsJsonAsync("https://localhost:7097/api/bankaccount/verify", payload);
            return response.IsSuccessStatusCode && bool.Parse(await response.Content.ReadAsStringAsync());
        }

        public async Task<bool> UpdateBankAccount(string iban, string name, decimal dailyLimit, decimal maxPerTransaction, int maxNrTransactions, bool blocked)
        {
            var payload = new
            {
                Iban = iban,
                Name = name,
                DailyLimit = dailyLimit,
                MaxPerTransaction = maxPerTransaction,
                MaxNrTransactions = maxNrTransactions,
                Blocked = blocked
            };

            var json = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"https://localhost:7097/api/bankaccount/{iban}", json);
            return response.IsSuccessStatusCode;
        }
    }
}