using LoanShark.API.Proxies;
using LoanShark.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LoanShark.MVC.Controllers
{
    public class LeaveChatController : Controller
    {
        private readonly IChatServiceProxy _chatServiceProxy;

        public LeaveChatController(IChatServiceProxy chatServiceProxy)
        {
            _chatServiceProxy = chatServiceProxy;
        }

        public async Task<IActionResult> Index(int chatId)
        {
            try
            {
                // Get chat name
                string chatName = await _chatServiceProxy.GetChatNameByID(chatId);

                var viewModel = new LeaveChatViewModel
                {
                    ChatId = chatId,
                    ChatName = chatName
                };

                return View(viewModel);
            }
            catch
            {
                // Maybe change redirections here too
                TempData["ErrorMessage"] = "Failed to load chat details.";
                return RedirectToAction("ListChats", "CreateChat");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Confirm(int chatId)
        {
            try
            {
                // Get current user ID
                int currentUserId = int.Parse(HttpContext.Session.GetString("id_user") ?? "1");

                // Remove user from chat
                await _chatServiceProxy.RemoveUserFromChat(currentUserId, chatId);


                // Change redirection after all pages are done
                TempData["SuccessMessage"] = "You have successfully left the chat.";
                return RedirectToAction("ListChats", "CreateChat");
            }
            catch
            {
                // here too maybe
                TempData["ErrorMessage"] = "Failed to leave the chat.";
                return RedirectToAction("Index", new { chatId });
            }
        }

        public IActionResult Cancel(int chatId)
        {
            // Change redirection after all pages are done
            return RedirectToAction("ViewChat", "Chat", new { id = chatId });
        }
    }
}