using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Timer = System.Timers.Timer;

namespace LoanShark.Domain
{
    public class Email
    {
        private string emailAddress;

        public Email(string emailAddress)
        {
            if (!IsValid(emailAddress))
            {
                throw new ArgumentException("Invalid email address");
            }
            this.emailAddress = emailAddress;
        }

        // checks if a string is a valid email address
        public static bool IsValid(string emailAddress)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(emailAddress);
                return addr.Address == emailAddress;
            }
            catch
            {
                return false;
            }
        }

        public override string ToString()
        {
            return emailAddress;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }
            return obj is Email email &&
                   emailAddress == email.emailAddress;
        }

        public override int GetHashCode()
        {
            return emailAddress.GetHashCode();
        }
    }

    public class Cnp
    {
        private string cnp;

        public Cnp(string cnp)
        {
            if (!IsValid(cnp))
            {
                throw new ArgumentException("Invalid CNP");
            }
            this.cnp = cnp;
        }

        // checks if a string is a valid cnp
        public static bool IsValid(string cnp)
        {
            if (cnp.Length != 13)
            {
                return false;
            }
            if (!cnp.All(char.IsDigit))
            {
                return false;
            }
            return true;
        }

        public override string ToString()
        {
            return cnp;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }
            return obj is Cnp cnpp &&
                   cnp == cnpp.cnp;
        }

        public override int GetHashCode()
        {
            return cnp.GetHashCode();
        }
    }

    public class PhoneNumber
    {
        private string phoneNumber;

        public PhoneNumber(string phoneNumber)
        {
            if (!IsValid(phoneNumber))
            {
                throw new ArgumentException("Invalid phone number");
            }
            this.phoneNumber = phoneNumber;
        }

        // checks if a string is a valid phone number
        public static bool IsValid(string phoneNumber)
        {
            if (phoneNumber.Length != 10)
            {
                return false;
            }
            if (!phoneNumber.All(char.IsDigit))
            {
                return false;
            }
            if (!phoneNumber.StartsWith("07"))
            {
                return false;
            }
            return true;
        }

        public override string ToString()
        {
            return phoneNumber.ToString();
        }

        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }
            return obj is PhoneNumber phoneNumberr &&
                   phoneNumber == phoneNumberr.phoneNumber;
        }

        public override int GetHashCode()
        {
            return phoneNumber.GetHashCode();
        }
    }

    public class User
    {
        public int UserID { get; set; }
        public Cnp Cnp { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Email Email { get; set; }
        public PhoneNumber PhoneNumber { get; set; }
        public HashedPassword HashedPassword { get; set; }
        public string Username { get; set; }
        public int ReportedCount { get; set; }
        public List<int> Friends { get; set; }
        public List<int> Chats { get; set; }
        private DateTime? TimeoutEnd { get; set; }

        public User()
        {
            this.UserID = 0;
            this.Cnp = new Cnp(string.Empty);
            this.FirstName = string.Empty;
            this.LastName = string.Empty;
            this.Email = new Email(string.Empty);
            this.HashedPassword = new HashedPassword(string.Empty);
            this.Username = string.Empty; // Initialize to a default non-null value
            this.PhoneNumber = new PhoneNumber(string.Empty); // Initialize to a default non-null value
            this.Friends = new List<int>(); // Initialize to an empty list
            this.Chats = new List<int>(); // Initialize to an empty list
        }
        public User(int userID, Cnp cnp, string userName, string firstName, string lastName, Email email, PhoneNumber phoneNumber, HashedPassword hashedPassword)
        {
            UserID = userID;
            Cnp = cnp;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            HashedPassword = hashedPassword;
            this.Username = userName;
            this.ReportedCount = 0;
            this.Friends = new List<int>();
            this.Chats = new List<int>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class with specified parameters.
        /// </summary>
        /// <param name="userId">The unique identifier for the user.</param>
        /// <param name="username">The username of the user.</param>
        /// <param name="phoneNumber">The phone number of the user.</param>
        /// <param name="reportedCount">The number of times the user has been reported.</param>
        public User(int userId, string username, PhoneNumber phoneNumber, int reportedCount)
        {
            this.UserID = userId;
            this.Username = username;
            this.PhoneNumber = phoneNumber;
            this.ReportedCount = reportedCount;
            this.Friends = new List<int>();
            this.Chats = new List<int>();
        }

        /// <summary>
        /// Gets the unique identifier for the user.
        /// </summary>
        /// <returns>The user ID.</returns>
        public int GetUserId()
        {
            return this.UserID;
        }

        /// <summary>
        /// Gets the username of the user.
        /// </summary>
        /// <returns>The username.</returns>
        public string GetUsername()
        {
            return this.Username;
        }

        /// <summary>
        /// Gets the phone number of the user.
        /// </summary>
        /// <returns>The phone number.</returns>
        public string GetPhoneNumber()
        {
            return this.PhoneNumber.ToString();
        }

        /// <summary>
        /// Gets the number of times the user has been reported.
        /// </summary>
        /// <returns>The reported count.</returns>
        public int GetReportedCount()
        {
            return this.ReportedCount;
        }

        /// <summary>
        /// Gets the list of friend IDs associated with the user.
        /// </summary>
        /// <returns>The list of friend IDs.</returns>
        public List<int> GetFriends()
        {
            return this.Friends;
        }

        /// <summary>
        /// Gets the list of chat IDs associated with the user.
        /// </summary>
        /// <returns>The list of chat IDs.</returns>
        public List<int> GetChats()
        {
            return this.Chats;
        }

        /// <summary>
        /// Gets the timeout end time for the user.
        /// </summary>
        /// <returns>The timeout end time, or null if no timeout is set.</returns>
        public DateTime? GetTimeoutEnd() => this.TimeoutEnd;

        /// <summary>
        /// Sets the timeout end time for the user.
        /// </summary>
        /// <param name="timeoutEnd">The timeout end time to set.</param>
        public void SetTimeoutEnd(DateTime? timeoutEnd) => this.TimeoutEnd = timeoutEnd;

        /// <summary>
        /// Increases the report count for the user and applies a timeout if necessary.
        /// </summary>
        /// <returns>A message indicating the user's timeout status.</returns>
        public string IncreaseReportCount()
        {
            this.ReportedCount++;
            System.Diagnostics.Debug.WriteLine($"User {this.Username} report count increased to {this.ReportedCount}");

            string message = string.Empty;
            if (this.ReportedCount >= 1)
            {
                this.SetTimeoutEnd(DateTime.Now.AddMinutes(3));
                System.Diagnostics.Debug.WriteLine($"User {this.Username} set in timeout until {this.TimeoutEnd}");
                this.ResetReportCountAfterDelay();

                message = $"User {this.Username} has been reported {this.ReportedCount} times and is now in timeout until {this.TimeoutEnd?.ToString("HH:mm:ss")}";
            }

            return message;
        }

        /// <summary>
        /// Resets the report count for the user.
        /// </summary>
        public void ResetReportCount()
        {
            this.ReportedCount = 0;
        }

        /// <summary>
        /// Adds a friend to the user's friend list.
        /// </summary>
        /// <param name="friendID">The ID of the friend to add.</param>
        public void AddFriend(int friendID)
        {
            this.Friends.Add(friendID);
        }

        /// <summary>
        /// Removes a friend from the user's friend list.
        /// </summary>
        /// <param name="friendID">The ID of the friend to remove.</param>
        public void RemoveFriend(int friendID)
        {
            this.Friends.Remove(friendID);
        }

        /// <summary>
        /// Adds the user to a chat.
        /// </summary>
        /// <param name="chatID">The ID of the chat to join.</param>
        public void JoinChat(int chatID)
        {
            this.Chats.Add(chatID);
        }

        /// <summary>
        /// Removes the user from a chat.
        /// </summary>
        /// <param name="chatID">The ID of the chat to leave.</param>
        public void LeaveChat(int chatID)
        {
            this.Chats.Remove(chatID);
        }

        /// <summary>
        /// Resets the report count after a delay.
        /// </summary>
        private void ResetReportCountAfterDelay()
        {
            Timer timer = new Timer(3 * 60 * 1000); // 3 minutes in milliseconds
            timer.Elapsed += (sender, e) =>
            {
                this.ResetReportCount();
                timer.Stop();
                timer.Dispose();
            };
            timer.Start();
        }
    }
}
