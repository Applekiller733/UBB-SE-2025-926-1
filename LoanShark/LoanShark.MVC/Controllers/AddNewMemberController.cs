using LoanShark.API.Proxies;
using LoanShark.MVC.Models;
using LoanShark.Service.SocialService.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoanShark.MVC.Controllers
{
    public class AddNewMemberController : Controller
    {
        private readonly ISocialUserServiceProxy _userService;
        private readonly IChatServiceProxy _chatService;
        private List<FriendDTO> _newlyAddedFriends;
        private List<FriendDTO> _allUnaddedFriends;

        public AddNewMemberController(ISocialUserServiceProxy userService, IChatServiceProxy chatService)
        {
            _userService = userService;
            _chatService = chatService;
            _newlyAddedFriends = new List<FriendDTO>();
            _allUnaddedFriends = new List<FriendDTO>();
        }

        public async Task<IActionResult> Index(string? searchQuery, int chatId = 2)
        {
            try
            {
                int currentUserId = await _userService.GetCurrentUser();
                string chatName = await _chatService.GetChatNameByID(chatId) ?? "Unknown Chat";

                // Load all unadded friends (using GetNonFriendsUsers for broader eligibility)
                var allPotentialUsers = await _userService.GetNonFriendsUsers(currentUserId);
                var currentChatMembers = await _chatService.GetChatParticipantsList(chatId);
                _allUnaddedFriends = allPotentialUsers
                    .Where(f => f != null && !currentChatMembers.Any(p => p?.GetUserId() == f.GetUserId()))
                    .Select(f => new FriendDTO
                    {
                        UserId = f.GetUserId().ToString(),
                        Username = f.Username,
                        PhoneNumber = f.PhoneNumber?.ToString()
                    })
                    .ToList();
                Console.WriteLine($"Loaded {_allUnaddedFriends.Count} unadded friends for chat {chatId}");

                // Filter unadded friends
                var unaddedFriends = string.IsNullOrEmpty(searchQuery)
                    ? _allUnaddedFriends
                    : _allUnaddedFriends
                        .Where(f => f.Username.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                                    (f.PhoneNumber?.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ?? false))
                        .ToList();

                // Get current chat members
                var currentChatMembersDto = currentChatMembers
                    .Select(p => new FriendDTO
                    {
                        UserId = p.GetUserId().ToString(),
                        Username = p.Username,
                        PhoneNumber = p.PhoneNumber?.ToString()
                    })
                    .ToList();

                var viewModel = new AddNewMemberViewModel
                {
                    ChatName = chatName,
                    CurrentChatMembers = currentChatMembersDto,
                    UnaddedFriends = unaddedFriends,
                    NewlyAddedFriends = _newlyAddedFriends,
                    SearchQuery = searchQuery
                };

                return View(viewModel);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error in Index: {ex.Message}");
                TempData["AlertMessage"] = $"Error loading data: {ex.Message}. Using defaults.";
                return View(new AddNewMemberViewModel
                {
                    ChatName = "Unknown Chat",
                    CurrentChatMembers = new List<FriendDTO>(),
                    UnaddedFriends = new List<FriendDTO>(),
                    NewlyAddedFriends = _newlyAddedFriends,
                    SearchQuery = searchQuery
                });
            }
        }

        [HttpPost]
        public IActionResult AddToSelected(string userId)
        {
            var friend = _allUnaddedFriends.FirstOrDefault(f => f.UserId == userId);
            if (friend != null && !_newlyAddedFriends.Any(f => f.UserId == userId))
            {
                _newlyAddedFriends.Add(friend);
                TempData["AlertMessage"] = $"Added {friend.Username} to selection.";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult RemoveFromSelected(string userId)
        {
            var friend = _newlyAddedFriends.FirstOrDefault(f => f.UserId == userId);
            if (friend != null)
            {
                _newlyAddedFriends.Remove(friend);
                TempData["AlertMessage"] = $"Removed {friend.Username} from selection.";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddUsersToChat(int chatId = 2)
        {
            foreach (var friend in _newlyAddedFriends)
            {
                await _chatService.AddUserToChat(int.Parse(friend.UserId), chatId);
            }

            _newlyAddedFriends.Clear();
            TempData["AlertMessage"] = "New members added to chat successfully!";

            return RedirectToAction("Index", "MainPage");
        }
    }
}