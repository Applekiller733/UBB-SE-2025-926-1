using System.Threading.Tasks;
using LoanShark.API.Proxies;
using LoanShark.Service.BankService;

namespace LoanShark.ViewModel.BankViewModel
{
    public class DeleteAccountViewModel
    {
        private IUserService? userService = null;

        public DeleteAccountViewModel(IUserService sv)
        {
            userService = sv;
        }
        public async Task<string> DeleteAccount(string password)
        {
            return await userService.DeleteUser(password);
        }
    }
}
