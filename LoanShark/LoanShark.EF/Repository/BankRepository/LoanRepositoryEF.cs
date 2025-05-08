using System.Data;
using System.Diagnostics;
using LoanShark.Domain;
using LoanShark.EF.EFModels;
using Microsoft.EntityFrameworkCore;

namespace LoanShark.EF.Repository.BankRepository
{
    public class LoanRepositoryEF : ILoanRepository
    {
        private readonly ILoanSharkDbContext dbContext;

        public LoanRepositoryEF(ILoanSharkDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Loan?> CreateLoan(Loan loan)
        {
            try
            {
                var loanEntity = new LoanEF
                {
                    UserID = loan.UserID,
                    Amount = loan.Amount,
                    Currency = loan.Currency,
                    DateDeadline = loan.DateDeadline,
                    TaxPercentage = loan.TaxPercentage,
                    NumberMonths = loan.NumberMonths,
                    State = "unpaid",
                };

                dbContext.Loan.Add(loanEntity);
                await dbContext.SaveChangesAsync();

                // Get the ID of the newly created loan
                loan.LoanID = loanEntity.LoanID;

                Debug.WriteLine($"REPO: Loan created: {loan.LoanID}, {loan}");

                return loan;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error creating loan: {ex.Message}");
                return null;
            }
        }

        // LOAN FUNCTIONS
        public async Task<List<Loan>> GetAllLoans()
        {
            try
            {
                List<Loan> loans = await dbContext.Loan
                    .Select(loan => new Loan(
                    loan.LoanID,
                    loan.UserID,
                    loan.Amount,
                    loan.Currency,
                    loan.DateTaken,
                    loan.DatePaid,
                    loan.TaxPercentage,
                    loan.NumberMonths,
                    loan.State)).ToListAsync();

                return loans;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error retrieving all loans: {ex.Message}");
                return new List<Loan>();
            }
        }

        public async Task<List<Loan>> GetLoansByUserId(int userId)
        {
            try
            {
                List<Loan> loans = await dbContext.Loan
                    .Where(loan => loan.UserID == userId)
                     .Select(loan => new Loan(
                     loan.LoanID,
                     loan.UserID,
                     loan.Amount,
                     loan.Currency,
                     loan.DateTaken,
                     loan.DatePaid,
                     loan.TaxPercentage,
                     loan.NumberMonths,
                     loan.State)).ToListAsync();

                return loans;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error retrieving loans for user {userId}: {ex.Message}");
                return new List<Loan>();
            }
        }

        public async Task<Loan?> GetLoanById(int loanId)
        {
            try
            {
                var loan = await dbContext.Loan.FirstOrDefaultAsync(loan => loan.LoanID == loanId);

                if (loan == null)
                {
                    return null;
                }

                return new Loan(loan.LoanID, loan.UserID, loan.Amount, loan.Currency, loan.DateTaken, loan.DatePaid, loan.TaxPercentage, loan.NumberMonths, loan.State);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error retrieving loan {loanId}: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> UpdateLoan(Loan loan)
        {
            try
            {
                var oldLoan = await dbContext.Loan.FindAsync(loan.LoanID);
                if (oldLoan == null)
                {
                    return false;
                }

                oldLoan.Amount = loan.Amount;
                oldLoan.Currency = loan.Currency;
                oldLoan.DateDeadline = loan.DateDeadline;
                oldLoan.DatePaid = loan.DatePaid;
                oldLoan.TaxPercentage = loan.TaxPercentage;
                oldLoan.State = loan.State;

                await dbContext.SaveChangesAsync();
                Debug.WriteLine("REPO: Loan updated", oldLoan.ToString());
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating loan {loan.LoanID}: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteLoan(int loanId)
        {
            try
            {
                var loan = await dbContext.Loan.FindAsync(loanId);
                if (loan == null)
                {
                    return false;
                }

                dbContext.Loan.Remove(loan);

                await dbContext.SaveChangesAsync();
                Debug.WriteLine("REPO: Loan deleted: ", loanId);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error deleting loan {loanId}: {ex.Message}");
                return false;
            }
        }

        // BANK ACCOUNT FUNCTIONS
        public async Task<List<BankAccount>> GetBankAccountsByUserId(int userId)
        {
            try
            {
                List<BankAccount> bankAccounts = await dbContext.BankAccount
                   .Where(bankAccount => bankAccount.UserID == userId)
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
            catch (Exception ex)
            {
                Debug.WriteLine($"Error retrieving bank accounts for user {userId}: {ex.Message}");
                return new List<BankAccount>();
            }
        }

        public async Task<BankAccount?> GetBankAccountByIBAN(string iban)
        {
            try
            {
                var entity = await dbContext.BankAccount
                   .FirstOrDefaultAsync(acc => acc.Iban == iban);

                if (entity == null)
                {
                    return null;
                }

                return new BankAccount(
                        entity.Iban,
                        entity.Currency,
                        entity.Balance,
                        entity.Blocked,
                        entity.UserID,
                        entity.Name,
                        entity.DailyLimit,
                        entity.MaximumPerTransaction,
                        entity.MaximumNrTransactions);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error retrieving bank account by IBAN {iban}: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> UpdateBankAccountBalance(string iban, decimal amount)
        {
            try
            {
                var bankAccount = await dbContext.BankAccount
                .FirstOrDefaultAsync(b => b.Iban == iban);

                if (bankAccount == null)
                {
                    throw new Exception($"Bank account with IBAN {iban} not found.");
                }

                bankAccount.Balance = amount;

                await dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating bank account {iban}: {ex.Message}");
                return false;
            }
        }

        // CURRENCY EXCHANGE FUNCTIONS
        public async Task<List<CurrencyExchange>> GetAllCurrencyExchanges()
        {
            try
            {
                List<CurrencyExchange> exchangeRates = await dbContext.CurrencyExchange
                   .Select(exchangeRate => new CurrencyExchange(
                       exchangeRate.FromCurrency,
                       exchangeRate.ToCurrency,
                       exchangeRate.ExchangeRate))
                   .ToListAsync();

                return exchangeRates;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error retrieving all currency exchanges: {ex.Message}");
                return new List<CurrencyExchange>();
            }
        }
    }
}
