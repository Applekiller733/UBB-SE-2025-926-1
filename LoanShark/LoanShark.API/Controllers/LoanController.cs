using LoanShark.Service.BankService;
using Microsoft.AspNetCore.Mvc;

namespace LoanShark.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoanController : ControllerBase
    {
        private readonly ILoanService loanService;

        public LoanController(ILoanService loanService)
        {
            this.loanService = loanService;
        }

        [HttpGet("GetUserLoans/{userId}")]
        public async Task<IActionResult> GetUserLoans(int userId)
        {
            try
            {
                var loans = await loanService.GetUserLoans(userId);
                var loanDTOs = loans.Select(loan => new LoanDTO
                {
                    LoanID = loan.LoanID,
                    UserID = loan.UserID,
                    Amount = loan.Amount,
                    Currency = loan.Currency,
                    DateTaken = loan.DateTaken,
                    DateDeadline = loan.DateDeadline,
                    DatePaid = loan.DatePaid,
                    TaxPercentage = loan.TaxPercentage,
                    NumberMonths = loan.NumberMonths,
                    State = loan.State,
                })
               .ToList();

                return Ok(loanDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to get user loans: {ex.Message}");
            }
        }

        [HttpPost("TakeLoan")]
        public async Task<IActionResult> TakeLoan([FromBody] TakeLoanDTO dto)
        {
            try
            {
                var result = await loanService.TakeLoanAsync(dto.UserId, dto.Amount, dto.Currency, dto.AccountIBAN, dto.Months);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to take loan: {ex.Message}");
            }

        }

        [HttpPost("PayLoan")]
        public async Task<IActionResult> PayLoan([FromBody] PayLoanDTO dto)
        {
            try
            {
                var result = await loanService.PayLoanAsync(dto.UserId, dto.LoanId, dto.AccountIBAN);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to pay loan: {ex.Message}");
            }
        }

        [HttpGet("GetFormattedBankAccounts/{userId}")]
        public async Task<IActionResult> GetFormattedBanckAccounts(int userId)
        {
            try
            {
                var result = await loanService.GetFormattedBankAccounts(userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to get formated bank account: {ex.Message}");
            }
        }

        [HttpPost("ConvertCurrency")]
        public async Task<IActionResult> ConvertCurrency([FromBody] ConvertCurrencyDTO dto)
        {
            try
            {
                var result = await loanService.ConvertCurrency(dto.Amount, dto.FromCurrency, dto.ToCurrency);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Currency conversion failed: {ex.Message}");
            }
        }
    }
}
