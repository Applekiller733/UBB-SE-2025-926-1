using Microsoft.AspNetCore.Mvc;
using LoanShark.Domain;
using LoanShark.API.Proxies;
using LoanShark.Service.BankService;
using LoanShark.MVC.Models;

namespace LoanShark.MVC.Controllers
{
    public class BankAccountDetailsController : Controller
    {
        private readonly IBankAccountService _bankAccountService;

        public BankAccountDetailsController(IBankAccountService bankAccountService)
        {
            _bankAccountService = bankAccountService;
        }

        [HttpGet]
        public async Task<IActionResult> Details(string iban)
        {
            if (string.IsNullOrEmpty(iban))
                return RedirectToAction("Index", "MainPage");

            var bankAccount = await _bankAccountService.FindBankAccount(iban);

            if (bankAccount == null)
                return RedirectToAction("Index", "MainPage");

            var model = new BankAccountDetailsModel
            {
                BankAccount = bankAccount
            };

            return View(model);
        }
    }
}