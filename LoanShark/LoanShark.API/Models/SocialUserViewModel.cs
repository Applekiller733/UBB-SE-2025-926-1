using LoanShark.Domain;

namespace LoanShark.API.Models
{
    public class SocialUserViewModel
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

    }

}
