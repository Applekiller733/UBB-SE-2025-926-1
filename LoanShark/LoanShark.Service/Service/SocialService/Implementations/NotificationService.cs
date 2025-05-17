// <copyright file="NotificationService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright> --------------------------------------------------------------------------------------------------------------------

namespace LoanShark.Service.SocialService.Implementations
{
    using System.Collections.Generic;
    using LoanShark.EF.Repository.SocialRepository;
    using LoanShark.Domain;
    using LoanShark.Service.SocialService.Interfaces;
    using NotificationModel = LoanShark.Domain.Notification;
    using WindowsNotification = Windows.UI.Notifications.Notification;

    /// <summary>
    /// Provides notification-related services such as sending and clearing notifications.
    /// </summary>
    public class NotificationService : INotificationService
    {
        private readonly IRepository repo;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationService"/> class.
        /// </summary>
        /// <param name="repo">The repository used for data access.</param>
        public NotificationService(IRepository repo)
        {
            this.repo = repo;
        }

        /// <summary>
        /// Retrieves a list of notifications for a specific user.
        /// </summary>
        /// <param name="userID">The ID of the user.</param>
        /// <returns>A list of notifications.</returns>
        public async Task<List<NotificationModel>> GetNotifications(int userID)
        {
            return await this.repo.GetNotifications(userID);
        }

        /// <summary>
        /// Sends a notification to a user and their new friend when a friendship is created.
        /// </summary>
        /// <param name="userID">The ID of the user.</param>
        /// <param name="newFriendID">The ID of the new friend.</param>
        public async Task SendFriendNotification(int userID, int newFriendID)
        {
            var user = await this.repo.GetUserById(userID);
            var newFriend = await this.repo.GetUserById(newFriendID);

            if (user != null && newFriend != null)
            {
                string userContent = $"You added user {newFriend.GetUsername()} ({newFriend.GetPhoneNumber()}) to your friend list.";
                string friendContent = $"User {user.GetUsername()} ({user.GetPhoneNumber()}) added you as a friend.";
                await this.repo.AddNotification(userContent, userID);
                await this.repo.AddNotification(friendContent, newFriendID);
            }
        }

        /// <summary>
        /// Sends a notification to a user and their old friend when a friendship is removed.
        /// </summary>
        /// <param name="userID">The ID of the user.</param>
        /// <param name="oldFriendID">The ID of the old friend.</param>
        public async Task SendRemoveFriendNotification(int userID, int oldFriendID)
        {
            var user = await this.repo.GetUserById(userID);
            var oldFriend = await this.repo.GetUserById(oldFriendID);

            if (user != null && oldFriend != null)
            {
                string userContent = $"You removed user {oldFriend.GetUsername()} ({oldFriend.GetPhoneNumber()}) from your friend list.";
                string friendContent = $"User {user.GetUsername()} ({user.GetPhoneNumber()}) deleted you from their friend list. So selfish!";
                await this.repo.AddNotification(userContent, userID);
                await this.repo.AddNotification(friendContent, oldFriendID);
            }
        }

        /// <summary>
        /// Sends a notification to all participants in a chat when a message is sent.
        /// </summary>
        /// <param name="messageSenderID">The ID of the message sender.</param>
        /// <param name="chatID">The ID of the chat.</param>
        public async Task SendMessageNotification(int messageSenderID, int chatID)
        {
            var sender = await this.repo.GetUserById(messageSenderID);
            var chat = await this.repo.GetChatById(chatID);
            var participants = await this.repo.GetChatParticipants(chatID);

            if (sender != null && chat != null)
            {
                string content = $"User {sender.GetUsername()} sent a message in chat {chat.getChatName()}.";
                foreach (var participant in participants)
                {
                    if (participant.GetUserId() != messageSenderID)
                    {
                        await this.repo.AddNotification(content, participant.GetUserId());
                    }
                }
            }
        }

        /// <summary>
        /// Sends a transaction notification to a user and all participants in a chat.
        /// </summary>
        /// <param name="receiverID">The ID of the receiver.</param>
        /// <param name="chatID">The ID of the chat.</param>
        /// <param name="type">The type of transaction.</param>
        /// <param name="amount">The amount of the transaction.</param>
        /// <param name="currency">The currency of the transaction.</param>
        public async Task SendTransactionNotification(int receiverID, int chatID, string type, float amount, string currency)
        {
            var moneyReceiver = await this.repo.GetUserById(receiverID);
            var chat = await this.repo.GetChatById(chatID);
            var participants = await this.repo.GetChatParticipants(chatID);

            if (moneyReceiver != null && chat != null)
            {
                string senderContent = $"You requested {amount} {currency} in {chat.getChatName()} group.";
                await this.repo.AddNotification(senderContent, receiverID);

                string participantContent = $"User {moneyReceiver.GetUsername()} requested {amount} {currency} in {chat.getChatName()} group.";
                foreach (var participant in participants)
                {
                    if (participant.GetUserId() != receiverID)
                    {
                        await this.repo.AddNotification(participantContent, participant.GetUserId());
                    }
                }
            }
        }

        /// <summary>
        /// Sends a notification to all participants when a new chat is created.
        /// </summary>
        /// <param name="chatID">The ID of the chat.</param>
        public async Task SendNewChatNotification(int chatID)
        {
            var participants = await this.repo.GetChatParticipants(chatID);
            foreach (var participant in participants)
            {
                string content = $"You have been added to a new chat {chatID}.";
                await this.repo.AddNotification(content, participant.GetUserId());
            }
        }

        /// <summary>
        /// Clears a specific notification.
        /// </summary>
        /// <param name="notificationID">The ID of the notification to clear.</param>
        public async Task ClearNotification(int notificationID)
        {
            await this.repo.DeleteNotification(notificationID);
        }

        /// <summary>
        /// Clears all notifications for a specific user.
        /// </summary>
        /// <param name="userID">The ID of the user.</param>
        public async Task ClearAllNotifications(int userID)
        {
            await this.repo.ClearAllNotifications(userID);
        }
    }
}