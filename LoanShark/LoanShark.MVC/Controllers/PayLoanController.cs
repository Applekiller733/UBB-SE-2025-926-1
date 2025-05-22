using System.Diagnostics;
using LoanShark.EF.Repository.BankRepository;
using LoanShark.MVC.Models;
using LoanShark.Service.BankService;
using Microsoft.AspNetCore.Mvc;

namespace LoanShark.MVC.Controllers
{
    public class PayLoanController : Controller
    {
        private readonly ILoanService loanService;
        public PayLoanController(ILoanService loanService)
        {
            this.loanService = loanService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {

            var userId = HttpContext.Session.GetInt32("userId");
           
            var model = new PayLoanViewModel
            {
                BankAccounts = await this.loanService.GetFormattedBankAccounts(userId.Value),
                UnpaidLoans = await this.loanService.GetUnpaidUserLoans(userId.Value)

            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> PayLoan(PayLoanViewModel model, string action)
        {
           
            try
            {
                var userId = HttpContext.Session.GetInt32("userId");
                model.BankAccounts = await this.loanService.GetFormattedBankAccounts(userId.Value);
                model.UnpaidLoans = await this.loanService.GetUnpaidUserLoans(userId.Value);

                if (string.IsNullOrEmpty(model.SelectedBankAccount))
                {
                    Debug.WriteLine("Validation failed: No bank account selected");
                    model.PayErrorMessage = "No bank account selected";
                    return View("Index", model);
                }

                if (!model.SelectedLoanId.HasValue)
                {
                    model.PayErrorMessage = "No loan selected";
                    return View("Index", model);
                }

                if (action == "showDetails")
                {
                    
                    string[] parts = model.SelectedBankAccount.Split('-');
                    if (parts.Length >= 3)
                    {
                        string accountCurrency = parts[1].Trim();
                        decimal accountBalance = decimal.Parse(parts[2].Trim());
                        string bankAccountID = parts[0].Trim();

                        model.SelectedAccountBalance = $"{accountBalance:F2} {accountCurrency}";
                        
             
                        var selectedLoan = loanService.GetUnpaidUserLoans(userId.Value).Result.FirstOrDefault(loan => loan.LoanID == model.SelectedLoanId);

                        
                        decimal loanAmount = selectedLoan.AmountToPay;
                        string loanCurrency = selectedLoan.Currency;

                        model.SelectedLoanAmount = $"{loanAmount:F2} {loanCurrency}";

                        if (loanCurrency == accountCurrency)
                        {
                            model.ConvertedLoanAmount = $"{loanAmount:F2} {loanCurrency}";
                        }
                        else
                        {
                            decimal convertedAmount = await loanService.ConvertCurrency(loanAmount, loanCurrency, accountCurrency);
                            model.ConvertedLoanAmount = $" {convertedAmount:F2} {accountCurrency} (converted from {loanAmount:F2} {loanCurrency})";
                        }

                        return View("Index", model);
                    }

                }
                    // var userId = HttpContext.Session.GetInt32("userId");

                    string bankAccountId = ExtractIbanFromBankAccount(model.SelectedBankAccount);
               
                string errorMessage = await loanService.PayLoanAsync(userId.Value, model.SelectedLoanId.Value, bankAccountId);
                if (errorMessage != "success")
                {
                    model.PayErrorMessage = errorMessage;
                    return View("Index", model);
                }

                TempData["SuccessMessage"] = "Loan was successfully taken!";
                return RedirectToAction("Index", "Loans");
            }
            catch (Exception ex)
            {

                model.PayErrorMessage = "An error occurred while processing your payment";
                return View("Index", model);
            }
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
