using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using LoanShark.Service.SocialService.Implementations;
using LoanShark.Domain;
using LoanShark.API.Models;
using LoanShark.EF.Repository.SocialRepository;
using LoanShark.Service.SocialService.Interfaces;
using LoanShark.Domain.MessageClasses;
using System.Collections.Generic;

namespace LoanShark.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly MessageService _messageService;

        public MessageController()
        {
            IRepository repository = new Repository();
            INotificationService notificationService = new NotificationService(repository);
            IUserService userService = new UserService(repository, notificationService);
            _messageService = new MessageService(repository, userService);
        }

        [HttpPost("text")]
        public async Task<ActionResult> SendTextMessage([FromBody] TextMessageViewModel messageDto)
        {
            UserSession.Instance.SetUserData("id_user", "1"); // Hardcoded for now
            if (messageDto == null || string.IsNullOrEmpty(messageDto.Content))
                return BadRequest("Message content is required.");

            _messageService.SendMessage(messageDto.SenderID, messageDto.ChatID, messageDto.Content);
            return NoContent();
        }

        [HttpPost("image")]
        public async Task<ActionResult> SendImageMessage([FromBody] ImageMessageViewModel messageDto)
        {
            UserSession.Instance.SetUserData("id_user", "1"); // Hardcoded for now
            if (messageDto == null || string.IsNullOrEmpty(messageDto.ImageURL))
                return BadRequest("Image URL is required.");

            _messageService.SendImage(messageDto.SenderID, messageDto.ChatID, messageDto.ImageURL);
            return NoContent();
        }

        [HttpPost("transfer")]
        public async Task<ActionResult> SendTransferMessage([FromBody] TransferMessageViewModel messageDto)
        {
            UserSession.Instance.SetUserData("id_user", "1"); // Hardcoded for now
            if (messageDto == null || string.IsNullOrEmpty(messageDto.Description) || string.IsNullOrEmpty(messageDto.Currency))
                return BadRequest("Transfer details are required.");

            _messageService.SendTransferMessage(
                messageDto.SenderID,
                messageDto.ChatID,
                messageDto.Description,
                messageDto.Status,
                messageDto.Amount,
                messageDto.Currency);
            return NoContent();
        }

        [HttpPost("request")]
        public async Task<ActionResult> SendRequestMessage([FromBody] RequestMessageViewModel messageDto)
        {
            UserSession.Instance.SetUserData("id_user", "1"); // Hardcoded for now
            if (messageDto == null || string.IsNullOrEmpty(messageDto.Description) || string.IsNullOrEmpty(messageDto.Currency))
                return BadRequest("Request details are required.");

            _messageService.SendRequestMessage(
                messageDto.SenderID,
                messageDto.ChatID,
                messageDto.Description,
                messageDto.Status,
                messageDto.Amount,
                messageDto.Currency);
            return NoContent();
        }

        [HttpPost("delete")]
        public async Task<ActionResult> DeleteMessage([FromBody] MessageViewModel messageDto)
        {
            UserSession.Instance.SetUserData("id_user", "1"); // Hardcoded for now
            if (messageDto == null)
                return BadRequest("Message data is required.");

            Message message = messageDto.MessageType switch
            {
                "TextMessage" => new TextMessage(
                    messageDto.MessageID,
                    messageDto.SenderID,
                    messageDto.ChatID,
                    DateTime.Parse(messageDto.Timestamp),
                    (messageDto as TextMessageViewModel)?.Content ?? string.Empty,
                    (messageDto as TextMessageViewModel)?.UsersReport ?? new List<int>()),
                "ImageMessage" => new ImageMessage(
                    messageDto.MessageID,
                    messageDto.SenderID,
                    messageDto.ChatID,
                    DateTime.Parse(messageDto.Timestamp),
                    (messageDto as ImageMessageViewModel)?.ImageURL ?? string.Empty,
                    (messageDto as ImageMessageViewModel)?.UsersReport ?? new List<int>()),
                "TransferMessage" => new TransferMessage(
                    messageDto.MessageID,
                    messageDto.SenderID,
                    messageDto.ChatID,
                    DateTime.Parse(messageDto.Timestamp),
                    (messageDto as TransferMessageViewModel)?.Status ?? string.Empty,
                    (messageDto as TransferMessageViewModel)?.Amount ?? 0f,
                    (messageDto as TransferMessageViewModel)?.Description ?? string.Empty,
                    (messageDto as TransferMessageViewModel)?.Currency ?? string.Empty),
                "RequestMessage" => new RequestMessage(
                    messageDto.MessageID,
                    messageDto.SenderID,
                    messageDto.ChatID,
                    DateTime.Parse(messageDto.Timestamp),
                    (messageDto as RequestMessageViewModel)?.Status ?? string.Empty,
                    (messageDto as RequestMessageViewModel)?.Amount ?? 0f,
                    (messageDto as RequestMessageViewModel)?.Description ?? string.Empty,
                    (messageDto as RequestMessageViewModel)?.Currency ?? string.Empty),
                _ => throw new ArgumentException("Invalid message type.")
            };

            _messageService.DeleteMessage(message);
            return NoContent();
        }

        [HttpPost("report")]
        public async Task<ActionResult> ReportMessage([FromBody] MessageViewModel messageDto)
        {
            UserSession.Instance.SetUserData("id_user", "1"); // Hardcoded for now
            if (messageDto == null)
                return BadRequest("Message data is required.");

            Message message = messageDto.MessageType switch
            {
                "TextMessage" => new TextMessage(
                    messageDto.MessageID,
                    messageDto.SenderID,
                    messageDto.ChatID,
                    DateTime.Parse(messageDto.Timestamp),
                    (messageDto as TextMessageViewModel)?.Content ?? string.Empty,
                    (messageDto as TextMessageViewModel)?.UsersReport ?? new List<int>()),
                "ImageMessage" => new ImageMessage(
                    messageDto.MessageID,
                    messageDto.SenderID,
                    messageDto.ChatID,
                    DateTime.Parse(messageDto.Timestamp),
                    (messageDto as ImageMessageViewModel)?.ImageURL ?? string.Empty,
                    (messageDto as ImageMessageViewModel)?.UsersReport ?? new List<int>()),
                "TransferMessage" => new TransferMessage(
                    messageDto.MessageID,
                    messageDto.SenderID,
                    messageDto.ChatID,
                    DateTime.Parse(messageDto.Timestamp),
                    (messageDto as TransferMessageViewModel)?.Status ?? string.Empty,
                    (messageDto as TransferMessageViewModel)?.Amount ?? 0f,
                    (messageDto as TransferMessageViewModel)?.Description ?? string.Empty,
                    (messageDto as TransferMessageViewModel)?.Currency ?? string.Empty),
                "RequestMessage" => new RequestMessage(
                    messageDto.MessageID,
                    messageDto.SenderID,
                    messageDto.ChatID,
                    DateTime.Parse(messageDto.Timestamp),
                    (messageDto as RequestMessageViewModel)?.Status ?? string.Empty,
                    (messageDto as RequestMessageViewModel)?.Amount ?? 0f,
                    (messageDto as RequestMessageViewModel)?.Description ?? string.Empty,
                    (messageDto as RequestMessageViewModel)?.Currency ?? string.Empty),
                _ => throw new ArgumentException("Invalid message type.")
            };

            _messageService.ReportMessage(message);
            return NoContent();
        }

        [HttpGet("repository")]
        public async Task<ActionResult<string>> GetRepositoryInfo()
        {
            UserSession.Instance.SetUserData("id_user", "1"); // Hardcoded for now
            var repo = _messageService.GetRepo();
            return Ok($"Repository Type: {repo.GetType().Name}");
        }
    }
}