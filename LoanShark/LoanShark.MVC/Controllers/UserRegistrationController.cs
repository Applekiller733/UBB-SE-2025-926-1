using Microsoft.AspNetCore.Mvc;
using LoanShark.Domain;
using LoanShark.API.Proxies;
using LoanShark.Service.BankService;
using System.Net;

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
                // Get client IP address
                var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
                HttpContext.Session.SetString("UserIP", ipAddress);

                // Map view model to domain User
                var user = new User
                {
                    Username = model.Username,
                    Email = model.Email,
                    Password = model.Password,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    Cnp = model.Cnp,
                    LastLoginIP = ipAddress,
                    LastLoginDate = DateTime.UtcNow
                };

                var result = await _userService.RegisterUserAsync(user);
                if (result)
                {
                    // Save session data as at login
                    HttpContext.Session.SetInt32("userId", user.UserID);
                    HttpContext.Session.SetString("userEmail", user.Email);
                    HttpContext.Session.SetString("first_name", user.FirstName);
                    HttpContext.Session.SetString("last_name", user.LastName);
                    HttpContext.Session.SetString("phone_number", user.PhoneNumber);
                    HttpContext.Session.SetString("cnp", user.Cnp);
                    HttpContext.Session.SetString("UserIP", ipAddress);
                    return RedirectToAction("Index", "Home");
                }
                
                ModelState.AddModelError("", "Registration failed. Please try again.");
                return View("Index", model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred during registration. Please try again.");
                return View("Index", model);
            }
        }
    }
} 