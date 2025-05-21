// <copyright file="Chat.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace LoanShark.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using LoanShark.Domain.MessageClasses;

    /// <summary>
    /// Represents a chat with a unique ID, name, and a list of user IDs.
    /// </summary>
    public class Chat
    {
        private List<int> userIDs;
        private int chatID;
        private string chatName;

        public List<int> UserIDs { get; set; }
        
        public int ChatID { get; set; }

        public string ChatName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Chat"/> class.
        /// </summary>
        /// <param name="chatID">The unique identifier for the chat.</param>
        /// <param name="chatName">The name of the chat.</param>
        /// <param name="userIds">The list of user IDs in the chat.</param>
        public Chat(int chatID, string chatName, List<int> userIds)
        {
            this.chatID = chatID;
            this.userIDs = userIds;
            this.chatName = chatName;
        }

        /// <summary>
        /// Gets the unique identifier for the chat.
        /// </summary>
        /// <returns>The chat ID.</returns>
        public int getChatID()
        {
            return this.chatID;
        }

        /// <summary>
        /// Gets the list of user IDs in the chat.
        /// </summary>
        /// <returns>The list of user IDs.</returns>
        public List<int> getUserIDsList()
        {
            return this.userIDs;
        }

        /// <summary>
        /// Gets the name of the chat.
        /// </summary>
        /// <returns>The chat name.</returns>
        public string getChatName()
        {
            return this.chatName;
        }

        /// <summary>
        /// Gets the number of users in the chat.
        /// </summary>
        /// <returns>The user count.</returns>
        public int getUserCount()
        {
            return this.userIDs.Count;
        }

        /// <summary>
        /// Adds a user to the chat.
        /// </summary>
        /// <param name="userID">The ID of the user to add.</param>
        public void AddUser(int userID)
        {
            this.userIDs.Add(userID);
        }

        /// <summary>
        /// Removes a user from the chat.
        /// </summary>
        /// <param name="userID">The ID of the user to remove.</param>
        public void RemoveUser(int userID)
        {
            this.userIDs.Remove(userID);
        }

        /// <summary>
        /// Checks if a user is in the chat.
        /// </summary>
        /// <param name="userID">The ID of the user to check.</param>
        /// <returns>True if the user is in the chat; otherwise, false.</returns>
        public bool IsUserInChat(int userID)
        {
            return this.userIDs.Contains(userID);
        }

        /// <summary>
        /// Returns the name of the chat as a string.
        /// </summary>
        /// <returns>The chat name.</returns>
        public override string ToString()
        {
            return this.chatName;
        }
    }
}