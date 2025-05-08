using LoanShark.Domain;
using LoanShark.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using LoanShark.EF.EFModels;

namespace LoanShark.EF.Repository.BankRepository
{
    public class UserRepositoryEF : IUserRepository
    {
        private readonly ILoanSharkDbContext _context;

        public UserRepositoryEF(ILoanSharkDbContext context)
        {
            _context = context;
        }

        public async Task<User?> CreateUser(User user)
        {
            try
            {
                var userDTO = new UserEF
                {
                    Cnp = user.Cnp.ToString(),
                    Username = user.Username,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email.ToString(),
                    PhoneNumber = user.PhoneNumber.ToString(),
                    HashedPassword = user.HashedPassword.GetHashedPassword(),
                    ReportedCount = user.ReportedCount,
                    PasswordSalt = user.HashedPassword.GetSalt(),
                };
                _context.User.Add(userDTO);
                await _context.SaveChangesAsync();
                return user;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"EF: Error creating user: {ex.Message}");
                return null;
            }
        }

        public async Task<User?> GetUserById(int userId)
        {
            try
            {

                var userEF = await _context.User.FirstOrDefaultAsync(a => a.UserID == userId) ?? throw new Exception();
                var user = new User
                {
                    UserID = userEF.UserID,
                    Cnp = new Cnp(userEF.Cnp),
                    Username = userEF.Username,
                    FirstName = userEF.FirstName,
                    LastName = userEF.LastName,
                    Email = new Email(userEF.Email),
                    PhoneNumber = new PhoneNumber(userEF.PhoneNumber),
                    HashedPassword = new HashedPassword(userEF.HashedPassword)
                };
                return user;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"EF: Error getting user by ID: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> UpdateUser(User user)
        {
            try
            {
                var userEF = await _context.User.FindAsync(user.UserID);
                if (userEF == null)
                    return false;

                userEF.Cnp = user.Cnp.ToString();
                userEF.Username = user.Username;
                userEF.FirstName = user.FirstName;
                userEF.LastName = user.LastName;
                userEF.Email = user.Email.ToString();
                userEF.PhoneNumber = user.PhoneNumber.ToString();
                userEF.HashedPassword = user.HashedPassword.GetHashedPassword();
                userEF.ReportedCount = user.ReportedCount;
                

                _context.User.Update(userEF);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"EF: Error updating user: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteUser()
        {
            int userId = int.Parse(UserSession.Instance.GetUserData("id_user") ?? "0");
            var user = await _context.User.FindAsync(userId);
            if (user == null)
                return false;

            try
            {
                _context.User.Remove(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"EF: Error deleting user: {ex.Message}");
                return false;
            }
        }

        public async Task<User?> GetUserByCnp(string cnp)
        {
            try
            {
                var userEF = await _context.User.FirstOrDefaultAsync(u => u.Cnp.ToString() == cnp) ?? throw new Exception();
                var user = new User
                {
                    UserID = userEF.UserID,
                    Cnp = new Cnp(userEF.Cnp),
                    Username = userEF.Username,
                    FirstName = userEF.FirstName,
                    LastName = userEF.LastName,
                    Email = new Email(userEF.Email),
                    PhoneNumber = new PhoneNumber(userEF.PhoneNumber),
                    HashedPassword = new HashedPassword(userEF.HashedPassword)
                };
                return user;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"EF: Error getting user by CNP: {ex.Message}");
                return null;
            }

        }

        public async Task<User?> GetUserByEmail(string email)
        {
            try
            {
                var userEF = await _context.User.FirstOrDefaultAsync(u => u.Email.ToString() == email) ?? throw new Exception();
                var user = new User
                {
                    UserID = userEF.UserID,
                    Cnp = new Cnp(userEF.Cnp),
                    Username = userEF.Username,
                    FirstName = userEF.FirstName,
                    LastName = userEF.LastName,
                    Email = new Email(userEF.Email),
                    PhoneNumber = new PhoneNumber(userEF.PhoneNumber),
                    HashedPassword = new HashedPassword(userEF.HashedPassword)
                };
                return user;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"EF: Error getting user by email: {ex.Message}");
                return null;
            }
        }

        public async Task<User?> GetUserByPhoneNumber(string phoneNumber)
        {
            try
            {
                var userEf = await _context.User.FirstOrDefaultAsync(u => u.PhoneNumber.ToString() == phoneNumber) ?? throw new Exception();
                var user = new User
                {
                    UserID = userEf.UserID,
                    Cnp = new Cnp(userEf.Cnp),
                    Username = userEf.Username,
                    FirstName = userEf.FirstName,
                    LastName = userEf.LastName,
                    Email = new Email(userEf.Email),
                    PhoneNumber = new PhoneNumber(userEf.PhoneNumber),
                    HashedPassword = new HashedPassword(userEf.HashedPassword)
                };
                return user;

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"EF: Error getting user by phone number: {ex.Message}");
                return null;
            }
        }

        public async Task<string[]> GetUserPasswordHashSalt()
        {
            int userId = int.Parse(UserSession.Instance.GetUserData("id_user") ?? "0");
            var user = await _context.User.FindAsync(userId);
            if (user == null)
                return Array.Empty<string>();

            return new string[]
            {
                user.HashedPassword.ToString(),
                "", // Salt is not stored in the database, so we return an empty string
            };
        }
    }
}
