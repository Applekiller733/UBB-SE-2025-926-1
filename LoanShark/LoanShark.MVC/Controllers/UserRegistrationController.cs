using Microsoft.AspNetCore.Mvc;
using LoanShark.API.Proxies;
using LoanShark.Service.BankService;
using System.Net;
using LoanShark.Domain;
using LoanShark.MVC.Models;
using User = LoanShark.Domain.User;

namespace LoanShark.MVC.Controllers
{
    public class UserRegistrationController : Controller
    {
        private readonly IUserService _userService;

        public UserRegistrationController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View(new UserRegistrationViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegistrationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            try
            {
                await _userService.CreateUser(model.Cnp, model.Username, model.FirstName,model.LastName, model.Email, model.PhoneNumber, model.Password);
                return RedirectToAction("Index", "Account");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("Index", model);
            }
        }
    }
} 