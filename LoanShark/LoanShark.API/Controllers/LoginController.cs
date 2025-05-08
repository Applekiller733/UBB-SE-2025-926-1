using LoanShark.API.Models;
using LoanShark.API.Proxies;
using LoanShark.Domain;
using LoanShark.EF.EFModels;
using LoanShark.Service.BankService;
using LoanShark.Service.Service.BankService;
using Microsoft.AspNetCore.Mvc;
using Windows.System;

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
        public async Task<ActionResult<UserEF>> InstantiateSession([FromBody] LoginRequestDto loginRequest)
        { 
            await this.loginService.InstantiateUserSessionAfterLogin(loginRequest.Email);
            var content = await this.loginService.GetUserInfoAfterLogin(loginRequest.Email);
            List<BankAccountEF> dt_bank_accounts = await this.loginService.GetUserBankAccounts(int.Parse(content.UserID.ToString() ?? string.Empty));
            string iban = string.Empty;

            if (dt_bank_accounts.Count > 0)
            {
                iban = dt_bank_accounts[0].Iban.ToString() ?? string.Empty;
            }
            UserSessionDto sessionDto = new UserSessionDto
            {
                UserId = content.UserID.ToString() ?? string.Empty,
                Cnp = content.Cnp?.ToString() ?? string.Empty,
                FirstName = content.FirstName?.ToString() ?? string.Empty,
                LastName = content.LastName?.ToString() ?? string.Empty,
                Email = content.Email?.ToString() ?? string.Empty,
                PhoneNumber = content.PhoneNumber?.ToString() ?? string.Empty,
                Iban = iban,
            };
            return this.Ok(sessionDto);
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
