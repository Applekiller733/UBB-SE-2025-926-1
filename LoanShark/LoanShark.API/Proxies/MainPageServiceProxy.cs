using System.Collections.ObjectModel;
using System.Diagnostics;
using LoanShark.Domain;
using LoanShark.Service.Service.BankService;

namespace LoanShark.API.Proxies
{
    public class MainPageServiceProxy : IMainPageService
    {
        private readonly HttpClient httpClient;

        public MainPageServiceProxy(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }


        public async Task<ObservableCollection<BankAccount>> GetUserBankAccounts(int userId)
        {
            try
            {
                var response = await this.httpClient.GetAsync($"https://localhost:7097/api/MainPage/GetUserBankAccounts/{userId}");

                response.EnsureSuccessStatusCode();

                var dtoList = await response.Content.ReadFromJsonAsync<List<BankAccountDto>>();

                var observableList = new ObservableCollection<BankAccount>();

                foreach (var dto in dtoList)
                {
                    string customName = !string.IsNullOrWhiteSpace(dto.Name)
                        ? dto.Name
                        : $"Account {observableList.Count + 1}";

                    observableList.Add(new BankAccount(
                        dto.Iban ?? string.Empty,
                        dto.Currency ?? string.Empty,
                        dto.Balance,
                        dto.Blocked,
                        dto.UserID,
                        customName,
                        dto.DailyLimit,
                        dto.MaximumPerTransaction,
                        dto.MaximumNrTransactions
                    ));
                }

                return observableList;
            }
            catch (Exception ex)
            {
                throw new Exception($"Proxy error getting bank accounts for MainPage: {ex.Message}");
            }
        }

        public async Task<Tuple<decimal, string>> GetBankAccountBalanceByUserIban(string iban)
        {
            if (string.IsNullOrWhiteSpace(iban))
                return new Tuple<decimal, string>(0m, string.Empty);

            try
            {
                var response = await this.httpClient.GetAsync($"https://localhost:7097/api/MainPage/GetBankAccountBalanceByUserIban/{iban}");
                if (!response.IsSuccessStatusCode)
                    return new Tuple<decimal, string>(0m, string.Empty);

                var tuple = await response.Content.ReadFromJsonAsync<Tuple<decimal, string>>();
                return tuple ?? new Tuple<decimal, string>(0m, string.Empty);
            }
            catch (Exception ex)
            {
                Debug.Print($"Proxy error fetching balance for MainPage: {ex.Message}");
                return new Tuple<decimal, string>(0m, string.Empty);
            }
        }
    }

    public class BankAccountDto
    {
        public string Iban { get; set; }

        public string Currency { get; set; }

        public decimal Balance { get; set; }

        public bool Blocked { get; set; }

        public int UserID { get; set; }

        public string Name { get; set; }

        public decimal DailyLimit { get; set; }

        public decimal MaximumPerTransaction { get; set; }

        public int MaximumNrTransactions { get; set; }
    }

    public class BalanceDto
    {
        public decimal Balance { get; set; }

        public string Currency { get; set; }
    }
}
