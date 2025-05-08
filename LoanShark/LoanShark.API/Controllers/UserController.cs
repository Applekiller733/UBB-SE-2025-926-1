using Microsoft.AspNetCore.Mvc;
using LoanShark.Service.BankService;
using LoanShark.Domain;
using System.Threading.Tasks;
using LoanShark.API.Models;

namespace LoanShark.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService userService;

        public UserController(UserService userService)
        {
            this.userService = userService;
        }

        [HttpGet("info")]
        public async Task<ActionResult<User>> GetUserInformation()
        {
            var user = await userService.GetUserInformation();
            if (user == null) return NotFound("User not found");
            return Ok(user);
        }

        [HttpPost("check-cnp")]
        public async Task<ActionResult> CheckCnp([FromBody] string cnp)
        {
            var result = await userService.CheckCnp(cnp);
            return string.IsNullOrEmpty(result) ? Ok() : BadRequest(result);
        }

        [HttpPost("check-email")]
        public async Task<ActionResult> CheckEmail([FromBody] string email)
        {
            var result = await userService.CheckEmail(email);
            return string.IsNullOrEmpty(result) ? Ok() : BadRequest(result);
        }

        [HttpPost("check-phone")]
        public async Task<ActionResult> CheckPhone([FromBody] string phone)
        {
            var result = await userService.CheckPhoneNumber(phone);
            return string.IsNullOrEmpty(result) ? Ok() : BadRequest(result);
        }

        [HttpPost("create")]
        public async Task<ActionResult> CreateUser([FromBody] CreateUserDto dto)
        {
            await userService.CreateUser(dto.Cnp, dto.UserName, dto.FirstName, dto.LastName, dto.Email, dto.PhoneNumber, dto.Password);
            return Ok("User created");
        }

        [HttpPut("update")]
        public async Task<ActionResult> UpdateUser([FromBody] User user)
        {
            var success = await userService.UpdateUser(user);
            return success ? Ok("User updated") : BadRequest("Update failed");
        }

        [HttpPost("delete")]
        public async Task<ActionResult> DeleteUser([FromBody] string password)
        {
            var result = await userService.DeleteUser(password);
            return result == "Succes" ? Ok("User deleted") : BadRequest(result);
        }

        [HttpGet("hash-salt")]
        public async Task<ActionResult<string[]>> GetHashSalt()
        {
            var result = await userService.GetUserPasswordHashSalt();
            return Ok(result);
        }
    }
}
