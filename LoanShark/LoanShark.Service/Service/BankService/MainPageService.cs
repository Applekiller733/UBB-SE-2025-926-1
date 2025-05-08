using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Threading.Tasks;
using LoanShark.Domain;
using LoanShark.EF.EFModels;
using LoanShark.EF.Repository.BankRepository;

namespace LoanShark.Service.Service.BankService
{
    public class MainPageService : IMainPageService
    {
        private readonly IMainPageRepository repo;

        public MainPageService(IMainPageRepository repository)
        {
            this.repo = repository;
        }

        public async Task<ObservableCollection<BankAccount>> GetUserBankAccounts(int userId)
        {
            try
            {
                ObservableCollection<BankAccount> bankAccounts = new ObservableCollection<BankAccount>();
                List<BankAccountEF> bankAccountsData = await this.repo.GetUserBankAccounts(userId);

                foreach (BankAccountEF account in bankAccountsData)
                {
                    string iban = account.Iban?.ToString() ?? string.Empty;
                    string currency = account.Currency?.ToString() ?? string.Empty;
                    decimal amount = account.Balance != 0 ? account.Balance : 0;
                    string customName = account.Name?.ToString() ?? $"Account {bankAccounts.Count + 1}";
                    decimal dailyLimit = account.DailyLimit != 0 ? account.DailyLimit : 0;
                    decimal maxPerTransaction = account.MaximumPerTransaction != 0
                        ? account.MaximumPerTransaction
                        : 0;
                    int maxNrTransactionsDaily = account.MaximumNrTransactions != 0
                        ? account.MaximumNrTransactions
                        : 0;
                    bool blocked = account.Blocked != false ? account.Blocked : false;
                    bankAccounts.Add(new BankAccount(iban, currency, amount, blocked, userId, customName, dailyLimit,
                        maxPerTransaction, maxNrTransactionsDaily));
                }

                return bankAccounts;
            }
            catch (Exception ex)
            {
                Debug.Print($"Service error getting bank accounts: {ex.Message}");
                throw;
            }
        }

        public async Task<Tuple<decimal, string>> GetBankAccountBalanceByUserIban(string iban)
        {
            return await this.repo.GetBankAccountBalanceByUserIban(iban);
        }
    }

    public interface IMainPageService
    {
        Task<ObservableCollection<BankAccount>> GetUserBankAccounts(int userId);
        Task<Tuple<decimal, string>> GetBankAccountBalanceByUserIban(string iban);
    }
}
