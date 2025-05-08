using LoanShark.Data;
using LoanShark.EF.EFModels;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LoanShark.EF.Repository.BankRepository
{
    public class MainPageRepositoryEF : IMainPageRepository
    {
        private readonly ILoanSharkDbContext _dbContext;

        public MainPageRepositoryEF(ILoanSharkDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }


        public async Task<List<BankAccountEF>> GetUserBankAccounts(int id_user)
        {
            if (id_user <= 0)
            {
                return new List<BankAccountEF>();
            }

            try
            {
                return await _dbContext.BankAccount
                    .Where(b => b.UserID == id_user)
                    .ToListAsync();
            }
            catch (Exception)
            {
                return new List<BankAccountEF>();
            }
        }


        public async Task<Tuple<decimal, string>> GetBankAccountBalanceByUserIban(string iban)
        {
            if (string.IsNullOrWhiteSpace(iban))
            {
                return new Tuple<decimal, string>(0m, string.Empty);
            }

            try
            {
                var account = await _dbContext.BankAccount
                    .FirstOrDefaultAsync(b => b.Iban == iban);

                if (account == null)
                {
                    return new Tuple<decimal, string>(0m, string.Empty);
                }

                decimal amount = account.Balance;
                string currency = account.Currency ?? string.Empty;

                return new Tuple<decimal, string>(amount, currency);
            }
            catch (Exception)
            {
                return new Tuple<decimal, string>(0m, string.Empty);
            }
        }

    }
}
