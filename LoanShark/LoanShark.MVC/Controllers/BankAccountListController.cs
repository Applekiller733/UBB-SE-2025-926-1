using Microsoft.AspNetCore.Mvc;
using LoanShark.Domain;
using LoanShark.Service.BankService;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using LoanShark.MVC.Models;

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
        [HttpGet]
        public async Task<IActionResult> Details(string iban)
        {
            if (string.IsNullOrWhiteSpace(iban))
                return NotFound();

            int userId = GetCurrentUserId();
            var account = await _bankAccountService.FindBankAccount(iban);

            if (account == null)
                return NotFound();

            var currentIban = HttpContext.Session.GetString("current_bank_account_iban");

            if (!string.IsNullOrEmpty(currentIban) && currentIban == iban)
            {
                var model = new BankAccountDetailsModel
                {
                    BankAccount = account
                };

                return RedirectToAction("Edit", new { iban = model.BankAccount.Iban });

            }

            var readOnlyModel = new BankAccountDetailsModel
            {
                BankAccount = account
            };

            return View("~/Views/BankAccountDetails/Index.cshtml", readOnlyModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string iban)
        {
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
                Currency = account.Currency,
                IsBlocked = account.Blocked
            };

            return View(model);
        }




        [HttpPost]
        public async Task<IActionResult> Edit(BankAccountEditModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _bankAccountService.UpdateBankAccount(
                model.Iban,
                model.Name,
                model.DailyLimit,
                model.MaximumPerTransaction,
                model.MaximumNrTransactions,
                model.IsBlocked
            );
            TempData["SuccessMessage"] = "Account updated successfully.";
            return RedirectToAction("Details", new { iban = model.Iban });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string iban)
        {
            await _bankAccountService.RemoveBankAccount(iban);
            return RedirectToAction("Index");
            return this.View(selectedAccount);

        }

        [HttpPost]
        public IActionResult SetCurrentBankAccount(string iban)
        {
            HttpContext.Session.SetString("current_bank_account_iban", iban);
            return RedirectToAction("Edit", "BankAccountUpdate");
        }


        private int GetCurrentUserId()
        {
            return int.TryParse(HttpContext.Session.GetString("userId"), out int id) ? id : 0;
        }
    }
}