using LoanShark.Domain;
using LoanShark.Domain.MessageClasses;
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
        public const int LOGGEDINUSERID = 2;
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
            throw new NotImplementedException();
        }

        public void AddImageMessage(int userId, int chatId, string imageURL)
        {
            throw new NotImplementedException();
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

        public void AddReport(int messageId, string reason, string description, string status)
        {
            throw new NotImplementedException();
        }

        public void AddRequestMessage(int userId, int chatId, string content, string? status = null, float? amount = null, string? currency = null)
        {
            throw new NotImplementedException();
        }

        public void AddTextMessage(int userId, int chatId, string content)
        {
            throw new NotImplementedException();
        }

        public void AddTransferMessage(int userId, int chatId, string content, string? status = null, float? amount = null, string? currency = null)
        {
            throw new NotImplementedException();
        }

        public void AddUserToChat(int userId, int chatId)
        {
            throw new NotImplementedException();
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

        public void DeleteFriend(int userId, int friendId)
        {
            throw new NotImplementedException();
        }

        public void DeleteMessage(int messageId)
        {
            throw new NotImplementedException();
        }

        public void DeleteNotification(int notificationId)
        {
            throw new NotImplementedException();
        }

        public Chat? GetChatById(int chatId)
        {
            throw new NotImplementedException();
        }

        public List<User> GetChatParticipants(int chatId)
        {
            throw new NotImplementedException();
        }

        public List<int> GetChatParticipantsIDs(int chatId)
        {
            throw new NotImplementedException();
        }

        public List<int> GetChatsIDs(int userId)
        {
            throw new NotImplementedException();
        }

        public List<Chat> GetChatsList()
        {
            throw new NotImplementedException();
        }

        public List<Post> GetFeedPostsList()
        {
            return this.loanSharkDbContext.Post.Select(post => PostMapper.ToDomainPost(post)).ToList();
        }

        public List<int> GetFriendsIDs(int userId)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
