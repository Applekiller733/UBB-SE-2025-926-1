using Microsoft.AspNetCore.Mvc;

namespace LoanShark.MVC.Controllers
{
    public class SocialController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
