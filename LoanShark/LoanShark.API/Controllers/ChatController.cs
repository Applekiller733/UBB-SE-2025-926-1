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
        public async Task<ActionResult<int>> GetNumberOfParticipants(int chatId)
        {
            return Ok(await chatService.GetNumberOfParticipants(chatId));
        }

        [HttpPost("request-money")]
        public async Task<IActionResult> RequestMoney([FromBody] RequestMoneyDto dto)
        {
            try
            {
                await chatService.RequestMoneyViaChat(dto.Amount, dto.Currency, dto.ChatID, dto.Description);
                return Ok("Request sent.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("send-money")]
        public async Task<IActionResult> SendMoney([FromBody] SendMoneyDto dto)
        {
            try
            {
                await chatService.SendMoneyViaChat(dto.Amount, dto.Currency, dto.Description, dto.ChatID);
                return Ok("Transfer processed.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("accept-request")]
        public async Task<IActionResult> AcceptRequest([FromBody] AcceptRequestDto dto)
        {
            await chatService.AcceptRequestViaChat(dto.Amount, dto.Currency, dto.AccepterID, dto.RequesterID, dto.ChatID);
            return Ok("Request handled.");
        }

        [HttpPost("create-chat")]
        public async Task<IActionResult> CreateChat([FromBody] CreateChatDto dto)
        {
            await chatService.CreateChat(dto.ParticipantsID, dto.ChatName);
            return Ok("Chat created.");
        }

        [HttpDelete("{chatId}")]
        public async Task<IActionResult> DeleteChat(int chatId)
        {
            await chatService.DeleteChat(chatId);
            return Ok("Chat deleted.");
        }

        [HttpGet("{chatId}/last-message-time")]
        public async Task<ActionResult<DateTime>> GetLastMessageTimestamp(int chatId)
        {
            return Ok(await chatService.GetLastMessageTimeStamp(chatId));
        }

        [HttpGet("{chatId}/history")]
        public async Task<ActionResult<List<Message>>> GetChatHistory(int chatId)
        {
            return Ok(await chatService.GetChatHistory(chatId));
        }

        [HttpPost("{chatId}/add-user/{userId}")]
        public async Task<IActionResult> AddUserToChat(int chatId, int userId)
        {
            await chatService.AddUserToChat(userId, chatId);
            return Ok("User added to chat.");
        }

        [HttpDelete("{chatId}/remove-user/{userId}")]
        public async Task<IActionResult> RemoveUserFromChat(int chatId, int userId)
        {
            await chatService.RemoveUserFromChat(userId, chatId);
            return Ok("User removed from chat.");
        }

        [HttpGet("{chatId}/name")]
        public async Task<ActionResult<string>> GetChatNameById(int chatId)
        {
            try
            {
                return Ok(await chatService.GetChatNameByID(chatId));
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{chatId}/participants/usernames")]
        public async Task<ActionResult<List<string>>> GetParticipantUsernames(int chatId)
        {
            return Ok(await chatService.GetChatParticipantsStringList(chatId));
        }

        [HttpGet("{chatId}/participants")]
        public async Task<ActionResult<List<User>>> GetParticipants(int chatId)
        {
            return Ok(await chatService.GetChatParticipantsList(chatId));
        }
    }
}