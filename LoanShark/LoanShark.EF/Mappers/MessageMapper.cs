using LoanShark.Domain.MessageClasses;
using LoanShark.EF.EfModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LoanShark.EF.Mappers
{
    public static class MessageMapper
    {
        public static ImageMessage ToDomainImageMessage(MessageEF messageEF)
        {
            if (messageEF == null)
            {
                throw new ArgumentNullException(nameof(messageEF));
            }

            if (messageEF.MessageType.TypeName != Domain.Enums.MessageType.Image)
            {
                throw new Exception("The message is not of Image type!");
            }

            if (string.IsNullOrEmpty(messageEF.ImageUrl))
            {
                throw new InvalidOperationException("ImageUrl is required for ImageMessage");
            }

            // if everything ok
            // deserialize UsersReport -> get the list of users who reported
            List<int> usersReport = string.IsNullOrEmpty(messageEF.SerializedUserIDs)
                ? new List<int>()
                : JsonSerializer.Deserialize<List<int>>(messageEF.SerializedUserIDs)
                  ?? new List<int>();

            return new ImageMessage(messageEF.MessageID, messageEF.UserID, messageEF.ChatID, messageEF.Timestamp, messageEF.ImageUrl, usersReport);
        }

        public static RequestMessage ToDomainRequestMessage(MessageEF messageEF)
        {
            if (messageEF == null)
            {
                throw new ArgumentNullException(nameof(messageEF));
            }

            // if everything ok
            return new RequestMessage(messageEF.MessageID, messageEF.UserID, messageEF.ChatID, messageEF.Timestamp, messageEF.Status, (float)messageEF.Amount, messageEF.Content, messageEF.Currency);
        }

        public static TextMessage ToDomainTextMessage(MessageEF messageEF)
        {
            if (messageEF == null)
            {
                throw new ArgumentNullException(nameof(messageEF));
            }

            // if everything ok
            // deserialize UsersReport -> get the list of users who reported
            List<int> usersReport = string.IsNullOrEmpty(messageEF.SerializedUserIDs)
                ? new List<int>()
                : JsonSerializer.Deserialize<List<int>>(messageEF.SerializedUserIDs)
                  ?? new List<int>();

            return new TextMessage(messageEF.MessageID, messageEF.UserID, messageEF.ChatID, messageEF.Timestamp, messageEF.Content, usersReport);
        }

        public static TransferMessage ToDomainTransferMessage(MessageEF messageEF)
        {
            if (messageEF == null)
            {
                throw new ArgumentNullException(nameof(messageEF));
            }

            // if everything ok
            // deserialize UsersIds -> get the list of users who received the transfer
            List<int> usersTransferedTo = string.IsNullOrEmpty(messageEF.SerializedUserIDs)
                ? new List<int>()
                : JsonSerializer.Deserialize<List<int>>(messageEF.SerializedUserIDs)
                  ?? new List<int>();

            return new TransferMessage(messageEF.MessageID, messageEF.UserID, messageEF.ChatID, messageEF.Timestamp, messageEF.Status, (float)messageEF.Amount, messageEF.Content, messageEF.Currency, usersTransferedTo);
        }
    }
}
