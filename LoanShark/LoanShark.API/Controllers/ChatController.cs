using Microsoft.AspNetCore.Mvc;
using LoanShark.Service.SocialService.Implementations;
using LoanShark.Domain;
using System.Collections.Generic;
using LoanShark.Domain.MessageClasses;
using LoanShark.API.Models;
using LoanShark.Service.SocialService.Interfaces;

namespace LoanShark.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IChatService chatService;

        public ChatController(IChatService chatService)
        {
            this.chatService = chatService;
        }

        [HttpGet("current-user-id")]
        public ActionResult<int> GetCurrentUserID()
        {
            return Ok(chatService.GetCurrentUserID());
        }

        [HttpGet("{chatId}/participants/count")]
        public ActionResult<int> GetNumberOfParticipants(int chatId)
        {
            return Ok(chatService.GetNumberOfParticipants(chatId));
        }

        [HttpPost("request-money")]
        public IActionResult RequestMoney([FromBody] RequestMoneyDto dto)
        {
            try
            {
                chatService.RequestMoneyViaChat(dto.Amount, dto.Currency, dto.ChatID, dto.Description);
                return Ok("Request sent.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("send-money")]
        public IActionResult SendMoney([FromBody] SendMoneyDto dto)
        {
            try
            {
                chatService.SendMoneyViaChat(dto.Amount, dto.Currency, dto.Description, dto.ChatID);
                return Ok("Transfer processed.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("accept-request")]
        public IActionResult AcceptRequest([FromBody] AcceptRequestDto dto)
        {
            chatService.AcceptRequestViaChat(dto.Amount, dto.Currency, dto.AccepterID, dto.RequesterID, dto.ChatID);
            return Ok("Request handled.");
        }

        [HttpPost("create-chat")]
        public IActionResult CreateChat([FromBody] CreateChatDto dto)
        {
            chatService.CreateChat(dto.ParticipantsID, dto.ChatName);
            return Ok("Chat created.");
        }

        [HttpDelete("{chatId}")]
        public IActionResult DeleteChat(int chatId)
        {
            chatService.DeleteChat(chatId);
            return Ok("Chat deleted.");
        }

        [HttpGet("{chatId}/last-message-time")]
        public ActionResult<DateTime> GetLastMessageTimestamp(int chatId)
        {
            return Ok(chatService.GetLastMessageTimeStamp(chatId));
        }

        [HttpGet("{chatId}/history")]
        public ActionResult<List<Message>> GetChatHistory(int chatId)
        {
            return Ok(chatService.GetChatHistory(chatId));
        }

        [HttpPost("{chatId}/add-user/{userId}")]
        public IActionResult AddUserToChat(int chatId, int userId)
        {
            chatService.AddUserToChat(userId, chatId);
            return Ok("User added to chat.");
        }

        [HttpDelete("{chatId}/remove-user/{userId}")]
        public IActionResult RemoveUserFromChat(int chatId, int userId)
        {
            chatService.RemoveUserFromChat(userId, chatId);
            return Ok("User removed from chat.");
        }

        [HttpGet("{chatId}/name")]
        public ActionResult<string> GetChatNameById(int chatId)
        {
            try
            {
                return Ok(chatService.GetChatNameByID(chatId));
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{chatId}/participants/usernames")]
        public ActionResult<List<string>> GetParticipantUsernames(int chatId)
        {
            return Ok(chatService.GetChatParticipantsStringList(chatId));
        }

        [HttpGet("{chatId}/participants")]
        public ActionResult<List<User>> GetParticipants(int chatId)
        {
            return Ok(chatService.GetChatParticipantsList(chatId));
        }
    }
}