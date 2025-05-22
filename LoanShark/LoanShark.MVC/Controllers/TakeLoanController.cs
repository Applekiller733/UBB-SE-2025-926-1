using System;
using System.Diagnostics;
using LoanShark.MVC.Models;
using LoanShark.Service.BankService;
using Microsoft.AspNetCore.Mvc;

namespace LoanShark.MVC.Controllers
{
    public class TakeLoanController : Controller
    {

        private readonly ILoanService loanService;
        public TakeLoanController(ILoanService loanService)
        {
            this.loanService = loanService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {

            var userId = HttpContext.Session.GetInt32("userId");
            var model = new TakeLoanViewModel
            {
                BankAccounts = await this.loanService.GetFormattedBankAccounts(userId.Value),

                Months = new List<int> { 6, 12, 24, 36 }
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Take(TakeLoanViewModel model, string action)
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("userId");
                model.BankAccounts = await loanService.GetFormattedBankAccounts(userId.Value);
                model.Months = new List<int> { 6, 12, 24, 36 };
                if (action == "calculate")
                {
                    var taxPercentage = loanService.CalculateTaxPercentage(model.SelectedMonths);
                    var amountToPay = loanService.CalculateAmountToPay(model.Amount, taxPercentage);

                    model.TaxPercentage = taxPercentage;
                    model.AmountToPay = amountToPay;

                    return View("Index", model);
                }

                    var errorMessage = loanService.ValidateLoanRequest(model.Amount, model.SelectedMonths);
                    if (errorMessage != "success")
                    {
                        model.TakeErrorMessage = errorMessage;
                        return View("Index", model);
                    }


                    string currency = ExtractCurrencyFromBankAccount(model.SelectedBankAccount);
                    string iban = ExtractIbanFromBankAccount(model.SelectedBankAccount);


                    var newLoan = await loanService.TakeLoanAsync(userId.Value, model.Amount, currency, iban, model.SelectedMonths);
                    TempData["SuccessMessage"] = "Loan was successfully taken!";

                    return RedirectToAction("Index", "Loans");
                }
                catch (Exception ex)
                {

                    model.TakeErrorMessage = "An error occurred while processing your loan";
                    return View("Index", model);
            }
        }

        private string ExtractCurrencyFromBankAccount(string bankAccount)
        {
            Debug.WriteLine($"Extracting currency from: {bankAccount}");

            // Format: "IBAN1 - EUR - 1000"
            string[] parts = bankAccount.Split('-');
            if (parts.Length >= 2)
            {
                string currency = parts[1].Trim();
                Debug.WriteLine($"Extracted currency: {currency}");
                return currency;
            }

            Debug.WriteLine("Could not extract currency, using default EUR");
            return "EUR"; // Default currency
        }

        private string ExtractIbanFromBankAccount(string bankAccount)
        {
            string[] parts = bankAccount.Split('-');
            if (parts.Length >= 1)
            {
                return parts[0].Trim();
            }
            throw new ArgumentException("Invalid bank account format");
        }
    }
}
