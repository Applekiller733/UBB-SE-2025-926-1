using LoanShark.API.Proxies;
using LoanShark.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoanShark.MVC.Controllers
{
    public class CreateChatController : Controller
    {
        private readonly IChatServiceProxy _chatServiceProxy;

        public CreateChatController(IChatServiceProxy chatServiceProxy)
        {
            _chatServiceProxy = chatServiceProxy;
        }

        public IActionResult Index()
        {
            var viewModel = new CreateChatViewModel();

            // Hardcoded list of mock users
            viewModel.AvailableUsers = new List<UserForChatViewModel>
            {
                new UserForChatViewModel { UserId = 1, Username = "John Doe", IsSelected = false },
                new UserForChatViewModel { UserId = 2, Username = "Jane Smith", IsSelected = false },
                new UserForChatViewModel { UserId = 3, Username = "Michael Johnson", IsSelected = false },
                new UserForChatViewModel { UserId = 4, Username = "Emily Brown", IsSelected = false },
                new UserForChatViewModel { UserId = 5, Username = "David Williams", IsSelected = false }
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateChatViewModel model)
        {
            if (string.IsNullOrEmpty(model.ChatName))
            {
                ModelState.AddModelError("ChatName", "Chat name is required");

                // Re-populate the hardcoded user list
                model.AvailableUsers = GetMockUsers();
                foreach (var user in model.AvailableUsers)
                {
                    user.IsSelected = model.SelectedUserIds?.Contains(user.UserId) ?? false;
                }

                return View("Index", model);
            }

            if (model.SelectedUserIds == null || !model.SelectedUserIds.Any())
            {
                ModelState.AddModelError("SelectedUserIds", "Please select at least one user");

                // Re-populate the hardcoded user list
                model.AvailableUsers = GetMockUsers();

                return View("Index", model);
            }

            try
            {
                // Simulate user ID 1 as the current user
                const int currentUserId = 1;

                // Add current user to participants if not already selected
                var participants = new List<int>(model.SelectedUserIds);
                if (!participants.Contains(currentUserId))
                {
                    participants.Add(currentUserId);
                }

                // Call API to create chat
                await _chatServiceProxy.CreateChat(participants, model.ChatName);

                // Or simply log what would have been created
                System.Diagnostics.Debug.WriteLine($"Creating chat: {model.ChatName}");
                System.Diagnostics.Debug.WriteLine($"Participants: {string.Join(", ", participants)}");

                TempData["SuccessMessage"] = "Chat created successfully!";
                return RedirectToAction("Index", "CreateChat");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Failed to create chat: {ex.Message}";

                // Re-populate the hardcoded user list
                model.AvailableUsers = GetMockUsers();
                foreach (var user in model.AvailableUsers)
                {
                    user.IsSelected = model.SelectedUserIds?.Contains(user.UserId) ?? false;
                }

                return View("Index", model);
            }
        }

        // Helper method to get mock users
        private List<UserForChatViewModel> GetMockUsers()
        {
            return new List<UserForChatViewModel>
            {
                new UserForChatViewModel { UserId = 1, Username = "John Doe", IsSelected = false },
                new UserForChatViewModel { UserId = 2, Username = "Jane Smith", IsSelected = false },
                new UserForChatViewModel { UserId = 3, Username = "Michael Johnson", IsSelected = false },
                new UserForChatViewModel { UserId = 4, Username = "Emily Brown", IsSelected = false },
                new UserForChatViewModel { UserId = 5, Username = "David Williams", IsSelected = false }
            };
        }
    }
}