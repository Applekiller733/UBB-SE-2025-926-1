namespace LoanShark.API.Models
{
    public class UserViewModel
    {
        public int UserID { get; set; }
        public string Cnp { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public int ReportedCount { get; set; }
    }

    public class CreateUserDto
    {
        public string Cnp { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Password { get; set; } = null!;
    }


}

