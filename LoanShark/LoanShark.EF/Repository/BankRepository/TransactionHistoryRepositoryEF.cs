using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using LoanShark.Domain;
using LoanShark.EF.EFModels;
using Microsoft.EntityFrameworkCore;

namespace LoanShark.EF.Repository.BankRepository
{
    public class TransactionHistoryRepositoryEF : ITransactionHistoryRepository
    {
        private readonly ILoanSharkDbContext _dbContext;

        public TransactionHistoryRepositoryEF(ILoanSharkDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<ObservableCollection<Transaction>> GetTransactionsNormal()
        {
            try
            {
                string currentIban = UserSession.Instance.GetUserData("current_bank_account_iban") ?? string.Empty;

                var transactions = await _dbContext.Transaction
                    .Where(t => t.SenderIban == currentIban || t.ReceiverIban == currentIban)
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

                return new ObservableCollection<Transaction>(transactions);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving transactions: {ex.Message}", ex);
            }
        }

        public async Task<ObservableCollection<string>> GetTransactionsForMenu()
        {
            try
            {
                string currentIban = UserSession.Instance.GetUserData("current_bank_account_iban") ?? string.Empty;

                var transactions = await _dbContext.Transaction
                    .Where(t => t.SenderIban == currentIban || t.ReceiverIban == currentIban)
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

                var menuStrings = transactions.Select(t => t.TostringForMenu());
                return new ObservableCollection<string>(menuStrings);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving transactions for menu: {ex.Message}", ex);
            }
        }

        public async Task<ObservableCollection<string>> GetTransactionsDetailed()
        {
            try
            {
                string currentIban = UserSession.Instance.GetUserData("current_bank_account_iban") ?? string.Empty;

                var transactions = await _dbContext.Transaction
                    .Where(t => t.SenderIban == currentIban || t.ReceiverIban == currentIban)
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

                var detailedStrings = transactions.Select(t => t.TostringDetailed());
                return new ObservableCollection<string>(detailedStrings);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving detailed transactions: {ex.Message}", ex);
            }
        }

        public async Task UpdateTransactionDescription(int transactionId, string newDescription)
        {
            try
            {
                var transaction = await _dbContext.Transaction
                    .FirstOrDefaultAsync(t => t.TransactionId == transactionId);

                if (transaction == null)
                {
                    throw new Exception($"Transaction with ID {transactionId} not found.");
                }

                transaction.TransactionDescription = newDescription;
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating transaction description: {ex.Message}", ex);
            }
        }
    }
} 