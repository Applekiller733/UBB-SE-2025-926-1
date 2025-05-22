using Microsoft.AspNetCore.Mvc;
using LoanShark.API.Proxies;
using LoanShark.Domain;
using LoanShark.MVC.Models;
using System.Threading.Tasks;

namespace LoanShark.MVC.Controllers
{
    public class ChatListController : Controller
    {
        private readonly IChatServiceProxy _chatService;
        private readonly ISocialUserServiceProxy _userService;

        public ChatListController(IChatServiceProxy chatService, ISocialUserServiceProxy userService)
        {
            _chatService = chatService;
            _userService = userService;
        }

        // GET: /ChatList/
        public async Task<IActionResult> Index(string searchQuery = "")
        {
            var viewModel = new ChatListViewModel { SearchQuery = searchQuery };
            var chats = await _userService.GetCurrentUserChats();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                chats = chats.Where(c => c.getChatName().Contains(searchQuery, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            viewModel.ChatList = chats;
            return View(viewModel);
        }

        //// GET: /ChatList/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //// POST: /ChatList/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(string chatName, List<int> participantIds)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        await _chatService.CreateChat(participantIds, chatName);
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View();
        //}
    }
}