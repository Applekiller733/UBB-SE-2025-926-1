using Microsoft.AspNetCore.Mvc;
using LoanShark.Service.BankService;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using LoanShark.MVC.Models;

namespace LoanShark.MVC.Controllers
{
    public class CurrencyExchangeController : Controller
    {

        private readonly ITransactionsService transactionsService;
        public async Task<IActionResult> Index()
        {
            var rawRates = await this.transactionsService.GetAllCurrencyExchangeRates();

            var rates = rawRates.Select(rate =>
            {
                return new CurrencyExchangeRateDTO
                {
                    FromCurrency = rate.FromCurrency,
                    ToCurrency = rate.ToCurrency,
                    ExchangeRate = rate.ExchangeRate,
                };
                
            }).ToList();

            return View(new CurrencyExchangeViewModel
            {
                ExchangeRates = rates,
            });
        }

        public CurrencyExchangeController(ITransactionsService transactionService)
        {
            this.transactionsService = transactionService;
        }
        public IActionResult Close()
        {
            return RedirectToAction("Index", "Transactions");
        }



    }
}
