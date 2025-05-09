﻿using LoanShark.Domain;
using LoanShark.Domain.Enums;
using LoanShark.Domain.MessageClasses;
using LoanShark.EF.EfModels;
using LoanShark.EF.EFModels;
using LoanShark.EF.Mappers;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanShark.EF.Repository.SocialRepository
{
    /// <summary>
    /// Represents a repository for accessing and managing data.
    /// </summary>
    public class RepositoryEF : IRepository
    {
        /// <summary>
        /// The ID of the currently logged-in user.
        /// </summary>
        public const int LOGGEDINUSERID = 1;
        private static int loggedInUserID = LOGGEDINUSERID;
        private readonly ILoanSharkDbContext loanSharkDbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryEF"/> class.
        /// </summary>
        public RepositoryEF(ILoanSharkDbContext loanSharkDbContext)
        {
            this.loanSharkDbContext = loanSharkDbContext;
        }

        public int AddChat(string chatName)
        {
            var chatEF = new ChatEF
            {
                ChatName = chatName,
            };

            this.loanSharkDbContext.Chat.Add(chatEF);
            this.loanSharkDbContext.SaveChangesAsync();

            return chatEF.Id;
        }

        public void AddFriend(int userId, int friendId)
        {
            var friendshipEF = new FriendshipEF
            {
                UserId = userId,
                FriendId = friendId,
            };

            this.loanSharkDbContext.Friendship
                .Add(friendshipEF);

            this.loanSharkDbContext.SaveChangesAsync();
        }

        //get the ef instance for this enum
        private MessageTypeEF GetMessageTypeEFByTypeName(MessageType messageType)
        {
            return this.loanSharkDbContext.MessageType
                .First(message => message.TypeName.Equals(messageType.ToString()));
        }

        public void AddImageMessage(int userId, int chatId, string imageURL)
        {
            var messageTypeEF = this.GetMessageTypeEFByTypeName(MessageType.Image);

            //i think???
            var imageMessageEF = new MessageEF
            {
                TypeID = messageTypeEF.TypeId,
                UserID = userId,
                ChatID = chatId,
                ImageUrl = imageURL,
            };

            this.loanSharkDbContext.Message.Add(imageMessageEF);
            this.loanSharkDbContext.SaveChangesAsync();
        }

        public void AddNotification(string content, int userId)
        {
            var notificationEF = new NotificationEF
            {
                Content = content,
                Timestamp = DateTime.UtcNow,
                UserReceiverID = userId,
            };

            this.loanSharkDbContext.Notification.Add(notificationEF);
            this.loanSharkDbContext.SaveChangesAsync();
        }

        public async void AddReport(int messageId, string reason, string description, string status)
        {
            var reportEF = new ReportEF
            {
                MessageID = messageId,
                ReporterUserID = this.GetLoggedInUserID(),  // cred???
                Status = status,
                Reason = reason,
                Description = description,
            };

            this.loanSharkDbContext.Report
                .Add(reportEF);

            await this.loanSharkDbContext.SaveChangesAsync();
        }

        public async void AddRequestMessage(int userId, int chatId, string content, string? status = null, float? amount = null, string? currency = null)
        {
            var messageTypeEF = this.GetMessageTypeEFByTypeName(MessageType.Request);

            var requestMessageEF = new MessageEF
            {
                TypeID = messageTypeEF.TypeId,
                UserID = userId,
                ChatID = chatId,
                Status = status,
                Content = content,
                Amount = amount,
                Currency = currency,
            };

            this.loanSharkDbContext.Message
                .Add(requestMessageEF);

            await this.loanSharkDbContext.SaveChangesAsync();
        }

        public void AddTextMessage(int userId, int chatId, string content)
        {
            var messageTypeEF = this.GetMessageTypeEFByTypeName(MessageType.Text);

            var textMessageEF = new MessageEF
            {
                TypeID = messageTypeEF.TypeId,
                UserID = userId,
                ChatID = chatId,
                Content = content,
            };

            this.loanSharkDbContext.Message
                .Add(textMessageEF);

            this.loanSharkDbContext.SaveChangesAsync();
        }

        public async void AddTransferMessage(int userId, int chatId, string content, string? status = null, float? amount = null, string? currency = null)
        {
            var messageTypeEF = this.GetMessageTypeEFByTypeName(MessageType.Transfer);

            var transferMessageEF = new MessageEF
            {
                UserID = userId,
                ChatID = chatId,
                Status = status,
                Amount = amount,
                Content = content,
                Currency = currency,
            };

            this.loanSharkDbContext.Message
                .Add(transferMessageEF);
            
            await this.loanSharkDbContext.SaveChangesAsync();
        }

        public void AddUserToChat(int userId, int chatId)
        {
            var chatUserEF = new ChatUserEF
            {
                ChatId = chatId,
                UserId = userId,
            };

            this.loanSharkDbContext.ChatUser
                .Add(chatUserEF);

            this.loanSharkDbContext.SaveChangesAsync();
        }

        public void ClearAllNotifications(int userId)
        {
            this.loanSharkDbContext.Notification
                .Where(notification => notification.UserReceiverID == userId)
                .ExecuteDelete();
            this.loanSharkDbContext.SaveChangesAsync();
        }

        public void DeleteChat(int chatId)
        {
            var chatEF = this.loanSharkDbContext.Chat
                .Find(chatId);

            this.loanSharkDbContext.Chat.Remove(chatEF);
            this.loanSharkDbContext.SaveChangesAsync();
        }

        public async void DeleteFriend(int userId, int friendId)
        {
            this.loanSharkDbContext.Friendship
                .Where(friendship => friendship.UserId == userId && friendship.FriendId == friendId)
                .ExecuteDelete();

            await this.loanSharkDbContext.SaveChangesAsync();
        }

        public async void DeleteMessage(int messageId)
        {
            this.loanSharkDbContext.Message
                .Where(message => message.MessageID == messageId)
                .ExecuteDelete();

            await this.loanSharkDbContext.SaveChangesAsync();
        }

        public void DeleteNotification(int notificationId)
        {
            this.loanSharkDbContext.Notification
                .Where(notification => notification.NotificationID == notificationId)
                .ExecuteDelete();

            this.loanSharkDbContext.SaveChangesAsync();
        }

        public Chat? GetChatById(int chatId)
        {
            ChatEF chatEF = this.loanSharkDbContext.Chat.Find(chatId);
            if (chatEF == null)
            {
                return null;
            }
            return ChatMapper.ToDomainChat(chatEF);
        }

        public List<User> GetChatParticipants(int chatId)
        {
            return this.loanSharkDbContext.ChatUser
                .Where(chatUser => chatUser.ChatId == chatId)
                .Select(chatUser => UserMapper.ToDomainUser(chatUser.User))
                .ToList();
        }

        public List<int> GetChatParticipantsIDs(int chatId)
        {
            // ids of users from a chat
            return this.loanSharkDbContext.ChatUser
                .Where(chatUser => chatUser.ChatId == chatId)
                .Select(chatUser => chatUser.UserId)
                .ToList();
        }

        public List<int> GetChatsIDs(int userId)
        {
            // chat ids a user is part of
            return this.loanSharkDbContext.ChatUser
                .Where(chatUser => chatUser.UserId == userId)
                .Select(chatUser => chatUser.ChatId)
                .ToList();
        }

        public List<Chat> GetChatsList()
        {
            return this.loanSharkDbContext.Chat
                .Select(chat => ChatMapper.ToDomainChat(chat))
                .ToList();
        }

        public List<Post> GetFeedPostsList()
        {
            return this.loanSharkDbContext.Post.Select(post => PostMapper.ToDomainPost(post)).ToList();
        }

        public List<int> GetFriendsIDs(int userId)
        {
            return this.loanSharkDbContext.Friendship
                .Where(friendship => friendship.UserId == userId)
                .Select(friendship => friendship.FriendId)
                .ToList();
        }


        /// <summary>
        /// Gets the ID of the currently logged-in user.
        /// </summary>
        /// <returns>The ID of the logged-in user.</returns>
        public int GetLoggedInUserID()
        {
            return loggedInUserID;
        }

        public List<Message> GetMessagesList()
        {
            var messages = this.loanSharkDbContext.Message.ToList();
            List<Message> messagesList = new List<Message>();

            foreach (var message in messages)
            {
                if (message.MessageType.TypeName.Equals(MessageType.Image.ToString()))
                {
                    messagesList.Add(MessageMapper.ToDomainImageMessage(message));
                }
                else if (message.MessageType.TypeName.Equals(MessageType.Text.ToString()))
                {
                    messagesList.Add(MessageMapper.ToDomainTextMessage(message));
                }
                else if (message.MessageType.TypeName.Equals(MessageType.Transfer.ToString()))
                {
                    messagesList.Add(MessageMapper.ToDomainTransferMessage(message));
                }
                else
                {
                    // if it is a request
                    messagesList.Add(MessageMapper.ToDomainRequestMessage(message));
                }
            }

            return messagesList;
        }


        public List<Notification> GetNotifications(int userId)
        {

            var notifications = this.loanSharkDbContext.Notification
                .Where(notification => notification.UserReceiverID == userId)
                .ToList();

            return notifications
                .Select(notification => NotificationMapper.ToDomainNotification(notification))
                .ToList();
        }

        public List<Report> GetReportsList()
        {
            return this.loanSharkDbContext.Report
                .Select(report => ReportMapper.ToDomainReport(report))
                .ToList();
        }

        /// <summary>
        /// Retrieves a user by their ID.
        /// </summary>
        /// <param name="userID">The ID of the user to retrieve.</param>
        /// <returns>A <see cref="User"/> object representing the user.</returns>
        public User? GetUserById(int userID)
        {
            UserEF? userEntity = this.loanSharkDbContext.User.Find(userID);
            if (userEntity == null)
            {
                return null;
            }
            return UserMapper.ToDomainUser(userEntity);
        }

        public List<User> GetUserFriendsList(int userId)
        {
            return this.loanSharkDbContext.Friendship
                .Where(friendship => friendship.UserId == userId)
                .Select(friendship => UserMapper.ToDomainUser(friendship.Friend))
                .ToList();
        }

        public List<User> GetUsersList()
        {
            //converts every entity to the domain object
            return this.loanSharkDbContext.User
                .Select(userEntity => UserMapper.ToDomainUser(userEntity))
                .ToList();
        }

        public void RemoveUserFromChat(int userId, int chatId)
        {
            this.loanSharkDbContext.ChatUser
                .Where(chatUser => chatUser.UserId == userId && chatUser.ChatId == chatId)
                .ExecuteDelete();
            this.loanSharkDbContext.SaveChangesAsync();
        }
    }
}
