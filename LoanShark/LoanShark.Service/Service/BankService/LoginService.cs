using System;
using System.Data;
using System.Diagnostics;
using System.Threading.Tasks;
using LoanShark.Domain;
using LoanShark.EF.EFModels;
using LoanShark.EF.Repository.BankRepository;
namespace LoanShark.Service.Service.BankService
{
    public class LoginService : ILoginService
    {
        private readonly ILoginRepository loginRepository;

        public LoginService(ILoginRepository loginRepository)
        {
            this.loginRepository = loginRepository;
        }

        public async Task<bool> ValidateUserCredentials(string email, string password)
        {
            try
            {
                UserEF? dt = await this.loginRepository.GetUserCredentials(email);

                // if exception is not thrown, then the user exists and we continue with the validation
                string hashedPassword = dt.HashedPassword;
                string passwordSalt = dt.PasswordSalt;

                HashedPassword userPassword = new HashedPassword(hashedPassword, passwordSalt, false);
                HashedPassword inputPassword = new HashedPassword(password, passwordSalt, true);

                return userPassword.Equals(inputPassword);
            }
            catch (Exception ex)
            {
                Debug.Print($"Error validating user credentials: {ex.Message}");
                return false;
            }
        }

        public async Task InstantiateUserSessionAfterLogin(string email)
        {
            UserEF dt_user_info = await this.loginRepository.GetUserInfoAfterLogin(email);
            List<BankAccountEF> dt_bank_accounts = await this.loginRepository.GetUserBankAccounts(int.Parse(dt_user_info.UserID.ToString() ?? string.Empty));
            string iban = string.Empty;

            if (dt_bank_accounts.Count > 0)
            {
                iban = dt_bank_accounts[0].Iban.ToString() ?? string.Empty;
            }

            UserSession.Instance.Initialize(
                dt_user_info.UserID.ToString() ?? string.Empty,
                dt_user_info.Cnp?.ToString() ?? string.Empty,
                dt_user_info.FirstName?.ToString() ?? string.Empty,
                dt_user_info.LastName?.ToString() ?? string.Empty,
                dt_user_info.Email?.ToString() ?? string.Empty,
                dt_user_info.PhoneNumber?.ToString() ?? string.Empty,
                iban);
        }

        public Task<UserEF> GetUserInfoAfterLogin(string email)
        {
            return loginRepository.GetUserInfoAfterLogin(email);
        }

        public Task<List<BankAccountEF>> GetUserBankAccounts(int userId)
        {
            return loginRepository.GetUserBankAccounts(userId);
        }
    }

    public interface ILoginService
    {
        Task<bool> ValidateUserCredentials(string email, string password);
        Task InstantiateUserSessionAfterLogin(string email);

        Task<UserEF> GetUserInfoAfterLogin(string email);
        Task<List<BankAccountEF>> GetUserBankAccounts(int userId);
    }
}

