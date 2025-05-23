using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using LoanShark.MVC.Models;
using LoanShark.Service.BankService;

namespace LoanShark.MVC.Controllers
{
    public class SendMoneyController : Controller
    {
        private readonly ITransactionsService transactionsService;

        public SendMoneyController(ITransactionsService transactionsService)
        {
            this.transactionsService = transactionsService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new SendMoneyViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Send(SendMoneyViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Iban) || string.IsNullOrWhiteSpace(model.SumOfMoney))
            {
                model.ErrorMessage = "IBAN and amount are required.";
                return View("Index", model);
            }

            // Get sender IBAN from session (previously selected)
            string? senderIban = HttpContext.Session.GetString("current_bank_account_iban");
            if (string.IsNullOrEmpty(senderIban))
            {
                model.ErrorMessage = "Sender account is not selected.";
                return View("Index", model);
            }

            // Convert amount safely
            if (!decimal.TryParse(model.SumOfMoney, out decimal amount))
            {
                model.ErrorMessage = "Invalid amount format.";
                return View("Index", model);
            }

            // Call the transaction service
            string result = await this.transactionsService.AddTransaction(
                senderIban,
                model.Iban,
                amount,
                model.Details ?? ""
            );

            TempData["ResultMessage"] = result;
            return RedirectToAction("Result");
        }


        [HttpGet]
        public IActionResult Result()
        {
            ViewBag.Message = TempData["ResultMessage"];
            return View();
        }

        [HttpPost]
        public IActionResult Close()
        {
            return RedirectToAction("Index", "MainPage");
        }
    }
}