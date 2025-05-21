using Microsoft.AspNetCore.Mvc;
using LoanShark.API.Proxies;
using LoanShark.Domain;
using LoanShark.Domain.MessageClasses;
using LoanShark.MVC.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace LoanShark.MVC.Controllers
{
    public class ChatMessagesController : Controller
    {
        private readonly IChatServiceProxy _chatService;
        private readonly ISocialUserServiceProxy _userService;
        private readonly IMessageServiceProxy _messageService;
        private readonly IReportServiceProxy _reportService;
        private readonly ImgurImageUploader _imgurUploader;
        private readonly ILogger<ChatMessagesController> _logger;

        public ChatMessagesController(
            IChatServiceProxy chatService,
            ISocialUserServiceProxy userService,
            IMessageServiceProxy messageService,
            IReportServiceProxy reportService,
            ImgurImageUploader imgurUploader,
            ILogger<ChatMessagesController> logger)
        {
            _chatService = chatService;
            _userService = userService;
            _messageService = messageService;
            _reportService = reportService;
            _imgurUploader = imgurUploader;
            _logger = logger;
        }

        // GET: /ChatMessages/Messages/{chatId}
        public async Task<IActionResult> Messages(int chatId)
        {
            try
            {
                _logger.LogInformation("Loading messages for chat ID {ChatId}", chatId);
                var currentUserId = await _userService.GetCurrentUser();
                var chatName = await _chatService.GetChatNameByID(chatId);
                var participants = await _chatService.GetChatParticipantsStringList(chatId);
                var messages = await _chatService.GetChatHistory(chatId);

                foreach (var message in messages)
                {
                    var user = await _userService.GetUserById(message.GetSenderID());
                    message.SenderUsername = user?.GetUsername() ?? "Unknown";
                }

                var viewModel = new ChatMessagesViewModel
                {
                    CurrentChatID = chatId,
                    CurrentChatName = chatName,
                    CurrentChatParticipants = participants,
                    ChatMessages = messages,
                    CurrentUserID = currentUserId
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading messages for chat ID {ChatId}", chatId);
                TempData["Error"] = "Failed to load chat messages. Please try again.";
                return RedirectToAction("Index", "ChatList");
            }
        }

        // POST: /ChatMessages/SendMessage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendMessage(int chatId, string messageContent)
        {
            try
            {
                if (string.IsNullOrEmpty(messageContent) || messageContent.Length > 256)
                {
                    TempData["Error"] = "Message must be between 1 and 256 characters.";
                    return RedirectToAction("Messages", new { chatId });
                }

                var currentUserId = await _userService.GetCurrentUser();
                await _messageService.SendMessage(currentUserId, chatId, messageContent);
                TempData["Success"] = "Message sent successfully.";
                return RedirectToAction("Messages", new { chatId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending message to chat ID {ChatId}", chatId);
                TempData["Error"] = "Failed to send message. Please try again.";
                return RedirectToAction("Messages", new { chatId });
            }
        }

        // POST: /ChatMessages/SendImage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendImage(int chatId, IFormFile imageFile)
        {
            try
            {
                if (imageFile == null || imageFile.Length == 0)
                {
                    TempData["Error"] = "Please select an image to upload.";
                    return RedirectToAction("Messages", new { chatId });
                }

                string imageUrl = await _imgurUploader.UploadImageAndGetUrl(imageFile);
                var currentUserId = await _userService.GetCurrentUser();
                await _messageService.SendImage(currentUserId, chatId, imageUrl);
                TempData["Success"] = "Image sent successfully.";
                return RedirectToAction("Messages", new { chatId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending image to chat ID {ChatId}", chatId);
                TempData["Error"] = ex.Message; // Use the exception message from ImgurImageUploader
                return RedirectToAction("Messages", new { chatId });
            }
        }

        // POST: /ChatMessages/DeleteMessage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMessage(int chatId, int messageId)
        {
            try
            {
                var messages = await _chatService.GetChatHistory(chatId);
                var message = messages.FirstOrDefault(m => m.GetMessageID() == messageId);
                if (message == null)
                {
                    TempData["Error"] = "Message not found.";
                    return RedirectToAction("Messages", new { chatId });
                }

                await _messageService.DeleteMessage(message);
                TempData["Success"] = "Message deleted successfully.";
                return RedirectToAction("Messages", new { chatId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting message {MessageId} in chat ID {ChatId}", messageId, chatId);
                TempData["Error"] = "Failed to delete message. Please try again.";
                return RedirectToAction("Messages", new { chatId });
            }
        }

        //// GET: /ChatMessages/ReportMessage/{chatId}/{messageId}
        //public IActionResult ReportMessage(int chatId, int messageId)
        //{
        //    try
        //    {
        //        ViewBag.ChatId = chatId;
        //        ViewBag.MessageId = messageId;
        //        return View();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error rendering ReportMessage view for message ID {MessageId} in chat ID {ChatId}", messageId, chatId);
        //        TempData["Error"] = "Failed to load report page. Please try again.";
        //        return RedirectToAction("Messages", new { chatId });
        //    }
        //}

        //// POST: /ChatMessages/ReportMessageConfirm
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ReportMessageConfirm(int chatId, int messageId, string reason, string description)
        //{
        //    try
        //    {
        //        var currentUserId = await _userService.GetCurrentUser();
        //        var report = new Report(messageId, currentUserId, "Pending", reason, description);
        //        await _reportService.AddReport(report);
        //        TempData["Success"] = "Message reported successfully.";
        //        return RedirectToAction("Messages", new { chatId });
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error reporting message {MessageId} in chat ID {ChatId}", messageId, chatId);
        //        TempData["Error"] = "Failed to report message. Please try again.";
        //        return RedirectToAction("Messages", new { chatId });
        //    }
        //}

        //// GET: /ChatMessages/AddMember/{chatId}
        //public async Task<IActionResult> AddMember(int chatId)
        //{
        //    try
        //    {
        //        var nonFriends = await _userService.GetNonFriendsUsers(await _userService.GetCurrentUser());
        //        ViewBag.ChatId = chatId;
        //        return View(nonFriends);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error rendering AddMember view for chat ID {ChatId}", chatId);
        //        TempData["Error"] = "Failed to load add member page. Please try again.";
        //        return RedirectToAction("Messages", new { chatId });
        //    }
        //}

        //// POST: /ChatMessages/AddMemberConfirm
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> AddMemberConfirm(int chatId, int userId)
        //{
        //    try
        //    {
        //        await _chatService.AddUserToChat(userId, chatId);
        //        TempData["Success"] = "Member added successfully.";
        //        return RedirectToAction("Messages", new { chatId });
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error adding user {UserId} to chat ID {ChatId}", userId, chatId);
        //        TempData["Error"] = "Failed to add member. Please try again.";
        //        return RedirectToAction("Messages", new { chatId });
        //    }
        //}

        //// GET: /ChatMessages/LeaveChat/{chatId}
        //[HttpGet]
        //public IActionResult LeaveChat(int chatId)
        //{
        //    try
        //    {
        //        ViewBag.ChatId = chatId;
        //        return View();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error rendering LeaveChat view for chat ID {ChatId}", chatId);
        //        TempData["Error"] = "Failed to load leave chat page. Please try again.";
        //        return RedirectToAction("Messages", new { chatId });
        //    }
        //}

        //// POST: /ChatMessages/LeaveChatConfirm
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> LeaveChatConfirm(int chatId)
        //{
        //    try
        //    {
        //        var currentUserId = await _userService.GetCurrentUser();
        //        await _userService.LeaveChat(currentUserId, chatId);
        //        TempData["Success"] = "You have successfully left the chat.";
        //        return RedirectToAction("Index", "ChatList");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error leaving chat ID {ChatId}", chatId);
        //        TempData["Error"] = "Failed to leave the chat. Please try again.";
        //        return RedirectToAction("Messages", new { chatId });
        //    }
        //}

        //// GET: /ChatMessages/GenerateTransfer/{chatId}
        //public IActionResult GenerateTransfer(int chatId)
        //{
        //    try
        //    {
        //        ViewBag.ChatId = chatId;
        //        return View();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error rendering GenerateTransfer view for chat ID {ChatId}", chatId);
        //        TempData["Error"] = "Failed to load transfer page. Please try again.";
        //        return RedirectToAction("Messages", new { chatId });
        //    }
        //}

        //// POST: /ChatMessages/GenerateTransferConfirm
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> GenerateTransferConfirm(int chatId, float amount, string currency, string description)
        //{
        //    try
        //    {
        //        var currentUserId = await _userService.GetCurrentUser();
        //        await _messageService.SendTransferMessage(currentUserId, chatId, description, "Pending", amount, currency);
        //        TempData["Success"] = "Transfer sent successfully.";
        //        return RedirectToAction("Messages", new { chatId });
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error sending transfer in chat ID {ChatId}", chatId);
        //        TempData["Error"] = "Failed to send transfer. Please try again.";
        //        return RedirectToAction("Messages", new { chatId });
        //    }
        //}
    }
}