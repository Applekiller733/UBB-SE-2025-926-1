using System.Diagnostics;
using System.Threading.Tasks;
using LoanShark.API.Controllers;
using LoanShark.Domain;
using LoanShark.MVC.Models;
using LoanShark.Service.BankService;
using Microsoft.AspNetCore.Mvc;

namespace LoanShark.MVC.Controllers
{
    public class LoansController : Controller
    {
        private readonly ILoanService loanService;
        public LoansController(ILoanService loanService)
        {
            this.loanService = loanService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("userId");
            var allLoans = await this.loanService.GetUserLoans(userId.Value);
            var unpaidLoans = await this.loanService.GetUnpaidUserLoans(userId.Value);

            var model = new LoansViewModel
            {
                Loans = allLoans,
                UnpaidLoans = unpaidLoans

            };

            return View(model);
        }

        public async Task<IActionResult> TakeLoan()
        {
            return RedirectToAction("Index", "TakeLoan");
        }

        public IActionResult PayLoan()
        {

            return RedirectToAction("Index", "PayLoan");
        }

        public IActionResult Close()
        {

            return RedirectToAction("Index", "MainPage");
        }

    }
}
