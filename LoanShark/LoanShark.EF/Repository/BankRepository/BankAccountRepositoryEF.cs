using LoanShark.Data;
using LoanShark.Domain;
using LoanShark.EF.EFModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;

namespace LoanShark.EF.Repository.BankRepository
{
    

    public class BankAccountRepositoryEF : IBankAccountRepository
    {
        private readonly ILoanSharkDbContext _dbContext;

        public BankAccountRepositoryEF(ILoanSharkDbContext db)
        {
            this._dbContext = db ?? throw new ArgumentNullException(nameof(db));
        }

        /// <summary>
        /// Initializes a new instance of the BankAccountRepository class
        /// </summary>
        public BankAccountRepositoryEF()
        {
        }

        /// <summary>
        /// Retrieves all bank accounts from the database
        /// </summary>
        /// <returns>A list of all bank accounts, or null if an error occurs</returns>
        public async Task<List<BankAccount>?> GetAllBankAccounts()
        {
            try
            {
                //DataTable dataTable = await dataLink.ExecuteReader("GetAllBankAccounts");
                //return await ConvertDataTableToBankAccountList(dataTable);

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
            catch (Exception)
            {
                Debug.WriteLine("Oopsie on get all bank accounts repo");
                return null;
            }
        }

        /// <summary>
        /// Retrieves all bank accounts for a specific user
        /// </summary>
        /// <param name="userID">The ID of the user</param>
        /// <returns>A list of bank accounts belonging to the user, or null if an error occurs</returns>
        public async Task<List<BankAccount>?> GetBankAccountsByUserId(int userID)
        {
            try
            {
                var sqlParams = new SqlParameter[]
                {
                    new SqlParameter("@id_user", userID)
                };
                //DataTable dataTable = await dataLink.ExecuteReader("GetBankAccountsByUser", sqlParams);
                //return await ConvertDataTableToBankAccountList(dataTable);

                var bankAcc = await _dbContext.BankAccount
                .Where(b => b.UserID == userID)
                .Select(b => new BankAccount(
                    b.Iban,
                    b.Currency,
                    b.Balance,
                    b.Blocked,
                    b.UserID,
                    b.Name,
                    b.DailyLimit,
                    b.MaximumPerTransaction,
                    b.MaximumNrTransactions
                ))
                .ToListAsync();

                return bankAcc;
            }
            catch (Exception)
            {
                Debug.WriteLine("Oopsie on get bank accounts by user id repo");
                return null;
            }
        }

        /// <summary>
        /// Retrieves a bank account by its IBAN
        /// </summary>
        /// <param name="iban">The IBAN of the bank account to retrieve</param>
        /// <returns>The bank account with the specified IBAN, or null if not found or an error occurs</returns>
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
            catch (Exception)
            {
                Debug.WriteLine("Oopsie on get bank account by IBAN repo");
                return null;
            }
        }

        /// <summary>
        /// Adds a new bank account to the database
        /// </summary>
        /// <param name="bankAccount">The bank account to add</param>
        /// <returns>True if the bank account was added successfully, false otherwise</returns>
        public async Task<bool> AddBankAccount(BankAccount bankAccount)
        {
            if(bankAccount == null)
            {
                throw new ArgumentNullException(nameof(bankAccount), "Bank Account cannot be null");
            }
            try
            {
                var baccEntity = new BankAccountEF
                {
                    Iban = bankAccount.Iban,
                    Currency = bankAccount.Currency,
                    Balance = bankAccount.Balance,
                    Blocked = bankAccount.Blocked,
                    UserID = bankAccount.UserID,
                    Name = bankAccount.Name,
                    DailyLimit = bankAccount.DailyLimit,
                    MaximumPerTransaction = bankAccount.MaximumPerTransaction,
                    MaximumNrTransactions = bankAccount.MaximumNrTransactions
                };
                _dbContext.BankAccount.Add(baccEntity);
                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                Debug.WriteLine("Oopsie on add bank account repo");
                return false;
            }
        }

        /// <summary>
        /// Removes a bank account from the database
        /// </summary>
        /// <param name="iban">The IBAN of the bank account to remove</param>
        /// <returns>True if the bank account was removed successfully, false otherwise</returns>
        public async Task<bool> RemoveBankAccount(string iban)
        {
            try
            {
                var entity = await _dbContext.BankAccount
                    .FirstOrDefaultAsync(acc => acc.Iban == iban);

                if (entity == null)
                {
                    throw new ArgumentNullException(nameof(entity));
                }

                _dbContext.BankAccount.Remove(entity);
                _dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                Debug.WriteLine("Oopsie on remove bank account repo");
                return false;
            }
        }

        /// <summary>
        /// Converts a DataRow to a BankAccount object
        /// </summary>
        /// <param name="row">The DataRow to convert</param>
        /// <returns>A BankAccount object with data from the row</returns>
        public BankAccount ConvertDataTableRowToBankAccount(DataRow row)
        {
            return new BankAccount(
                         Convert.ToString(row["iban"]) ?? string.Empty,
                         Convert.ToString(row["currency"]) ?? string.Empty,
                         decimal.Parse(row["amount"].ToString() ?? "0"),
                         Convert.ToBoolean(row["blocked"]),
                         Convert.ToInt32(row["id_user"]),
                         Convert.ToString(row["custom_name"]) ?? string.Empty,
                         decimal.Parse(row["daily_limit"].ToString() ?? "0"),
                         decimal.Parse(row["max_per_transaction"].ToString() ?? "0"),
                         Convert.ToInt32(row["max_nr_transactions_daily"]));
        }

        /// <summary>
        /// Converts a DataTable to a list of BankAccount objects
        /// </summary>
        /// <param name="dataTable">The DataTable to convert</param>
        /// <returns>A list of BankAccount objects</returns>
        public Task<List<BankAccount>> ConvertDataTableToBankAccountList(DataTable dataTable)
        {
            List<BankAccount> bankAccounts = new List<BankAccount>();
            foreach (DataRow row in dataTable.Rows)
            {
                bankAccounts.Add(ConvertDataTableRowToBankAccount(row));
            }
            return Task.FromResult(bankAccounts);
        }

        /// <summary>
        /// Converts a DataTable to a list of currency strings
        /// </summary>
        /// <param name="dataTable">The DataTable to convert</param>
        /// <returns>A list of currency names as strings</returns>
        public Task<List<string>> ConvertDataTableToCurrencyList(DataTable dataTable)
        {
            List<string> currencies = new List<string>();
            foreach (DataRow row in dataTable.Rows)
            {
                currencies.Add(Convert.ToString(row["currency_name"]) ?? string.Empty);
            }
            return Task.FromResult(currencies);
        }

        /// <summary>
        /// Retrieves all available currencies from the database
        /// </summary>
        /// <returns>A list of currency names as strings</returns>
        public async Task<List<string>> GetCurrencies()
        {
            return await _dbContext.Currency
                          .Select(c => c.CurrencyName)
                          .Distinct()
                          .ToListAsync();
        }

        /// <summary>
        /// Retrieves user credentials for the specified email
        /// </summary>
        /// <param name="email">The email of the user</param>
        /// <returns>A list containing the user's hashed password and salt</returns>
        public async Task<List<string>> GetCredentials(string email)
        {
            var user = await _dbContext.User
                    .Where(u => u.Email == email)
                    .Select(u => new { u.HashedPassword, u.PasswordSalt })
                    .FirstOrDefaultAsync();

            if (user == null)
            {
                return new List<string> { string.Empty, string.Empty };
            }

            return new List<string>
            {
                user.HashedPassword ?? string.Empty,
                user.PasswordSalt ?? string.Empty
            };
        }

        // updates the bank account with the given iban with the new attributes by calling
        // the sql procedure UpdateBankAccount
        public async Task<bool> UpdateBankAccount(string iban, BankAccount nba)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(iban))
                {
                    throw new ArgumentException("IBAN must be provided.", nameof(iban));
                }
                var bankAccount = await _dbContext.BankAccount
                    .FirstOrDefaultAsync(b => b.Iban == iban);

                if (bankAccount == null)
                {
                    throw new Exception($"Bank account with IBAN {iban} not found.");
                }

                bankAccount.Iban = iban;
                //bankAccount.Currency = nba.Currency;
                //bankAccount.Balance = nba.Balance;
                bankAccount.Blocked = nba.Blocked;
                //bankAccount.UserID = nba.UserID;
                bankAccount.Name = nba.Name;
                bankAccount.DailyLimit = nba.DailyLimit;
                bankAccount.MaximumNrTransactions = nba.MaximumNrTransactions;
                bankAccount.MaximumPerTransaction = nba.MaximumPerTransaction;

                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                Debug.WriteLine("Could not update bank account");
                return false;
            }
        }
    }
}
