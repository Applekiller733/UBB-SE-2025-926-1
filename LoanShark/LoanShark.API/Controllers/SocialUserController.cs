using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using LoanShark.Domain;
using LoanShark.API.Models;
using LoanShark.Service.SocialService.Implementations;
using LoanShark.EF.Repository.SocialRepository;
using LoanShark.Service.SocialService.Interfaces;
using Windows.System;
using User = LoanShark.Domain.User;
using Microsoft.AspNetCore.Routing.Constraints;


namespace LoanShark.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SocialUserController : ControllerBase
    {
        private readonly IUserService _userService;

        public SocialUserController(IUserService us)
        {
            //IRepository repository = new Repository();
            //INotificationService notificationService = new NotificationService(repository);
            this._userService = us;
        }

        [HttpGet("repo")]
        public async Task<ActionResult<IRepository>> GetRepo()
        {
            var repo = _userService.GetRepo();
            return Ok(repo);
        }

        [HttpPost("{userID}/friends/{newFriendID}")]
        public async Task<ActionResult> AddFriend(int userID, int newFriendID)
        {
            await _userService.AddFriend(userID, newFriendID);
            return NoContent();
        }

        [HttpDelete("{userID}/friends/{oldFriendID}")]
        public async Task<ActionResult> RemoveFriend(int userID, int oldFriendID)
        {
            await _userService.RemoveFriend(userID, oldFriendID);
            return NoContent();
        }

        [HttpPost("{userID}/chats/{chatID}")]
        public async Task<ActionResult> JoinChat(int userID, int chatID)
        {
            await _userService.JoinChat(userID, chatID);
            return NoContent();
        }

        [HttpDelete("{userID}/chats/{chatID}")]
        public async Task<ActionResult> LeaveChat(int userID, int chatID)
        {
            await _userService.LeaveChat(userID, chatID);
            return NoContent();
        }

        [HttpGet("filter")]
        public async Task<ActionResult<List<int>>> FilterUsers([FromQuery] string keyword, [FromQuery] int userID)
        {
            var userIds = await _userService.FilterUsers(keyword, userID);
            return Ok(userIds);
        }

        [HttpGet("{userID}/friends/filter")]
        public async Task<ActionResult<List<int>>> FilterFriends([FromQuery] string keyword, int userID)
        {
            var friendIds = await _userService.FilterFriends(keyword, userID);
            return Ok(friendIds);
        }

        [HttpGet("{userID}/friends/ids")]
        public async Task<ActionResult<List<int>>> GetFriendsIDsByUser(int userID)
        {
            var friendIds = await _userService.GetFriendsIDsByUser(userID);
            return Ok(friendIds);
        }

        [HttpGet("{userID}/friends")]
        public async Task<ActionResult<List<SocialUserViewModel>>> GetFriendsByUser(int userID)
        {
            var friends = await _userService.GetFriendsByUser(userID);
            var dtos = friends.Select(f => new SocialUserViewModel
            {
                UserID = f.UserID,
                Username = f.Username,
                FirstName = f.FirstName,
                LastName = f.LastName,
                Email = f.Email?.ToString(),
                PhoneNumber = f.PhoneNumber?.ToString(),
                Cnp = f.Cnp?.ToString(),
                HashedPassword = f.HashedPassword?.ToString(),
                ReportedCount = f.ReportedCount
            }).ToList();
            return Ok(dtos);
        }

        [HttpGet("{userID}/chats")]
        public async Task<ActionResult<List<int>>> GetChatsByUser(int userID)
        {
            var chatIds = await _userService.GetChatsByUser(userID);
            return Ok(chatIds);
        }

        [HttpGet("chats/current")]
        public async Task<ActionResult<List<ChatViewModel>>> GetCurrentUserChats()
        {
            var chats = await _userService.GetCurrentUserChats();
            var dtos = chats.Select(c => new ChatViewModel
            {
                ChatID = c.getChatID(),
                UserIDs = c.getUserIDsList()
            });
            return Ok(dtos);
        }

        [HttpGet("{userID}")]
        public async Task<ActionResult<UserViewModel>> GetUserById(int userID)
        {
            var user = await _userService.GetUserById(userID);
            if (user.GetUserId() == 0) // Assuming GetUserId() returns 0 for a new/empty User
                return NotFound();

            var dto = new UserViewModel
            {
                UserID = user.UserID,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email?.ToString(),
                PhoneNumber = user.PhoneNumber?.ToString(),
                Cnp = user.Cnp?.ToString(),
                Password = user.HashedPassword.ToString(),
                ReportedCount = user.ReportedCount
            };
            return Ok(dto);
        }

        [HttpGet("{userID}/nonfriends")]
        public async Task<ActionResult<List<SocialUserViewModel>>> GetNonFriendsUsers(int userID)
        {
            var nonFriends = await _userService.GetNonFriendsUsers(userID);
            //var dtos = nonFriends.Select(u => new UserViewModel
            //{
            //    UserID = u.UserID,
            //    Username = u.Username,
            //    FirstName = u.FirstName,
            //    LastName = u.LastName,
            //    Email = u.Email?.ToString(),
            //    PhoneNumber = u.PhoneNumber?.ToString(),
            //    Cnp = u.Cnp?.ToString(),
            //    Password = u.HashedPassword.ToString(),
            //    ReportedCount = u.ReportedCount
            //}).ToList();
            //return Ok(dtos);
            if (nonFriends.Count == 0)
                return new List<SocialUserViewModel>();

            var result = nonFriends.Select(u => new SocialUserViewModel
            {
                UserID = u.UserID, // Use the non-friend's UserID
                Username = u.Username,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email?.ToString(),
                PhoneNumber = u.PhoneNumber?.ToString(),
                Cnp = u.Cnp?.ToString(),
                HashedPassword = u.HashedPassword?.ToString()
            }).ToList();
            return Ok(result);
        }

        [HttpGet("current")]
        public ActionResult<int> GetCurrentUser()
        {
            var userId = _userService.GetCurrentUser();
            return Ok(userId);
        }

        [HttpPost("dangerous")]
        public ActionResult MarkUserAsDangerousAndGiveTimeout([FromBody] UserViewModel userDto)
        {
            var user = new User(
                userDto.UserID,
                new Cnp(userDto.Cnp),
                userDto.Username,
                userDto.FirstName,
                userDto.LastName,
                new Email(userDto.Email),
                new PhoneNumber(userDto.PhoneNumber),
                new HashedPassword(userDto.Password)
            );
            _userService.MarkUserAsDangerousAndGiveTimeout(user);
            return NoContent();
        }

        [HttpPost("{userID}/timeout")]
        public ActionResult<bool> IsUserInTimeout([FromBody] UserViewModel userDto)
        {
            var user = new User(
                userDto.UserID,
                new Cnp(userDto.Cnp),
                userDto.Username,
                userDto.FirstName,
                userDto.LastName,
                new Email(userDto.Email),
                new PhoneNumber(userDto.PhoneNumber),
                new HashedPassword(userDto.Password)
            );
            var isInTimeout = _userService.IsUserInTimeout(user);
            return Ok(isInTimeout);
        }
    }
}