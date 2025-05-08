using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using LoanShark.API.Models;
using LoanShark.Domain;
using LoanShark.EF.Repository.BankRepository;
using LoanShark.Service.BankService;


//add namespace LoanShark.API.Proxies;

namespace LoanShark.API.Proxies
{
    public class LoanServiceProxy : ILoanService
    {
        private readonly HttpClient httpClient;

        public LoanServiceProxy(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        // Get all loans for a specific user
        public async Task<List<Loan>> GetUserLoans(int userId)
        {
            try
            {
                var response = await httpClient.GetAsync($"https://localhost:7097/api/Loan/GetUserLoans/{userId}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();

                // Deserialize the response string into a List<LoanDTO>
                var loans = JsonSerializer.Deserialize<List<Loan>>(content);

                return loans ?? new List<Loan>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving loans in proxy: {ex.Message}");
                return new List<Loan>();
            }
        }

        // Get only unpaid loans for a user
        public async Task<List<Loan>> GetUnpaidUserLoans(int userId)
        {
            return (await GetUserLoans(userId)).Where(
                loan => loan.State == "unpaid").ToList();
        }

        // Create a new loan with the specified parameters
        public async Task<Loan?> TakeLoanAsync(int userId, decimal amount, string currency, string accountIBAN, int months)
        {
            try
            {
                var dto = new TakeLoanDTO
                {
                    UserId = userId,
                    Amount = amount,
                    Currency = currency,
                    AccountIBAN = accountIBAN,
                    Months = months,
                };
                var content = new StringContent(
                    JsonSerializer.Serialize(dto),
                    Encoding.UTF8,
                    "application/json");

                var response = await httpClient.PostAsync("https://localhost:7097/api/Loan/TakeLoan", content);
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                var loan = JsonSerializer.Deserialize<Loan>(json);
                return loan ?? null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error taking a loan in proxy: {ex.Message}");
                return null;
            }
        }

        // Process loan payment from specified bank account
        public async Task<string> PayLoanAsync(int userID, int loanId, string accountIBAN)
        {
            try
            {
                var dto = new PayLoanDTO
                {
                    UserId = userID,
                    LoanId = loanId,
                    AccountIBAN = accountIBAN,
                };
                var content = new StringContent(
                    JsonSerializer.Serialize(dto),
                    Encoding.UTF8,
                    "application/json");

                var response = await httpClient.PostAsync("https://localhost:7097/api/Loan/PayLoan", content);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<string>(json);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error paying a loan in proxy: {ex.Message}");
                return "Error paying a loan in proxy";
            }
        }

        // Validate loan request parameters
        public string ValidateLoanRequest(decimal amount, int months)
        {
            // Amount must be positive and not exceed one million
            if (amount <= 0 || amount > 1000000)
            {
                Debug.WriteLine($"Invalid loan amount: {amount}");
                return "Invalid loan amount";
            }

            // Months must be one of the allowed values
            var allowedMonths = new[] { 6, 12, 24, 36 };
            if (!allowedMonths.Contains(months))
            {
                Debug.WriteLine($"Invalid loan duration: {months}");
                return "Invalid loan duration";
            }

            return "success";
        }

        // Get formatted bank account strings for display
        public async Task<List<string>> GetFormattedBankAccounts(int userId)
        {
            try
            {
                var response = await httpClient.GetAsync($"https://localhost:7097/api/Loan/GetFormattedBankAccounts/{userId}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<List<string>>(content);
                    return result;
                }
                else
                {
                    throw new Exception($"Failed to get formatted bank accounts. Status Code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in proxy function: {ex.Message}");
                return new List<string>();
            }
        }

        public decimal CalculateTaxPercentage(int months)
        {
            // Simple calculation: 1% per month
            return months;
        }

        // Calculate the total amount to be repaid
        public decimal CalculateAmountToPay(decimal amount, decimal taxPercentage)
        {
            return amount * (1 + (taxPercentage / 100));
        }

        // Handle currency conversion
        public async Task<decimal> ConvertCurrency(decimal amount, string fromCurrency, string toCurrency)
        {
            try
            {
                var dto = new ConvertCurrencyDTO
                {
                    Amount = amount,
                    FromCurrency = fromCurrency,
                    ToCurrency = toCurrency,
                };

                var content = new StringContent(
                    JsonSerializer.Serialize(dto),
                    Encoding.UTF8,
                    "application/json");

                var response = await httpClient.PostAsync("https://localhost:7097/api/Loan/ConvertCurrency", content);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<decimal>(json);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error converting currency in proxy: {ex.Message}");
                throw new Exception($"Error converting currency in proxy: {ex.Message}");
            }
        }
    }

}

public class TakeLoanDTO
{
    public int UserId { get; set; }

    public decimal Amount { get; set; }

    public string Currency { get; set; }

    public string AccountIBAN { get; set; }

    public int Months { get; set; }
}

public class PayLoanDTO
{
    public int UserId { get; set; }

    public int LoanId { get; set; }

    public string AccountIBAN { get; set; }
}

public class ConvertCurrencyDTO
{
    public decimal Amount { get; set; }

    public string FromCurrency { get; set; }

    public string ToCurrency { get; set; }
}
