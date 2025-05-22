using Microsoft.AspNetCore.Mvc;
using LoanShark.Service.BankService;
using LoanShark.MVC.Models;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace LoanShark.MVC.Controllers
{
    public class DeleteAccountController : Controller
    {
        private readonly IUserService userService;

        public DeleteAccountController(IUserService userService)
        {
            this.userService = userService;
        }

        public IActionResult Index()
        {
            return View(new DeleteAccountViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAccount(DeleteAccountViewModel model)
        {
            if (string.IsNullOrEmpty(model.Password))
            {
                model.ErrorMessage = "Password needs to be filled in";
                return View("Index", model);
            }

            var result = await userService.DeleteUser(model.Password);
            if (result != "User deleted") 
            {
                model.ErrorMessage = $"Failed to delete account: {result}";
                return View("Index", model);
            }


            HttpContext.Session.Clear();

            TempData["SuccessMessage"] = "Your account has been deleted.";
            return RedirectToAction("Index", "Login");


        }
    }
}
