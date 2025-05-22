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
        public async Task<ActionResult> Index()
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
        public async Task<IActionResult> Index(BankAccountCreateModel model)
        {
            var userId = HttpContext.Session.GetInt32("userId");
            if (userId == null)
            {
                TempData["Error"] = "User session expired. Please log in again.";
                return RedirectToAction("Index", "Account");
            }

            if (Request.Form.TryGetValue("SelectedCurrencyIndex", out var selectedIndexStr)
                && int.TryParse(selectedIndexStr, out int selectedIndex)
                && selectedIndex >= 0 && selectedIndex < model.AvailableCurrencies.Count)
            {
                model.AvailableCurrencies[selectedIndex].IsChecked = true;
                model.SelectedCurrency = model.AvailableCurrencies[selectedIndex];
            }
            else
            {
                TempData["Error"] = "Please select a currency.";
                model.AvailableCurrencies = (await _bankAccountService.GetCurrencies())
                    .Select(name => new CurrencyItemModel { Name = name })
                    .ToList();
                return View(model);
            }

            if (string.IsNullOrWhiteSpace(model.CustomName))
            {
                ModelState.AddModelError(nameof(model.CustomName), "Please enter a custom name.");
                return View(model);
            }

            var success = await _bankAccountService.CreateBankAccount(
                userId.Value,
                model.CustomName,
                model.SelectedCurrency.Name
            );

            if (!success)
            {
                TempData["Error"] = "Failed to create bank account.";
                return View(model);
            }

            TempData["Success"] = "Bank account created successfully!";
            return RedirectToAction("Index", "MainPage");
        }

    }
}
