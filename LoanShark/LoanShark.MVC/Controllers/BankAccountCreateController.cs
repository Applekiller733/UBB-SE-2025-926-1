using Microsoft.AspNetCore.Mvc;
using LoanShark.Domain;
using LoanShark.API.Proxies;
using LoanShark.Service.BankService;
using LoanShark.MVC.Models;
using Microsoft.AspNetCore.Http;

namespace LoanShark.MVC.Controllers
{
    public class BankAccountCreateController : Controller
    {
        private readonly IBankAccountService _bankAccountService;

        public BankAccountCreateController(IBankAccountService bankAccountService)
        {
            _bankAccountService = bankAccountService;
        }

        [HttpGet]
        public async Task<ActionResult> Create()
        {
            var model = new BankAccountCreateModel
            {
                AvailableCurrencies = (await _bankAccountService.GetCurrencies())
                    .Select(currencyName => new CurrencyItemModel { Name = currencyName })
                    .ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Create(BankAccountCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AvailableCurrencies = (await _bankAccountService.GetCurrencies())
                    .Select(currencyName => new CurrencyItemModel { Name = currencyName })
                    .ToList();

                return View(model);
            }

            var userId = HttpContext.Session.GetInt32("userId");
            if (userId == null)
            {
                TempData["Error"] = "User session expired. Please log in again.";
                return RedirectToAction("Login", "User");
            }

            var selectedCurrency = model.SelectedCurrency?.Name;
            if (string.IsNullOrWhiteSpace(selectedCurrency))
            {
                model.AvailableCurrencies = (await _bankAccountService.GetCurrencies())
                    .Select(name => new CurrencyItemModel { Name = name })
                    .ToList();

                ModelState.AddModelError(string.Empty, "Please select a currency.");
                return View(model);
            }

            var success = await _bankAccountService.CreateBankAccount(
                userId.Value,
                model.CustomName ?? string.Empty,
                selectedCurrency
            );

            if (!success)
            {
                TempData["Error"] = "Failed to create bank account.";
                return View(model);
            }

            TempData["Success"] = "Bank account created successfully!";
            return RedirectToAction("Index", "BankAccount");
        }
    }
}
