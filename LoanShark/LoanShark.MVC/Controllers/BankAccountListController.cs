using Microsoft.AspNetCore.Mvc;
using LoanShark.Domain;
using LoanShark.Service.BankService;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace LoanShark.MVC.Controllers
{
    public class BankAccountListController : Controller
    {
        private readonly IBankAccountService _bankAccountService;

        public BankAccountListController(IBankAccountService bankAccountService)
        {
            _bankAccountService = bankAccountService;
        }


        public async Task<IActionResult> Index()
        {
            int userId = GetCurrentUserId();
            var accounts = await _bankAccountService.GetUserBankAccounts(userId);
            return this.View(accounts);
        }

        public async Task<IActionResult> Details(string iban)
        {
            if (string.IsNullOrWhiteSpace(iban))
                return NotFound();

            int userId = GetCurrentUserId();
            var accounts = await _bankAccountService.GetUserBankAccounts(userId);
            var selectedAccount = accounts.FirstOrDefault(a => a.Iban == iban);

            if (selectedAccount == null)
                return NotFound();

            return this.View(selectedAccount);
        }

        private int GetCurrentUserId()
        {
            return int.TryParse(HttpContext.Session.GetString("id_user"), out int id) ? id : 0;
        }
    }
}