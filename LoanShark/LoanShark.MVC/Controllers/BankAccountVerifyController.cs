using Microsoft.AspNetCore.Mvc;
using LoanShark.Service.BankService;
using LoanShark.Domain;
using System.Threading.Tasks;
using LoanShark.MVC.Models;
using Microsoft.AspNetCore.Http;

namespace LoanShark.Controllers
{
    public class BankAccountVerifyController : Controller
    {
        private readonly IBankAccountService _bankAccountService;

        public BankAccountVerifyController(IBankAccountService bankAccountService)
        {
            _bankAccountService = bankAccountService;
        }

        [HttpGet]
        public IActionResult Verify(string iban)
        {
            return View(new BankAccountVerifyModel { Iban = iban });
        }

        [HttpPost]
        public async Task<IActionResult> Verify(BankAccountVerifyModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var email = HttpContext.Session.GetString("email");
            if (email == null)
            {
                ModelState.AddModelError("", "Session expired. Please log in again.");
                return View(model);
            }

            var verified = await _bankAccountService.VerifyUserCredentials(email, model.Password);
            if (!verified)
            {
                ModelState.AddModelError(nameof(model.Password), "Invalid password.");
                return View(model);
            }

            // ✅ Password is valid — proceed with next step (e.g., redirect to delete)
            TempData["VerifiedIBAN"] = model.Iban;
            return RedirectToAction("Confirm", "BankAccountDelete", new { iban = model.Iban });
        }
    }
}
