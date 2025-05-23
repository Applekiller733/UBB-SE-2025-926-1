using Microsoft.AspNetCore.Mvc;
using LoanShark.MVC.Models;

namespace LoanShark.MVC.Controllers
{
    public class UserInformationController : Controller
    {
        public IActionResult Index()
        {
            var model = new UserInformationViewModel
            {
                Username = HttpContext.Session.GetString("userEmail"), // fallback if Username not in session
                Email = HttpContext.Session.GetString("userEmail"),
                FirstName = HttpContext.Session.GetString("first_name"),
                LastName = HttpContext.Session.GetString("last_name"),
                PhoneNumber = HttpContext.Session.GetString("phone_number"),
                Cnp = HttpContext.Session.GetString("cnp")
            };
            return View(model);
        }
    }
} 