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

        public AddNewMemberController(ISocialUserServiceProxy userService, IChatServiceProxy chatService)
        {
            _userService = userService;
            _chatService = chatService;
        }

        public async Task<IActionResult> Index(string? searchQuery, int chatId, List<FriendDTO> newlyAddedFriends = null)
        {
            try
            {
                int currentUserId = await _userService.GetCurrentUser();
                string chatName = await _chatService.GetChatNameByID(chatId) ?? "Unknown Chat";

                var allPotentialUsers = await _userService.GetNonFriendsUsers(currentUserId);
                var currentChatMembers = await _chatService.GetChatParticipantsList(chatId);
                var allUnaddedFriends = allPotentialUsers
                    .Where(f => f != null && !currentChatMembers.Any(p => p?.GetUserId() == f.GetUserId()))
                    .Select(f => new FriendDTO
                    {
                        UserId = f.GetUserId().ToString(),
                        Username = f.Username,
                        PhoneNumber = f.PhoneNumber?.ToString()
                    })
                    .ToList();

                // Use the passed newlyAddedFriends or initialize if null
                newlyAddedFriends = newlyAddedFriends ?? new List<FriendDTO>();

                var unaddedFriends = string.IsNullOrEmpty(searchQuery)
                    ? allUnaddedFriends
                    : allUnaddedFriends
                        .Where(f => f.Username.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                                    (f.PhoneNumber?.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ?? false))
                        .ToList();

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
                    NewlyAddedFriends = newlyAddedFriends,
                    SearchQuery = searchQuery,
                    ChatId = chatId
                };

                return View(viewModel);
            }
            catch (HttpRequestException ex)
            {
                TempData["AlertMessage"] = $"Error loading data: {ex.Message}. Using defaults.";
                return View(new AddNewMemberViewModel
                {
                    ChatName = "Unknown Chat",
                    CurrentChatMembers = new List<FriendDTO>(),
                    UnaddedFriends = new List<FriendDTO>(),
                    NewlyAddedFriends = newlyAddedFriends ?? new List<FriendDTO>(),
                    SearchQuery = searchQuery,
                    ChatId = chatId
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddToSelected(string userId, int chatId, List<FriendDTO> newlyAddedFriends)
        {
            var allUnaddedFriends = await GetUnaddedFriends(chatId); // Await the task to get the result
            var friend = allUnaddedFriends.Where(f => f.UserId == userId).FirstOrDefault(); // Use FirstOrDefault to avoid exceptions
            if (friend != null && !newlyAddedFriends.Any(f => f.UserId == userId))
            {
                newlyAddedFriends.Add(friend);
                TempData["AlertMessage"] = $"Added {friend.Username} to selection.";
            }
            else
            {
                TempData["AlertMessage"] = "Friend not found or already added.";
            }

            return RedirectToAction("Index", new { chatId, newlyAddedFriends });
        }

        [HttpPost]
        public IActionResult RemoveFromSelected(string userId, int chatId, List<FriendDTO> newlyAddedFriends)
        {
            var friend = newlyAddedFriends.Where(f => f.UserId == userId).First();
            if (friend != null)
            {
                newlyAddedFriends.Remove(friend);
                TempData["AlertMessage"] = $"Removed {friend.Username} from selection.";
            }

            return RedirectToAction("Index", new { chatId, newlyAddedFriends });
        }

        [HttpPost]
        public async Task<IActionResult> AddUsersToChat(int chatId, List<FriendDTO> newlyAddedFriends)
        {
            try
            {
                foreach (var friend in newlyAddedFriends)
                {
                    await _chatService.AddUserToChat(int.Parse(friend.UserId), chatId);
                }

                newlyAddedFriends.Clear();
                TempData["AlertMessage"] = "New members added to chat successfully!";
                return RedirectToAction("Messages", "ChatMessages", new { chatId });
            }
            catch (Exception ex)
            {
                TempData["AlertMessage"] = $"Error adding members: {ex.Message}";
                return RedirectToAction("Index", new { chatId, newlyAddedFriends });
            }
        }

        private async Task<List<FriendDTO>> GetUnaddedFriends(int chatId)
        {
            int currentUserId = await _userService.GetCurrentUser();
            var allPotentialUsers = await _userService.GetNonFriendsUsers(currentUserId);
            var currentChatMembers = await _chatService.GetChatParticipantsList(chatId);
            return allPotentialUsers
                .Where(f => f != null && !currentChatMembers.Any(p => p?.GetUserId() == f.GetUserId()))
                .Select(f => new FriendDTO
                {
                    UserId = f.GetUserId().ToString(),
                    Username = f.Username,
                    PhoneNumber = f.PhoneNumber?.ToString()
                })
                .ToList();
        }
    }
}