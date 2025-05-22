using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using LoanShark.Service.SocialService.Interfaces;
using LoanShark.MVC.Models;
using LoanShark.Domain;
using User = LoanShark.MVC.Models.User;
using LoanShark.API.Proxies;

namespace LoanShark.MVC.Controllers
{
    public class AddFriendsController : Controller
    {
        private readonly ISocialUserServiceProxy _userService;

        public AddFriendsController(ISocialUserServiceProxy userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // int currentUserId = await _userService.GetCurrentUser();
            int currentUserId = 1;
            var nonFriends = await _userService.GetNonFriendsUsers(currentUserId);
            var viewModel = new AddFriendsViewModel
            {
                SearchQuery = string.Empty,
                UsersList = nonFriends
                    .Where(u => u.UserID != currentUserId) // Exclude current user
                    .Select(u => new User
                    {
                        UserID = u.UserID,
                        Username = u.Username,
                        PhoneNumber = u.PhoneNumber?.ToString()
                    }).ToList()
            };
            return View("Index", viewModel);
        }

        // POST: /Social/AddFriend
        [HttpPost]
        public async Task<IActionResult> AddFriend(int userId)
        {
            // int currentUserId = await _userService.GetCurrentUser();
            int currentUserId = 1;
            await _userService.AddFriend(currentUserId, userId);
            return RedirectToAction("Index", "FriendsList", new { showAddFriends = true });
        }
    }
}
