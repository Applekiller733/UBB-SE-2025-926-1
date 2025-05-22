using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoanShark.Service.SocialService.Interfaces;
using LoanShark.API.Proxies;
using LoanShark.MVC.Models;

namespace LoanShark.MVC.Controllers
{
    public class FriendsListController : Controller
    {
        private readonly ISocialUserServiceProxy _userService;

        public FriendsListController(ISocialUserServiceProxy userService)
        {
            _userService = userService;
        }

        // GET: /FriendsList
        [HttpGet]
        public async Task<IActionResult> Index(string searchQuery, bool showAddFriends = false)
        {
            //int currentUserId = await _userService.GetCurrentUser();
            int currentUserId = 1;
            var friends = await _userService.GetFriendsByUser(currentUserId);
            var viewModel = new FriendsListViewModel
            {
                SearchQuery = searchQuery ?? string.Empty,
                FriendsList = friends
                    .Where(f => f.UserID != currentUserId)
                    .Where(f => string.IsNullOrEmpty(searchQuery) ||
                               f.Username.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                               f.PhoneNumber.ToString().Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
                    .ToList(),
                ShowAddFriends = showAddFriends
            };

            if (showAddFriends)
            {
                var nonFriends = await _userService.GetNonFriendsUsers(currentUserId);
                var addFriendsModel = new AddFriendsViewModel
                {
                    SearchQuery = string.Empty,
                    UsersList = nonFriends
                        .Where(u => u.UserID != currentUserId)
                        .Select(u => new User
                        {
                            UserID = u.UserID,
                            Username = u.Username,
                            PhoneNumber = u.PhoneNumber?.ToString()
                        }).ToList()
                };

                ViewData["AddFriendsModel"] = addFriendsModel;
            }

            return View(viewModel);
        }

        // POST: /FriendsList/RemoveFriend
        [HttpPost]
        public async Task<IActionResult> RemoveFriend(int friendId)
        {
            //int currentUserId = await _userService.GetCurrentUser();
            int currentUserId = 1;
            await _userService.RemoveFriend(currentUserId, friendId);
            return RedirectToAction("Index");
        }
    }
}