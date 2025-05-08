using LoanShark.API.Models;
using LoanShark.API.Proxies;
using LoanShark.Domain;
using LoanShark.EF.EFModels;
using LoanShark.Service.BankService;
using LoanShark.Service.Service.BankService;
using Microsoft.AspNetCore.Mvc;

namespace LoanShark.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService loginService;

        public LoginController(ILoginService loginService)
        {
            this.loginService = loginService;
        }

        [HttpPost("ValidateUserCredentials")]
        public async Task<IActionResult> ValidateUser([FromBody] LoginRequestDto loginRequest)
        {
            if (string.IsNullOrWhiteSpace(loginRequest.Email) || string.IsNullOrWhiteSpace(loginRequest.Password))
            {
                return this.BadRequest("Email and password must be provided.");
            }

            bool isValid = await this.loginService.ValidateUserCredentials(loginRequest.Email, loginRequest.Password);

            if (!isValid)
            {
                return this.Unauthorized("Invalid email or password.");
            }

            return this.Ok("Login successful.");
        }

        [HttpPost("InstantiateUserSessionAfterLogin")]
        public async Task<IActionResult> InstantiateSession([FromBody] LoginRequestDto loginRequest)
        { 
            await this.loginService.InstantiateUserSessionAfterLogin(loginRequest.Email);
            return this.Ok("User session instantiated");
        }

        [HttpGet("GetUserInfoAfterLogin/{email}")]
        public async Task<ActionResult<UserEF>> GetUserInfo(string email)
        {
            var user = await this.loginService.GetUserInfoAfterLogin(email);
            return user != null ? Ok(user) : NotFound();
        }

        [HttpGet("GetUserBankAccounts/{userId}")]
        public async Task<ActionResult<List<BankAccountEF>>> GetUserBankAccounts(int userId)
        {
            return await this.loginService.GetUserBankAccounts(userId);
        }
    }
}
