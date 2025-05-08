using LoanShark.Data;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoanShark.Domain;
using LoanShark.EF.EFModels;
using Microsoft.EntityFrameworkCore;

namespace LoanShark.EF.Repository.BankRepository
{
    public class LoginRepositoryEF : ILoginRepository
    {
        private readonly ILoanSharkDbContext dbContext;

        public LoginRepositoryEF(ILoanSharkDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }


        // Returns a DataTable with the user credentials, they will be accessible from dt.Rows[0]["hashed_password"] and dt.Rows[0]["passowrd_salt"]
        // If the user with the given email is not found, an exception will be thrown
        public async Task<UserEF> GetUserCredentials(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email cannot be null or empty", nameof(email));
            }

            try
            {
                var user = await this.dbContext.User
                    .FirstOrDefaultAsync(u => u.Email == email);

                if (user == null)
                {
                    throw new Exception($"User with the email {email} does NOT exist.");
                }

                return user;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting user credentials: {ex.Message}", ex);
            }
        }

        public async Task<UserEF> GetUserInfoAfterLogin(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email cannot be null or empty", nameof(email));
            }

            try
            {
                var user = await this.dbContext.User.FirstOrDefaultAsync(u => u.Email == email);

                if (user == null)
                {
                    throw new Exception($"User with the email {email} does NOT exist.");
                }

                return user;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting user info: {ex.Message}", ex);
            }
        }


        public async Task<List<BankAccountEF>> GetUserBankAccounts(int id_user)
        {
            if (id_user <= 0)
            {
                throw new ArgumentException("Invalid user ID", nameof(id_user));
            }

            try
            {
                var accounts = await this.dbContext.BankAccount
                    .Where(b => b.UserID == id_user)
                    .ToListAsync();

                return accounts;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting user bank accounts: {ex.Message}", ex);
            }
        }

    }
}
