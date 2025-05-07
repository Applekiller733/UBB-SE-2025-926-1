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
    public class TransactionsServiceProxy : ITransactionsService
    {
        private readonly HttpClient _httpClient;

        public TransactionsServiceProxy(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> AddTransaction(string senderIban, string receiverIban, decimal amount, string transactionDescription = "")
        {
            try
            {
                var dto = new AddTransactionDTO
                {
                    SenderIban = senderIban,
                    ReceiverIban = receiverIban,
                    Amount = amount,
                    TransactionDescription = transactionDescription
                };

                var content = new StringContent(
                    JsonSerializer.Serialize(dto),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await _httpClient.PostAsync("https://localhost:7097/api/Transactions/AddTransaction", content);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<string>(json);
                return result ?? "empty string";
            }
            catch (Exception ex)
            {
                throw new Exception("EROARE ADD TRANSACTION PROXY", ex);
            }
        }

        public async Task<string> TakeLoanTransaction(string iban, decimal loanAmount)
        {
            var dto = new TakeLoanTransactionDTO
            {
                Iban = iban,
                LoanAmount = loanAmount
            };

            var json = JsonSerializer.Serialize(dto);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                using var response = await _httpClient.PostAsync("https://localhost:7097/api/Transactions/PayLoanTransaction", content);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();

                return !string.IsNullOrWhiteSpace(responseBody)
                    ? responseBody
                    : "empty string";
            }
            catch (HttpRequestException httpEx)
            {
                throw new Exception("HTTP request failed while taking loan transaction.", httpEx);
            }
            catch (JsonException jsonEx)
            {
                throw new Exception("Failed to parse the response JSON.", jsonEx);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while taking loan transaction.", ex);
            }
        }


        public async Task<string> PayLoanTransaction(string iban, decimal paymentAmount)
        {
            var dto = new PayLoanTransactionDTO
            {
                Iban = iban,
                PaymentAmount = paymentAmount
            };

            var json = JsonSerializer.Serialize(dto);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                using var response = await _httpClient.PostAsync("https://localhost:7097/api/Transactions/PayLoanTransaction", content);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();

                return !string.IsNullOrWhiteSpace(responseBody)
                    ? responseBody
                    : "empty string";
            }
            catch (HttpRequestException httpEx)
            {
                throw new Exception("HTTP request failed while paying loan transaction.", httpEx);
            }
            catch (JsonException jsonEx)
            {
                throw new Exception("Failed to parse the response JSON.", jsonEx);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while paying loan transaction.", ex);
            }
        }


        public async Task<List<CurrencyExchange>> GetAllCurrencyExchangeRates()
        {
            try
            {
                var response = await _httpClient.GetAsync("https://localhost:7097/api/Transactions/GetAllCurrencyExchangeRates"); // Adjust port
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                var dto = JsonSerializer.Deserialize<List<CurrencyExchange>>(json);
                return dto != null ? dto : new List<CurrencyExchange>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving currency exchange rates: {ex.Message}", ex);
            }
        }
    }
    public class TakeLoanTransactionDTO
    {
        public string Iban { get; set; }
        public decimal LoanAmount { get; set; }
    }

    public class AddTransactionDTO
    {
        public string SenderIban { get; set; }
        public string ReceiverIban { get; set; }
        public decimal Amount { get; set; }
        public string TransactionDescription { get; set; }
    }

    public class PayLoanTransactionDTO
    {
        public string Iban { get; set; }
        public decimal PaymentAmount { get; set; }
    }

}