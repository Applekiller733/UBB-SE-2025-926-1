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
        Task SendMessage(int senderID, int chatID, string content);

        Task SendImage(int senderID, int chatID, string imageURL);

        Task SendTransferMessage(int userID, int chatID, string content, string status, float amount, string currency);

        Task SendRequestMessage(int userID, int chatID, string content, string status, float amount, string currency);

        Task DeleteMessage(Message message);

        Task ReportMessage(Message message); // does nothing

        Task<string> GetRepositoryInfo();
    }

    public class MessageServiceProxy : IMessageServiceProxy
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://localhost:7097/api/Message";

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageServiceProxy"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client for making API requests.</param>
        public MessageServiceProxy(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        /// <summary>
        /// Asynchronously sends a text message to a chat via HTTP.
        /// </summary>
        /// <param name="senderID">The ID of the sender.</param>
        /// <param name="chatID">The ID of the chat.</param>
        /// <param name="content">The content of the message.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task SendMessage(int senderID, int chatID, string content)
        {
            var dto = new TextMessageViewModel
            {
                SenderID = senderID,
                ChatID = chatID,
                Content = content,
                MessageType = "TextMessage",
                Timestamp = DateTime.UtcNow.ToString("o"),
                UsersReport = new List<int>()
            };

            var jsonContent = new StringContent(
                JsonSerializer.Serialize(dto),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync($"{_baseUrl}/text", jsonContent);
            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Asynchronously sends an image message to a chat via HTTP.
        /// </summary>
        /// <param name="senderID">The ID of the sender.</param>
        /// <param name="chatID">The ID of the chat.</param>
        /// <param name="imageURL">The URL of the image.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task SendImage(int senderID, int chatID, string imageURL)
        {
            var dto = new ImageMessageViewModel
            {
                SenderID = senderID,
                ChatID = chatID,
                ImageURL = imageURL,
                MessageType = "ImageMessage",
                Timestamp = DateTime.UtcNow.ToString("o"),
                UsersReport = new List<int>()
            };

            var jsonContent = new StringContent(
                JsonSerializer.Serialize(dto),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync($"{_baseUrl}/image", jsonContent);
            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Asynchronously sends a transfer message to a chat via HTTP.
        /// </summary>
        /// <param name="userID">The ID of the user sending the message.</param>
        /// <param name="chatID">The ID of the chat.</param>
        /// <param name="content">The content of the message.</param>
        /// <param name="status">The status of the transfer.</param>
        /// <param name="amount">The amount being transferred.</param>
        /// <param name="currency">The currency of the transfer.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task SendTransferMessage(int userID, int chatID, string content, string status, float amount, string currency)
        {
            var dto = new TransferMessageViewModel
            {
                SenderID = userID,
                ChatID = chatID,
                Description = content,
                Status = status,
                Amount = amount,
                Currency = currency,
                MessageType = "TransferMessage",
                Timestamp = DateTime.UtcNow.ToString("o"),
                ListOfReceiversID = new List<int>()
            };

            var jsonContent = new StringContent(
                JsonSerializer.Serialize(dto),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync($"{_baseUrl}/transfer", jsonContent);
            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Asynchronously sends a request message to a chat via HTTP.
        /// </summary>
        /// <param name="userID">The ID of the user sending the message.</param>
        /// <param name="chatID">The ID of the chat.</param>
        /// <param name="content">The content of the message.</param>
        /// <param name="status">The status of the request.</param>
        /// <param name="amount">The amount being requested.</param>
        /// <param name="currency">The currency of the request.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task SendRequestMessage(int userID, int chatID, string content, string status, float amount, string currency)
        {
            var dto = new RequestMessageViewModel
            {
                SenderID = userID,
                ChatID = chatID,
                Description = content,
                Status = status,
                Amount = amount,
                Currency = currency,
                MessageType = "RequestMessage",
                Timestamp = DateTime.UtcNow.ToString("o")
            };

            var jsonContent = new StringContent(
                JsonSerializer.Serialize(dto),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync($"{_baseUrl}/request", jsonContent);
            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Asynchronously deletes a message via HTTP.
        /// </summary>
        /// <param name="message">The message to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task DeleteMessage(Message message)
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
                    UsersReport = text.GetUsersReport() ?? new List<int>()
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
                    UsersReport = image.GetUsersReport() ?? new List<int>()
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
                    ListOfReceiversID = transfer.GetListOfReceiversID() ?? new List<int>()
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

            var jsonContent = new StringContent(
                JsonSerializer.Serialize(dto),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync($"{_baseUrl}/delete", jsonContent);
            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Asynchronously reports a message via HTTP.
        /// </summary>
        /// <param name="message">The message to report.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task ReportMessage(Message message)
        {
            //MessageViewModel dto = message switch
            //{
            //    TextMessage text => new TextMessageViewModel
            //    {
            //        MessageID = text.GetMessageID(),
            //        SenderID = text.GetSenderID(),
            //        ChatID = text.GetChatID(),
            //        Timestamp = text.GetTimestamp().ToString("o"),
            //        SenderUsername = text.SenderUsername,
            //        MessageType = "TextMessage",
            //        Content = text.GetContent(),
            //        UsersReport = text.GetUsersReport() ?? new List<int>()
            //    },
            //    ImageMessage image => new ImageMessageViewModel
            //    {
            //        MessageID = image.GetMessageID(),
            //        SenderID = image.GetSenderID(),
            //        ChatID = image.GetChatID(),
            //        Timestamp = image.GetTimestamp().ToString("o"),
            //        SenderUsername = image.SenderUsername,
            //        MessageType = "ImageMessage",
            //        ImageURL = image.GetImageURL(),
            //        UsersReport = image.GetUsersReport() ?? new List<int>()
            //    },
            //    TransferMessage transfer => new TransferMessageViewModel
            //    {
            //        MessageID = transfer.GetMessageID(),
            //        SenderID = transfer.GetSenderID(),
            //        ChatID = transfer.GetChatID(),
            //        Timestamp = transfer.GetTimestamp().ToString("o"),
            //        SenderUsername = transfer.SenderUsername,
            //        MessageType = "TransferMessage",
            //        Status = transfer.GetStatus(),
            //        Amount = transfer.GetAmount(),
            //        Description = transfer.GetDescription(),
            //        Currency = transfer.GetCurrency(),
            //        ListOfReceiversID = transfer.GetListOfReceiversID() ?? new List<int>()
            //    },
            //    RequestMessage request => new RequestMessageViewModel
            //    {
            //        MessageID = request.GetMessageID(),
            //        SenderID = request.GetSenderID(),
            //        ChatID = request.GetChatID(),
            //        Timestamp = request.GetTimestamp().ToString("o"),
            //        SenderUsername = request.SenderUsername,
            //        MessageType = "RequestMessage",
            //        Status = request.GetStatus(),
            //        Amount = request.GetAmount(),
            //        Description = request.GetDescription(),
            //        Currency = request.GetCurrency()
            //    },
            //    _ => throw new ArgumentException("Invalid message type.")
            //};

            //var jsonContent = new StringContent(
            //    JsonSerializer.Serialize(dto),
            //    Encoding.UTF8,
            //    "application/json");

            //var response = await _httpClient.PostAsync($"{_baseUrl}/report", jsonContent);
            //response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Asynchronously gets information about the repository via HTTP.
        /// </summary>
        /// <returns>A task that returns a string with repository information.</returns>
        public async Task<string> GetRepositoryInfo()
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/repository");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}