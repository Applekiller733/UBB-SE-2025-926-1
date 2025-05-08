using LoanShark.Domain;
using LoanShark.Domain.MessageClasses;
using LoanShark.EF.EFModels;
using LoanShark.EF.Mappers;
using Microsoft.Data.SqlClient;
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public void DeleteChat(int chatId)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public void RemoveUserFromChat(int userId, int chatId)
        {
            throw new NotImplementedException();
        }
    }
}
