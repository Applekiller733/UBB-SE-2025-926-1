using LoanShark.Data;
using LoanShark.Domain;
using LoanShark.EF.EFModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanShark.EF.Repository.BankRepository
{
    public class TransactionsRepositoryEF : ITransactionsRepository
    {
        private readonly ILoanSharkDbContext _dbContext;

        public TransactionsRepositoryEF(ILoanSharkDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<bool> AddTransaction(Transaction transaction)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException(nameof(transaction), "Transaction cannot be null");
            }
            try
            {
                var transactionEntity = new TransactionEF
                {
                    SenderIban = transaction.SenderIban,
                    ReceiverIban = transaction.ReceiverIban,
                    SenderCurrency = transaction.SenderCurrency,
                    ReceiverCurrency = transaction.ReceiverCurrency,
                    SenderAmount = transaction.SenderAmount,
                    ReceiverAmount = transaction.ReceiverAmount,
                    TransactionType = transaction.TransactionType,
                    TransactionDescription = string.IsNullOrWhiteSpace(transaction.TransactionDescription) ? null : transaction.TransactionDescription,
                    TransactionDatetime = DateTime.UtcNow
                };

                _dbContext.Transaction.Add(transactionEntity);
                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch (SqlException ex)
            {
                throw new Exception($"Database error in AddTransaction: {ex.Message}", ex);
            }
        }

        public async Task<List<BankAccount>> GetAllBankAccounts()
        {
            try
            {
                List<BankAccount> bankAccounts = await _dbContext.BankAccount
                    .Select(bankAccount => new BankAccount(
                        bankAccount.Iban,
                        bankAccount.Currency,
                        bankAccount.Balance,
                        bankAccount.Blocked,
                        bankAccount.UserID,
                        bankAccount.Name,
                        bankAccount.DailyLimit,
                        bankAccount.MaximumPerTransaction,
                        bankAccount.MaximumNrTransactions
                    ))
                    .ToListAsync();
                return bankAccounts;
            }
            catch (SqlException ex)
            {
                throw new Exception($"Database error in GetAllBankAccounts: {ex.Message}", ex);
            }
        }

        public async Task<List<CurrencyExchange>> GetAllCurrencyExchangeRates()
        {
            try
            {
                List<CurrencyExchange> exchangeRates = await _dbContext.CurrencyExchange
                    .Select(exchangeRate => new CurrencyExchange(
                        exchangeRate.FromCurrency,
                        exchangeRate.ToCurrency,
                        exchangeRate.ExchangeRate))
                    .ToListAsync();
                return exchangeRates;
            }
            catch (SqlException ex)
            {
                throw new Exception($"Database error in GetAllCurrencyExchangeRates: {ex.Message}", ex);
            }
        }

        public async Task<BankAccount?> GetBankAccountByIBAN(string iban)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(iban))
                {
                    throw new ArgumentException("IBAN cannot be empty.", nameof(iban));
                }

                var entity = await _dbContext.BankAccount
                    .FirstOrDefaultAsync(acc => acc.Iban == iban);

                if (entity == null)
                    return null;

                return new BankAccount(
                        entity.Iban,
                        entity.Currency,
                        entity.Balance,
                        entity.Blocked,
                        entity.UserID,
                        entity.Name,
                        entity.DailyLimit,
                        entity.MaximumPerTransaction,
                        entity.MaximumNrTransactions
                );
            }
            catch (Exception ex)
            {
                throw new Exception($"ORM error in GetBankAccountByIBAN: {ex.Message}", ex);
            }
        }

        public async Task<List<Transaction>> GetBankAccountTransactions(string iban)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(iban))
                {
                    throw new ArgumentException("IBAN cannot be empty.", nameof(iban));
                }

                var transactions = await _dbContext.Transaction
                .Where(t => t.SenderIban == iban || t.ReceiverIban == iban)
                .Select(t => new Transaction(
                    t.TransactionId,
                    t.SenderIban,
                    t.ReceiverIban,
                    t.TransactionDatetime,
                    t.SenderCurrency,
                    t.ReceiverCurrency,
                    t.SenderAmount,
                    t.ReceiverAmount,
                    t.TransactionType,
                    t.TransactionDescription ?? string.Empty
                ))
                .ToListAsync();

                return transactions;
            }
            catch (Exception ex)
            {
                throw new Exception($"ORM error in GetBankAccountTransactions: {ex.Message}", ex);
            }
        }

        public async Task<decimal> GetExchangeRate(string fromCurrency, string toCurrency)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fromCurrency) || string.IsNullOrWhiteSpace(toCurrency))
                {
                    throw new ArgumentException("Currency codes cannot be null or empty.");
                }

                var exchangeRate = await _dbContext.CurrencyExchange
                    .Where(c => c.FromCurrency == fromCurrency && c.ToCurrency == toCurrency)
                    .Select(c => c.ExchangeRate)
                    .FirstOrDefaultAsync();

                if (exchangeRate == 0)
                {
                    throw new Exception("Exchange rate not found for the provided currencies.");
                }

                return exchangeRate;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving exchange rate: {ex.Message}", ex);
            }
        }

        public async Task<bool> UpdateBankAccountBalance(string iban, decimal newBalance)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(iban))
                {
                    throw new ArgumentException("IBAN must be provided.", nameof(iban));
                }

                if (newBalance < 0)
                {
                    throw new ArgumentException("Balance cannot be negative.");
                }

                var bankAccount = await _dbContext.BankAccount
                    .FirstOrDefaultAsync(b => b.Iban == iban);

                if (bankAccount == null)
                {
                    throw new Exception($"Bank account with IBAN {iban} not found.");
                }

                bankAccount.Balance = newBalance;

                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating bank account balance: {ex.Message}", ex);
            }
        }
    }
}
