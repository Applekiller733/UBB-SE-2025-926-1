using Microsoft.AspNetCore.Mvc;
using LoanShark.Domain;
using LoanShark.API.Proxies;

namespace LoanShark.MVC.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserServiceProxy _userService;

        public UserController(IUserServiceProxy userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> Info()
        {
            
            
            var user = await _userService.GetUserInformationAsync();
            if (user == null)
                return NotFound();

            return View(user);
        }
    }
}
