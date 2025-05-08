using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using LoanShark.API.Models;
using LoanShark.Domain;
using LoanShark.Domain.MessageClasses;
using System.Collections.Generic;
using System.Text;

namespace LoanShark.API.Proxies
{
    public interface IMessageServiceProxy
    {
        Task SendTextMessageAsync(TextMessage message);
        Task SendImageMessageAsync(ImageMessage message);
        Task SendTransferMessageAsync(TransferMessage message);
        Task SendRequestMessageAsync(RequestMessage message);
        Task DeleteMessageAsync(Message message);
        Task ReportMessageAsync(Message message);
        Task<string> GetRepositoryInfoAsync();
    }

    public class MessageServiceProxy : IMessageServiceProxy
    {
        private readonly HttpClient _httpClient;

        public MessageServiceProxy(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task SendTextMessageAsync(TextMessage message)
        {
            var dto = new TextMessageViewModel
            {
                MessageID = message.GetMessageID(),
                SenderID = message.GetSenderID(),
                ChatID = message.GetChatID(),
                Timestamp = message.GetTimestamp().ToString("o"),
                SenderUsername = message.SenderUsername,
                MessageType = "TextMessage",
                Content = message.GetContent(),
                UsersReport = message.GetUsersReport()
            };

            var content = new StringContent(
                JsonSerializer.Serialize(dto),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync("https://localhost:7097/api/Message/text", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task SendImageMessageAsync(ImageMessage message)
        {
            var dto = new ImageMessageViewModel
            {
                MessageID = message.GetMessageID(),
                SenderID = message.GetSenderID(),
                ChatID = message.GetChatID(),
                Timestamp = message.GetTimestamp().ToString("o"),
                SenderUsername = message.SenderUsername,
                MessageType = "ImageMessage",
                ImageURL = message.GetImageURL(),
                UsersReport = message.GetUsersReport()
            };

            var content = new StringContent(
                JsonSerializer.Serialize(dto),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync("https://localhost:7097/api/Message/image", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task SendTransferMessageAsync(TransferMessage message)
        {
            var dto = new TransferMessageViewModel
            {
                MessageID = message.GetMessageID(),
                SenderID = message.GetSenderID(),
                ChatID = message.GetChatID(),
                Timestamp = message.GetTimestamp().ToString("o"),
                SenderUsername = message.SenderUsername,
                MessageType = "TransferMessage",
                Status = message.GetStatus(),
                Amount = message.GetAmount(),
                Description = message.GetDescription(),
                Currency = message.GetCurrency(),
                ListOfReceiversID = message.GetListOfReceiversID()
            };

            var content = new StringContent(
                JsonSerializer.Serialize(dto),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync("https://localhost:7097/api/Message/transfer", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task SendRequestMessageAsync(RequestMessage message)
        {
            var dto = new RequestMessageViewModel
            {
                MessageID = message.GetMessageID(),
                SenderID = message.GetSenderID(),
                ChatID = message.GetChatID(),
                Timestamp = message.GetTimestamp().ToString("o"),
                SenderUsername = message.SenderUsername,
                MessageType = "RequestMessage",
                Status = message.GetStatus(),
                Amount = message.GetAmount(),
                Description = message.GetDescription(),
                Currency = message.GetCurrency()
            };

            var content = new StringContent(
                JsonSerializer.Serialize(dto),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync("https://localhost:7097/api/Message/request", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteMessageAsync(Message message)
        {
            MessageViewModel dto = message switch
            {
                TextMessage text => new TextMessageViewModel
                {
                    MessageID = text.GetMessageID(),
                    SenderID = text.GetSenderID(),
                    ChatID = text.GetChatID(),
                    Timestamp = text.GetTimestamp().ToString("o"),
                    SenderUsername = text.SenderUsername,
                    MessageType = "TextMessage",
                    Content = text.GetContent(),
                    UsersReport = text.GetUsersReport()
                },
                ImageMessage image => new ImageMessageViewModel
                {
                    MessageID = image.GetMessageID(),
                    SenderID = image.GetSenderID(),
                    ChatID = image.GetChatID(),
                    Timestamp = image.GetTimestamp().ToString("o"),
                    SenderUsername = image.SenderUsername,
                    MessageType = "ImageMessage",
                    ImageURL = image.GetImageURL(),
                    UsersReport = image.GetUsersReport()
                },
                TransferMessage transfer => new TransferMessageViewModel
                {
                    MessageID = transfer.GetMessageID(),
                    SenderID = transfer.GetSenderID(),
                    ChatID = transfer.GetChatID(),
                    Timestamp = transfer.GetTimestamp().ToString("o"),
                    SenderUsername = transfer.SenderUsername,
                    MessageType = "TransferMessage",
                    Status = transfer.GetStatus(),
                    Amount = transfer.GetAmount(),
                    Description = transfer.GetDescription(),
                    Currency = transfer.GetCurrency(),
                    ListOfReceiversID = transfer.GetListOfReceiversID()
                },
                RequestMessage request => new RequestMessageViewModel
                {
                    MessageID = request.GetMessageID(),
                    SenderID = request.GetSenderID(),
                    ChatID = request.GetChatID(),
                    Timestamp = request.GetTimestamp().ToString("o"),
                    SenderUsername = request.SenderUsername,
                    MessageType = "RequestMessage",
                    Status = request.GetStatus(),
                    Amount = request.GetAmount(),
                    Description = request.GetDescription(),
                    Currency = request.GetCurrency()
                },
                _ => throw new ArgumentException("Invalid message type.")
            };

            var content = new StringContent(
                JsonSerializer.Serialize(dto),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync("https://localhost:7097/api/Message/delete", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task ReportMessageAsync(Message message)
        {
            MessageViewModel dto = message switch
            {
                TextMessage text => new TextMessageViewModel
                {
                    MessageID = text.GetMessageID(),
                    SenderID = text.GetSenderID(),
                    ChatID = text.GetChatID(),
                    Timestamp = text.GetTimestamp().ToString("o"),
                    SenderUsername = text.SenderUsername,
                    MessageType = "TextMessage",
                    Content = text.GetContent(),
                    UsersReport = text.GetUsersReport()
                },
                ImageMessage image => new ImageMessageViewModel
                {
                    MessageID = image.GetMessageID(),
                    SenderID = image.GetSenderID(),
                    ChatID = image.GetChatID(),
                    Timestamp = image.GetTimestamp().ToString("o"),
                    SenderUsername = image.SenderUsername,
                    MessageType = "ImageMessage",
                    ImageURL = image.GetImageURL(),
                    UsersReport = image.GetUsersReport()
                },
                TransferMessage transfer => new TransferMessageViewModel
                {
                    MessageID = transfer.GetMessageID(),
                    SenderID = transfer.GetSenderID(),
                    ChatID = transfer.GetChatID(),
                    Timestamp = transfer.GetTimestamp().ToString("o"),
                    SenderUsername = transfer.SenderUsername,
                    MessageType = "TransferMessage",
                    Status = transfer.GetStatus(),
                    Amount = transfer.GetAmount(),
                    Description = transfer.GetDescription(),
                    Currency = transfer.GetCurrency(),
                    ListOfReceiversID = transfer.GetListOfReceiversID()
                },
                RequestMessage request => new RequestMessageViewModel
                {
                    MessageID = request.GetMessageID(),
                    SenderID = request.GetSenderID(),
                    ChatID = request.GetChatID(),
                    Timestamp = request.GetTimestamp().ToString("o"),
                    SenderUsername = request.SenderUsername,
                    MessageType = "RequestMessage",
                    Status = request.GetStatus(),
                    Amount = request.GetAmount(),
                    Description = request.GetDescription(),
                    Currency = request.GetCurrency()
                },
                _ => throw new ArgumentException("Invalid message type.")
            };

            var content = new StringContent(
                JsonSerializer.Serialize(dto),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync("https://localhost:7097/api/Message/report", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task<string> GetRepositoryInfoAsync()
        {
            var response = await _httpClient.GetAsync("https://localhost:7097/api/Message/repository");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}