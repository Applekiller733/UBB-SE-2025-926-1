using LoanShark.Service.BankService;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LoanShark.MVC.Controllers
{
    public class TransactionsHistoryChartController : Controller
    {
        private readonly ITransactionHistoryService _transactionsHistoryService;

        public TransactionsHistoryChartController(ITransactionHistoryService transactionsHistoryService)
        {
            _transactionsHistoryService = transactionsHistoryService;
        }
        public async Task<IActionResult> Index()
        {
            //_transactionsHistoryService.iban = HttpContext.Session.GetString("current_bank_account_iban");
            _transactionsHistoryService.iban = "RO10SEUPBMC6N7XG6GRUWOK3";
    
            var transactionTypesCount = await _transactionsHistoryService.GetTransactionTypeCounts();

            ViewBag.ChartData = transactionTypesCount;

            return View();
        }
    }
}
