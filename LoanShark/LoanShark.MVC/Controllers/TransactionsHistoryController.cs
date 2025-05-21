using LoanShark.MVC.Models;
using LoanShark.Service.BankService;
using Microsoft.AspNetCore.Mvc;
using System.Collections.ObjectModel;
using Windows.UI.WindowManagement;

namespace LoanShark.MVC.Controllers
{
    public class TransactionsHistoryController : Controller
    {
        private readonly ITransactionHistoryService _transactionsHistoryService;

        public TransactionsHistoryController(ITransactionHistoryService transactionsHistoryService)
        {
            _transactionsHistoryService = transactionsHistoryService;
        }

        public async Task<IActionResult> Index(string? Filter)
        {
            //_transactionsHistoryService.iban = HttpContext.Session.GetString("current_bank_account_iban");
            _transactionsHistoryService.iban = "RO10SEUPBMC6N7XG6GRUWOK3";

            ObservableCollection <string> transactions;

            if (Filter == null)
            {
                transactions = await _transactionsHistoryService.RetrieveForMenu();
            }
            else
            {
                transactions = await _transactionsHistoryService.FilterByTypeForMenu(Filter);
            }

            List<TransactionsHistoryDTO> trans = new List<TransactionsHistoryDTO>();
            foreach (var transaction in transactions)
            {
                var data = transaction.Split('\n');

                trans.Add(new TransactionsHistoryDTO
                {
                    SenderIBAN = data[0],
                    ReceiverIBAN = data[1],
                    SentAmount = data[2],
                    ReceivedAmount = data[3],
                    Date = data[4],
                    Type = data[5],
                });
            }

            return View(new TransactionsHistoryViewModel
            {
                Transactions = trans,
            });
        }

        public async Task<IActionResult> ExportToCsv()
        {
            //_transactionsHistoryService.iban = HttpContext.Session.GetString("current_bank_account_iban");
            _transactionsHistoryService.iban = "RO10SEUPBMC6N7XG6GRUWOK3";

            await _transactionsHistoryService.CreateCSV();

            TempData["AlertMessage"] = "Exported to CSV on desktop!";

            return RedirectToAction("Index");
        }

        public IActionResult Chart()
        {
            return RedirectToAction("Index", "TransactionsHistoryChart");
        }
    }
}
