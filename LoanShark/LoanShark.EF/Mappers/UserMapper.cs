using LoanShark.Domain;
using LoanShark.EF.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanShark.EF.Mappers
{
    public static class UserMapper
    {
        public static User ToDomainUser(UserEF userEF)
        {
            if (userEF == null)
            {
                throw new ArgumentNullException(nameof(userEF));
            }

            try
            {
                // Map basic properties
                var user = new User
                {
                    UserID = userEF.UserID,
                    Cnp = new Cnp(userEF.Cnp),
                    FirstName = userEF.FirstName,
                    LastName = userEF.LastName,
                    Email = new Email(userEF.Email),
                    PhoneNumber = new PhoneNumber(userEF.PhoneNumber),
                    Username = userEF.Username,
                    ReportedCount = userEF.ReportedCount,
                    HashedPassword = new HashedPassword(userEF.HashedPassword, userEF.PasswordSalt, false),
                    Friends = new List<int>(),
                    Chats = new List<int>(),
                };

                // Handle TimeoutEnd
                if (!string.IsNullOrEmpty(userEF.TimeoutEnd) && DateTime.TryParse(userEF.TimeoutEnd, out var timeoutEnd))
                {
                    user.SetTimeoutEnd(timeoutEnd);
                }
                else
                {
                    user.SetTimeoutEnd(null);
                }

                // Deserialize FriendsSerialized (assuming comma-separated string)
                if (!string.IsNullOrEmpty(userEF.FriendsSerialized))
                {
                    user.Friends = userEF.FriendsSerialized
                        .Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(int.Parse)
                        .ToList();
                }

                // Deserialize ChatsSerialized (assuming comma-separated string)
                if (!string.IsNullOrEmpty(userEF.ChatsSerialized))
                {
                    user.Chats = userEF.ChatsSerialized
                        .Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(int.Parse)
                        .ToList();
                }

                return user;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to convert UserEF to User: " + ex.Message, ex);
            }
        }
    }
}
