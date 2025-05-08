using Microsoft.AspNetCore.Mvc;
using LoanShark.Domain;
using LoanShark.API.Proxies;
using LoanShark.Service.BankService;

namespace LoanShark.MVC.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> Info()
        {
            
            
            //var user = await _userService.GetUserInformationAsync();
            //if (user == null)
            //    return NotFound();

            return View(null);
        }
    }
}
