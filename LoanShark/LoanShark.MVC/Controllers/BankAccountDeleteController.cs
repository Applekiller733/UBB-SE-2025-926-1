using Microsoft.AspNetCore.Mvc;
using LoanShark.Domain;
using LoanShark.API.Proxies;
using LoanShark.Service.BankService;
using LoanShark.MVC.Models;

namespace LoanShark.MVC.Controllers
{
    public class BankAccountDeleteController : Controller
    {
        private readonly IBankAccountService _bankAccountService;

        public BankAccountDeleteController(IBankAccountService bankAccountService)
        {
            _bankAccountService = bankAccountService;
        }

        [HttpGet]
        public IActionResult Delete(string iban)
        {
            if (string.IsNullOrEmpty(iban))
            {
                return RedirectToAction("Index","MainPage");
            }

            var model = new BankAccountDeleteModel { IBAN = iban };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(string iban)
        {
            if (!string.IsNullOrEmpty(iban))
            {
                var success = await _bankAccountService.RemoveBankAccount(iban);
                if (success)
                {
                    TempData["Success"] = "Bank account deleted successfully.";
                }
                else
                {
                    TempData["Error"] = "Failed to delete the bank account.";
                }
            }
            return RedirectToAction("Index","MainPage");
        }
    }
}