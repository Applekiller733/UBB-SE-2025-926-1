using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using LoanShark.Service.BankService;
using LoanShark.Domain;
using LoanShark.API.Models;


namespace LoanShark.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController()
        {
            _userService = new UserService();
        }

        [HttpGet("info")]
        public async Task<ActionResult<UserViewModel>> GetUserInformation()
        {
            UserSession.Instance.SetUserData("id_user", "1"); // hardcoded for now
            var user = await _userService.GetUserInformation();
            if (user == null)
                return NotFound();

            var dto = new UserViewModel
            {
                UserID = user.UserID,
                Cnp = user.Cnp.ToString(),
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email.ToString(),
                PhoneNumber = user.PhoneNumber.ToString(),
                Password = user.HashedPassword.GetHashedPassword(),
            };
            return Ok(dto);
        }
    }
}
