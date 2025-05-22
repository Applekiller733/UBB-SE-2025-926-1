using LoanShark.Domain;
using LoanShark.Service.Service.BankService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using LoanShark.MVC.Models;

namespace LoanShark.MVC.Controllers
{
    public class MainPageController : Controller
    {
        private readonly IMainPageService _mainPageService;

        public MainPageController(IMainPageService mainPageService)
        {
            _mainPageService = mainPageService;
        }

        [HttpPost]
        public IActionResult SelectBankAccount(string iban)
        {
            if (!string.IsNullOrEmpty(iban))
            {
                HttpContext.Session.SetString("current_bank_account_iban", iban);
            }

            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("userId");
            if (userId == null)
                return RedirectToAction("Index", "Account");

            var accounts = await _mainPageService.GetUserBankAccounts(userId.Value);
            var vm = new MainPageViewModel
            {
                WelcomeText = $"Welcome, {HttpContext.Session.GetString("first_name")}!",
                BankAccounts = new List<BankAccount>(accounts),
                BalanceButtonContent = TempData["BalanceButtonContent"]?.ToString() ?? "Check Balance",
                SelectedAccountIban = TempData["SelectedAccountIban"]?.ToString()
            };


            return View("Index", vm);
        }

        [HttpPost]
        public async Task<IActionResult> CheckBalance(string iban)
        {
            if (string.IsNullOrEmpty(iban))
            {
                TempData["BalanceMessage"] = "No account selected.";
                return RedirectToAction("Index");
            }

            HttpContext.Session.SetString("current_bank_account_iban", iban);

            var result = await _mainPageService.GetBankAccountBalanceByUserIban(iban);
            TempData["BalanceButtonContent"] = $"{result.Item1:N2} {result.Item2}";
            TempData["SelectedAccountIban"] = iban;


            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult BankAccountDetails()
        {
            var iban = HttpContext.Session.GetString("current_bank_account_iban");
            if (string.IsNullOrEmpty(iban))
            {
                TempData["BalanceMessage"] = "No bank account selected.";
                return RedirectToAction("Index");
            }

            // No route values passed
            return RedirectToAction("Details", "BankAccountDetails");
        }



        [HttpPost]
        public IActionResult Transaction()
        {
            var iban = HttpContext.Session.GetString("current_bank_account_iban");
            if (string.IsNullOrEmpty(iban))
                return RedirectToAction("Index");

            return RedirectToAction("Index", "Transaction"); // Must have TransactionController
        }

        [HttpPost]
        public IActionResult TransactionHistory()
        {
            var iban = HttpContext.Session.GetString("current_bank_account_iban");
            if (string.IsNullOrEmpty(iban))
                return RedirectToAction("Index");

            return RedirectToAction("Index", "TransactionHistory"); // Must have TransactionHistoryController
        }

        [HttpPost]
        public IActionResult BankAccountSettings()
        {
            var iban = HttpContext.Session.GetString("current_bank_account_iban");
            if (string.IsNullOrEmpty(iban))
                return RedirectToAction("Index");

            return RedirectToAction("Index", "BankAccountSettings");
        }

        [HttpPost]
        public IActionResult CreateBankAccount()
        {
            return RedirectToAction("Index", "BankAccountCreate"); // Must have BankAccountCreateController
        }

        [HttpPost]
        public IActionResult Loan()
        {
            var iban = HttpContext.Session.GetString("current_bank_account_iban");
            if (string.IsNullOrEmpty(iban))
                return RedirectToAction("Index");

            return RedirectToAction("Index", "Loan");
        }

        [HttpGet]
        public IActionResult GoToSocial()
        {
            return RedirectToAction("Index", "Social");
        }

        [HttpGet]
        public IActionResult AccountSettings()
        {
            return RedirectToAction("Index", "AccountSettings");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Account");
        }
    }
}
