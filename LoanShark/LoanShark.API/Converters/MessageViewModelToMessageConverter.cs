using LoanShark.API.Models;
using LoanShark.Domain.MessageClasses;
using Newtonsoft.Json.Linq;
//using Newtonsoft.Json;
using LoanShark.Domain.Enums;
using LoanShark.API.JSONConverters;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace LoanShark.API.Converters
{
    public class MessageViewModelToMessageConverter
    {
        public List<Message> Convert(List<MessageViewModel> viewModels)
        {
            if (viewModels == null)
            {
                return new List<Message>();
            }

            var messages = new List<Message>();

            foreach (var viewModel in viewModels)
            {
                // Serialize the view model to JSON to use JObject for dynamic property access
                //var json = JsonConvert.SerializeObject(viewModel);

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                    Converters = { new MessageViewModelConverter() }
                };

                string json = JsonSerializer.Serialize(viewModel, options);

                var jObject = JObject.Parse(json);

                // Get the messageType field (case-insensitive)
                var messageType = jObject["messageType"]?.ToString();
                if (string.IsNullOrEmpty(messageType))
                {
                    throw new JsonException($"The 'messageType' property is missing for message ID {viewModel.MessageID}.");
                }

                Message message = messageType.ToLower() switch
                {
                    "text" => CreateTextMessage(viewModel, jObject),
                    "image" => CreateImageMessage(viewModel, jObject),
                    "transfer" => CreateTransferMessage(viewModel, jObject),
                    "request" => CreateRequestMessage(viewModel, jObject),
                    _ => throw new JsonException($"Unknown messageType: {messageType}. Expected one of: Text, Image, Transfer, Request.")
                };

                messages.Add(message);
            }

            return messages;
        }

        private TextMessage CreateTextMessage(MessageViewModel viewModel, JObject jObject)
        {
            var content = jObject["content"]?.ToString() ?? string.Empty;
            var usersReport = jObject["usersReport"]?.ToObject<List<int>>() ?? new List<int>();

            return new TextMessage(
                messageID: viewModel.MessageID,
                senderID: viewModel.SenderID,
                chatID: viewModel.ChatID,
                timestamp: DateTime.Parse(viewModel.Timestamp),
                content: content,
                usersReport: usersReport
            )
            {
                SenderUsername = viewModel.SenderUsername,
                MessageType = MessageType.Text
            };
        }

        private ImageMessage CreateImageMessage(MessageViewModel viewModel, JObject jObject)
        {
            var imageUrl = jObject["imageURL"]?.ToString() ?? string.Empty;
            var usersReport = jObject["usersReport"]?.ToObject<List<int>>() ?? new List<int>();

            return new ImageMessage(
                messageID: viewModel.MessageID,
                senderID: viewModel.SenderID,
                chatID: viewModel.ChatID,
                timestamp: DateTime.Parse(viewModel.Timestamp),
                imageUrl: imageUrl,
                usersReport: usersReport
            )
            {
                SenderUsername = viewModel.SenderUsername,
                MessageType = MessageType.Image
            };
        }

        private TransferMessage CreateTransferMessage(MessageViewModel viewModel, JObject jObject)
        {
            var status = jObject["status"]?.ToString() ?? string.Empty;
            var amount = jObject["amount"]?.ToObject<float>() ?? 0f;
            var description = jObject["description"]?.ToString() ?? string.Empty;
            var currency = jObject["currency"]?.ToString() ?? string.Empty;
            var listOfReceiversId = jObject["listOfReceiversID"]?.ToObject<List<int>>() ?? new List<int>();

            return new TransferMessage(
                messageID: viewModel.MessageID,
                senderID: viewModel.SenderID,
                chatID: viewModel.ChatID,
                timestamp: DateTime.Parse(viewModel.Timestamp),
                status: status,
                amount: amount,
                desc: description,
                currency: currency,
                listOfReceiversId: listOfReceiversId
            )
            {
                SenderUsername = viewModel.SenderUsername,
                MessageType = MessageType.Transfer
            };
        }

        private RequestMessage CreateRequestMessage(MessageViewModel viewModel, JObject jObject)
        {
            var status = jObject["status"]?.ToString() ?? string.Empty;
            var amount = jObject["amount"]?.ToObject<float>() ?? 0f;
            var description = jObject["description"]?.ToString() ?? string.Empty;
            var currency = jObject["currency"]?.ToString() ?? string.Empty;

            return new RequestMessage(
                messageID: viewModel.MessageID,
                requesterID: viewModel.SenderID,
                chatID: viewModel.ChatID,
                timestamp: DateTime.Parse(viewModel.Timestamp),
                status: status,
                amount: amount,
                desc: description,
                currency: currency
            )
            {
                SenderUsername = viewModel.SenderUsername,
                MessageType = MessageType.Request
            };
        }
    }
}
