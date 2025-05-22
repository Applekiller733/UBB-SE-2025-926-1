using LoanShark.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using LoanShark.Service.Service.BankService;

public class AccountController : Controller
{
    private readonly ILoginService _loginService;

    public AccountController(ILoginService loginService)
    {
        _loginService = loginService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View("Index", new LoginViewModel()); // loads Views/Account/Index.cshtml
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
        {
            model.ErrorMessage = "Email and password cannot be empty.";
            model.IsErrorVisible = true;
            return View("Index", model);
        }

        bool isValid = await _loginService.ValidateUserCredentials(model.Email, model.Password);
        if (!isValid)
        {
            model.ErrorMessage = "Invalid credentials.";
            model.IsErrorVisible = true;
            return View("Index", model);
        }

        await _loginService.InstantiateUserSessionAfterLogin(model.Email);
        var user = await _loginService.GetUserInfoAfterLogin(model.Email);

        HttpContext.Session.SetInt32("userId", user.UserID);
        HttpContext.Session.SetString("userEmail", user.Email);
        HttpContext.Session.SetString("first_name", user.FirstName);
        HttpContext.Session.SetString("last_name", user.LastName);
        HttpContext.Session.SetString("phone_number", user.PhoneNumber);
        HttpContext.Session.SetString("cnp", user.Cnp);

        return RedirectToAction("Index", "MainPage");
    }

    [HttpGet]
    public IActionResult Register()
    {
        return RedirectToAction("Index", "UserRegistration");
    }


    [HttpGet]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}