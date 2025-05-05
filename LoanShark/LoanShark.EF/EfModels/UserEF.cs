using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoanShark.EF.EFModels
{
    public class UserEF
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }

        [Required]
        public string Cnp { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string HashedPassword { get; set; }

        [Required]
        public string PasswordSalt { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public int ReportedCount { get; set; }

        public string? TimeoutEnd { get; set; }

        public string? FriendsSerialized { get; set; }

        public string? ChatsSerialized { get; set; }
    }
}
