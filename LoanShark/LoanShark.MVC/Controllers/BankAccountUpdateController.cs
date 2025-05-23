using Microsoft.AspNetCore.Mvc;
using LoanShark.Domain;
using LoanShark.Service.BankService;
using System.Threading.Tasks;
using LoanShark.MVC.Models;

namespace LoanShark.Controllers
{
    public class BankAccountUpdateController : Controller
    {
        private readonly IBankAccountService _bankAccountService;

        public BankAccountUpdateController(IBankAccountService bankAccountService)
        {
            _bankAccountService = bankAccountService;
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string iban)
        {
            if (string.IsNullOrEmpty(iban))
                return NotFound();

            var account = await _bankAccountService.FindBankAccount(iban);
            if (account == null)
                return NotFound();

            var model = new BankAccountEditModel
            {
                Iban = account.Iban,
                Name = account.Name,
                DailyLimit = account.DailyLimit,
                MaximumPerTransaction = account.MaximumPerTransaction,
                MaximumNrTransactions = account.MaximumNrTransactions,
                IsBlocked = account.Blocked
            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(BankAccountEditModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _bankAccountService.UpdateBankAccount(
                model.Iban,
                model.Name,
                (decimal)model.DailyLimit,
                (decimal)model.MaximumPerTransaction,
                model.MaximumNrTransactions,
                model.IsBlocked
            );

            if (result)
            {
                TempData["SuccessMessage"] = "Account updated successfully.";
                return RedirectToAction("Index", "BankAccountList");
            }

            ModelState.AddModelError("", "Failed to update the account.");
            return View(model);
        }
    }
}
