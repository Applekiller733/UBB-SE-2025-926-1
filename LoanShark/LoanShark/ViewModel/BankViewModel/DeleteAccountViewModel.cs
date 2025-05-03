using System.Threading.Tasks;
using LoanShark.Service.BankService;

namespace LoanShark.ViewModel.BankViewModel
{
    public class DeleteAccountViewModel
    {
        private readonly UserService userService;

        public DeleteAccountViewModel()
        {
            userService = new UserService();
        }

        public async Task<string> DeleteAccount(string password)
        {
            return await userService.DeleteUser(password);
        }
    }
}
