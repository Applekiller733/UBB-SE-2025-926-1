using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using LoanShark.Domain;
using LoanShark.Service.BankService;

namespace LoanShark.API.Proxies
{
    public class TransactionHistoryProxy : ITransactionHistoryService
    {
        private readonly HttpClient _httpClient;

        public string iban { get; set; }

        public TransactionHistoryProxy(HttpClient httpClient)
        {
            _httpClient = httpClient;
            iban = UserSession.Instance.GetUserData("current_bank_account_iban");
        }

        public async Task<ObservableCollection<string>> RetrieveForMenu()
        {
            try
            {
                var response = await _httpClient.GetAsync($"https://localhost:7097/api/TransactionHistory/RetrieveForMenu/{iban}");
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadFromJsonAsync<ObservableCollection<string>>();
                return result ?? new ObservableCollection<string>();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving transactions for menu", ex);
            }
        }

        public async Task<ObservableCollection<string>> FilterByTypeForMenu(string type)
        {
            try
            {
                var response = await _httpClient.GetAsync($"https://localhost:7097/api/TransactionHistory/FilterByTypeForMenu?type={type}");
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadFromJsonAsync<ObservableCollection<string>>();
                return result ?? new ObservableCollection<string>();
            }
            catch (Exception ex)
            {
                throw new Exception("Error filtering transactions by type for menu", ex);
            }
        }

        public async Task<ObservableCollection<string>> FilterByTypeDetailed(string type)
        {
            try
            {
                var response = await _httpClient.GetAsync($"https://localhost:7097/api/TransactionHistory/FilterByTypeDetailed?type={type}");
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadFromJsonAsync<ObservableCollection<string>>();

                return result ?? new ObservableCollection<string>();
            }
            catch (Exception ex)
            {
                throw new Exception("Error filtering transactions by type detailed", ex);
            }
        }

        public async Task<ObservableCollection<string>?> SortByDate(string order)
        {
            try
            {
                var response = await _httpClient.GetAsync($"https://localhost:7097/api/TransactionHistory/SortByDate?order={order}");
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadFromJsonAsync<ObservableCollection<string>>();

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error sorting transactions by date", ex);
            }
        }

        public async Task CreateCSV()
        {
            try
            {
                var response = await _httpClient.PostAsync($"https://localhost:7097/api/TransactionHistory/CreateCSV/{iban}", null);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating CSV file", ex);
            }
        }

        public async Task<Transaction> GetTransactionByMenuString(string menuString)
        {
            try
            {
                var response = await _httpClient.GetAsync($"https://localhost:7097/api/TransactionHistory/GetTransactionByMenuString?menuString={menuString}");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<Transaction>(json);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting transaction by menu string", ex);
            }
        }

        public async Task<Dictionary<string, int>> GetTransactionTypeCounts()
        {
            try
            {
                var response = await _httpClient.GetAsync("https://localhost:7097/api/TransactionHistory/GetTransactionTypeCounts");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<Dictionary<string, int>>(json);
                return result ?? new Dictionary<string, int>();
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting transaction type counts", ex);
            }
        }

        public async Task UpdateTransactionDescription(int transactionId, string newDescription)
        {
            try
            {
                var dto = new UpdateTransactionDescriptionDTO
                {
                    TransactionId = transactionId,
                    NewDescription = newDescription
                };

                var json = JsonSerializer.Serialize(dto);
                using var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync("https://localhost:7097/api/TransactionHistory/UpdateTransactionDescription", content);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating transaction description", ex);
            }
        }
    }

    public class UpdateTransactionDescriptionDTO
    {
        public int TransactionId { get; set; }
        public string NewDescription { get; set; }
    }
} 