using LoanShark.Domain;

namespace LoanShark.API.Models
{
    public class SocialUserViewModel
    {
        public int UserID { get; set; }
        public string Cnp { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string HashedPassword { get; set; }
        public string Username { get; set; }
        public int ReportedCount { get; set; }

    }

}
